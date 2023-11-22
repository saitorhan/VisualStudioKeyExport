using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisualStudioKeyExport
{
    class Program
    {
        static readonly List<Product> VsList = new List<Product>
        {
            new Product("Visual Studio 2013 Professional"     , "E79B3F9C-6543-4897-BBA5-5BFB0A02BB5C", "06177"),

            new Product("Visual Studio 2015 Enterprise"       , "4D8CFBCB-2F6A-4AD2-BABF-10E28F6F2C8F", "07060"),
            new Product("Visual Studio 2015 Professional"     , "4D8CFBCB-2F6A-4AD2-BABF-10E28F6F2C8F", "07062"),

            new Product("Visual Studio 2017 Enterprise"       , "5C505A59-E312-4B89-9508-E162F8150517", "08860"),
            new Product("Visual Studio 2017 Professional"     , "5C505A59-E312-4B89-9508-E162F8150517", "08862"),
            new Product("Visual Studio 2017 Test Professional", "5C505A59-E312-4B89-9508-E162F8150517", "08866"),

            new Product("Visual Studio 2019 Enterprise"       , "41717607-F34E-432C-A138-A3CFD7E25CDA", "09260"),
            new Product("Visual Studio 2019 Professional"     , "41717607-F34E-432C-A138-A3CFD7E25CDA", "09262"),

            new Product("Visual Studio 2022 Enterprise"       , "1299B4B9-DFCC-476D-98F0-F65A2B46C96D", "09660"),
            new Product("Visual Studio 2022 Professional"     , "1299B4B9-DFCC-476D-98F0-F65A2B46C96D", "09662"),
        };

        static List<string> Licenses = new List<string>();
        static void Main()
        {
            Console.WriteLine("Visual Studio Keys Found");
            Console.WriteLine("Bulunan Visual Studio Anahtarları");
            Console.WriteLine("-----");
            foreach (var product in VsList) FindLicense(product);

            Console.WriteLine("-----");
            Console.WriteLine("Press Y to write the found Licenses to the file.");
            Console.WriteLine("Bulunan Lisansları dosyaya yazmak için Y tuşuna basın.");
            if (string.Compare(Console.ReadLine(), "y", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                // Get user's desktop path
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string file = $"visualStudiu_{DateTime.Now:yyyyMMddHHmmss}.txt";
                path = Path.Combine(path, file);
                System.IO.File.WriteAllLines(path, Licenses);
                Console.WriteLine($"\nLicenses were written to the {file} file on the desktop.");
                Console.WriteLine($"\nLisanslar masaüstünde {file} dosyasına yazıldı.");
            }

            Console.ReadLine();
        }

        private static void FindLicense(Product product)
        {
            string license = String.Empty;
            var encrypted = Registry.GetValue($"HKEY_CLASSES_ROOT\\Licenses\\{product.GUID}\\{product.MPC}", "", null);
            if (encrypted == null) return;
            try
            {
                var secret = ProtectedData.Unprotect((byte[])encrypted, null, DataProtectionScope.CurrentUser);
                var unicode = new UnicodeEncoding();
                var str = unicode.GetString(secret);
                foreach (var sub in str.Split('\0'))
                {
                    var match = Regex.Match(sub, @"\w{5}-\w{5}-\w{5}-\w{5}-\w{5}");
                    if (match.Success)
                    {
                        license = $"{product.Name}: {match.Captures[0]}";
                        Console.WriteLine(license);
                        Licenses.Add(license);
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
