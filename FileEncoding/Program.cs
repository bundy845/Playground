using CsvHelper;
using System;
using System.IO;
using System.Text;

namespace FileEncoding
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {

            // files saved as ANSI (probably from Excel as a CSV in a country with codepage 1252) are not being read correctly
            // this program shows a file saved as ansi and one as UTF and how they render from the stream
            // it's not pretty
            // in order to see this correctly in the console you need to modify the console to use a TT font

            SetEncodingAndPrint("Default", Encoding.Default);
            SetEncodingAndPrint("Ascii", Encoding.ASCII);
            SetEncodingAndPrint("Unicode", Encoding.Unicode);
            SetEncodingAndPrint("UTF-8", Encoding.UTF8);
            SetEncodingAndPrint("UTF-7", Encoding.UTF7);
            SetEncodingAndPrint("1252 Codepage", Encoding.GetEncoding(1252)); 
            AutoDetectOptions();
            Console.ReadLine();
        }

        private static void AutoDetectOptions()
        {
            AutoDetectFile(@"TestFiles\ANSI.csv");
            AutoDetectFile(@"TestFiles\UTF-8.csv");
        }

        private static void AutoDetectFile(string file)
        {
            string text;
            var encoding = AutoDetect.DetectTextEncoding(file, out text);
            Console.WriteLine("{0} detected as {1}",file, encoding.EncodingName);
            PrintFile(encoding.EncodingName, file, encoding);
        }

        private static void SetEncodingAndPrint(string encodingName, Encoding encoding)
        {
            PrintFile(encodingName + " - Ansi File", @"TestFiles\ANSI.csv", encoding);
            PrintFile(encodingName + " - UTF-8 File", @"TestFiles\UTF-8.csv", encoding);
        }

        private static void PrintFile(string type, string file, Encoding encoding)
        {
            Console.WriteLine("***" + type);

            using(var stream = new FileStream(file,FileMode.Open,FileAccess.Read))
            using (var streamReader = new StreamReader(stream, encoding))
            {
                var csvReader = new CsvReader(streamReader);
                Console.OutputEncoding = encoding;
                var i = 0;
                while (csvReader.Read() && i < 10)
                {
                    foreach (var field in csvReader.CurrentRecord)
                    {
                        Console.Write(field);
                        Console.Write("\t");
                    }
                    Console.WriteLine();
                    i++;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
                
        }


    }
}
