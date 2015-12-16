using System;
using System.Security.Cryptography;
using System.Text;

namespace Advent
{
	class Program
	{
		static void Main(string[] args)
		{
			Day4A();
			Day4B();
			Console.ReadLine();
		}


		private static void Day4A()
		{
			const string input = "iwrupvqb";
			var hash = MD5.Create();
			for (var i = 0; ; ++i)
			{
				var dataToEncode = input + i;
				var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(dataToEncode));
				if (bytes[0] ==0 && bytes[1] ==0 && bytes[2] < 10 )
				{
					Console.WriteLine("MD 5 = " + i);
					return;
				}
			}
		}

		private static void Day4B()
		{
			const string input = "iwrupvqb";
			var hash = MD5.Create();
			for (var i = 0; ; ++i)
			{
				var dataToEncode = input + i;
				var bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(dataToEncode));
				if (bytes[0] ==0 && bytes[1] ==0 && bytes[2] ==0)
				{
					Console.WriteLine("MD 5 = " + i);
					return;
				}
			}
		}
	}
}
