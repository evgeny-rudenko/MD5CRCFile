using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
namespace md5file
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length==1)
            {
                if (File.Exists(args[0]))
                {
                    string filenamewoe = Path.GetFileNameWithoutExtension(args[0]);
                    string filename = args[0];
                    string mdhash = CalculateMD5(filename);

                    File.WriteAllText(filenamewoe+ ".crc", mdhash);
                }
            }
            else
            {
                string executablepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                DirectoryInfo d = new DirectoryInfo(executablepath);
                FileInfo[] Files = d.GetFiles(); 
                string str = "";
                foreach (FileInfo file in Files)
                {
                    string extension = Path.GetExtension(file.Name);
                    if (extension != ".rar")
                        continue;
                    if (!File.Exists(Path.GetFileNameWithoutExtension(file.Name)+".crc"))
                    {
                        string mdhash = CalculateMD5(file.Name);
                        File.WriteAllText(Path.GetFileNameWithoutExtension(file.Name) + ".crc", mdhash);
                    }
                }
            }
        
        }
        static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }

    }
}
