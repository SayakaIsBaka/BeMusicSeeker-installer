using RGiesecke.DllExport;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace BeMusicSeeker_installer_configmigrate
{
    public class Class1
    {
        static Char[] s_Base32Char = {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h',
            'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', '0', '1', '2', '3', '4', '5'};

        public static string ToBase32StringSuitableForDirName(byte[] buff)
        {
            StringBuilder sb = new StringBuilder();
            byte b0, b1, b2, b3, b4;
            int l, i;

            l = buff.Length;
            i = 0;

            // Create l chars using the last 5 bits of each byte.  
            // Consume 3 MSB bits 5 bytes at a time.

            do
            {
                b0 = (i < l) ? buff[i++] : (byte)0;
                b1 = (i < l) ? buff[i++] : (byte)0;
                b2 = (i < l) ? buff[i++] : (byte)0;
                b3 = (i < l) ? buff[i++] : (byte)0;
                b4 = (i < l) ? buff[i++] : (byte)0;

                // Consume the 5 Least significant bits of each byte
                sb.Append(s_Base32Char[b0 & 0x1F]);
                sb.Append(s_Base32Char[b1 & 0x1F]);
                sb.Append(s_Base32Char[b2 & 0x1F]);
                sb.Append(s_Base32Char[b3 & 0x1F]);
                sb.Append(s_Base32Char[b4 & 0x1F]);

                // Consume 3 MSB of b0, b1, MSB bits 6, 7 of b3, b4
                sb.Append(s_Base32Char[(
                    ((b0 & 0xE0) >> 5) |
                    ((b3 & 0x60) >> 2))]);

                sb.Append(s_Base32Char[(
                    ((b1 & 0xE0) >> 5) |
                    ((b4 & 0x60) >> 2))]);

                // Consume 3 MSB bits of b2, 1 MSB bit of b3, b4

                b2 >>= 5;

                if ((b3 & 0x80) != 0)
                    b2 |= 0x08;
                if ((b4 & 0x80) != 0)
                    b2 |= 0x10;

                sb.Append(s_Base32Char[b2]);

            } while (i < l);

            return sb.ToString();
        }

        private static string GetExeLocalAppDataUserConfigPath(string fullExePath)
        {
            //E.g.: fullExePath = @"C:\Program Files (x86)\MyExeFolder\MyProgram.exe"
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var versionInfo = FileVersionInfo.GetVersionInfo(fullExePath);
            var companyName = "BeMusicSeeker"; // versionInfo.CompanyName returns nothing
            var exeName = versionInfo.OriginalFilename;// or 'AppDomain.CurrentDomain.FriendlyName'

            var assemblyName = AssemblyName.GetAssemblyName(fullExePath);
            var version = assemblyName.Version.ToString();

            var uri = "file:///" + fullExePath; //or 'assemblyName.CodeBase' if vshost (you can check the 'FriendlyName')
            uri = uri.ToUpperInvariant();

            var ms = new MemoryStream();
            var bSer = new BinaryFormatter();
            bSer.Serialize(ms, uri);
            ms.Position = 0;
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(ms);
            var hashstring = ToBase32StringSuitableForDirName(hash);

            //<AppData Local User Path> + <Company Name> + <[ExeName]_[eid]_[Hash]> + <Version> + user.config
            var userConfigLocalAppDataPath = localAppDataPath + "\\" + companyName + "\\" + exeName + "_Url_" + hashstring + "\\" + version + "\\" + "user.config";

            return userConfigLocalAppDataPath;
        }

        public static string FindPreviousConfigPath()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            localAppDataPath += "\\Apps\\2.0\\Data";
            var confFiles = Directory.GetFiles(localAppDataPath, "user.config", SearchOption.AllDirectories);
            var candidate = "";
            var candidateDateTime = new DateTime(0);
            foreach (var path in confFiles)
            {
                var p = path.Split(Path.DirectorySeparatorChar);
                if (p.Length >= 3 && p[p.Length - 4].StartsWith("bemu..tion")) {
                    var pDateTime = File.GetLastWriteTime(path);
                    if (pDateTime > candidateDateTime)
                    {
                        candidate = path;
                        candidateDateTime = pDateTime;
                    }
                }
            }
            return candidate;
        }

        [DllExport("MigrateConfig", CallingConvention = CallingConvention.StdCall)]
        public static void MigrateConfig([MarshalAs(UnmanagedType.LPWStr)]string bemusicFolderPath)
        {
            try
            {
                string newConfigPath = GetExeLocalAppDataUserConfigPath(Path.Combine(bemusicFolderPath, "BeMusicSeeker.exe"));
                string prevConfigPath = FindPreviousConfigPath();
                Directory.CreateDirectory(Path.GetDirectoryName(newConfigPath));
                File.Copy(prevConfigPath, newConfigPath);
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
