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

            PrintFile("Default Encoding", @"TestFiles\ANSI.csv", Encoding.Default);
            PrintFile("Default Encoding", @"TestFiles\UTF-8.csv", Encoding.Default);
            Console.ReadLine();
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
