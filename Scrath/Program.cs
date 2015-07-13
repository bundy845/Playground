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
            
            var stream = new FileStream(@"TestFiles\Aeromexico Hierarchy V2.csv",FileMode.Open,FileAccess.Read);
            using (var streamReader = new StreamReader(stream, Encoding.Default))
            {
                var csvReader = new CsvReader(streamReader);

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

                Console.ReadLine();

            }

        }
    }
}
