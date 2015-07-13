using CsvHelper;
using System;
using System.IO;
using System.Text;

namespace Scrath
{
    class Program
    {
        static void Main(string[] args)
        {

           SetEncodingAndPrint("Default", Encoding.Default);
           SetEncodingAndPrint("Ascii", Encoding.ASCII);
           SetEncodingAndPrint("Unicode", Encoding.Unicode);
           SetEncodingAndPrint("UTF-8", Encoding.UTF8);
           SetEncodingAndPrint("UTF-7", Encoding.UTF7);
           SetEncodingAndPrint("1252 Codepage", Encoding.GetEncoding(1252)); 
           Console.ReadLine();
        }

        private static void SetEncodingAndPrint( string encodingName, Encoding encoding)
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
