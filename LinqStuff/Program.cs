using System;
using System.Linq;

namespace LinqStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] numbers1 = { 2.0, 2.1, 2.2, 2.3, 2.4, 2.5 };
            double[] numbers2 = { 2.2, 2.6 };

            var onlyInFirstSet = numbers1.Except(numbers2);

            var any = onlyInFirstSet.Any();

            foreach (var number in onlyInFirstSet)
                Console.WriteLine(number);

            Console.WriteLine("were there any? {0}", any);

            Console.ReadLine();
        }
    }
}
