using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent
{
	class Program
	{
		static void Main(string[] args)
		{
			//Day4A();
			//Day4B();
			//Console.WriteLine("Day 5A " + Day5A());
			//Console.WriteLine("Day 5B " + Day5B());
			//Console.WriteLine("Day 6A " + Day6A());
			//Console.WriteLine("Day 6B " + Day6B());
			//Day7Setup();
			//Console.WriteLine("Day 7A " + Day7("a"));
			//Console.WriteLine("Day 7B " + Day7B("b", "a"));
			Console.Write("Day 8 " + Day8());
			Console.ReadLine();
		}

		private static int Day8()
		{
			var lines = GetInput(8);

			var totalCharacters = 0;
			var actualCharacters = 0;
			var encodedCharacters = 0;
			foreach (var line in lines)
			{
				totalCharacters += line.Length;

				var i = 0;
				while (i < line.Length)
				{
					var character = line[i];

					switch (character)
					{
						case '"':
							i++;
							encodedCharacters += 3;
							break;
						case '\\':
							encodedCharacters += 2;
							actualCharacters++;
							var nextCharacter = line[i + 1];
							if (nextCharacter == '"' || nextCharacter == '\\')
							{
							
								i += 2;
								encodedCharacters += 2;
							}
							else
							{
								i += 4;
								encodedCharacters += 3;
							}
							break;
						default:
							i++;
							actualCharacters++;
							encodedCharacters += 1;
							break;
					}
				}
			}
			return encodedCharacters - totalCharacters;
			//return totalCharacters - actualCharacters;
		}

		private static IEnumerable<string> GetInput(int day)
		{
			return File.ReadAllLines($@"Input\Day{day}.txt");

		}

		private static int Day5A()
		{
			return GetInput(5).Count(IsDay5AValid);
		}

		private static int Day5B()
		{
			return GetInput(5).Count(IsDay5BValid);
		}

		private static int Day6(Action<string, int[,]> parser)
		{
			var input = GetInput(6);
			var lights = new int[1000, 1000];

			foreach (var instruction in input)
			{
				parser(instruction, lights);
			}

			var total = 0;
			for (var x = 0; x < 1000; x++)
			{
				for (var y = 0; y < 1000; y++)
				{
					total += lights[x, y];
				}
			}

			return total;
		}

		private static int Day6A()
		{
			return Day6(ParseLights);
		}

		private static int Day6B()
		{
			return Day6(ParseLights2);
		}


		private static Dictionary<string, string> _formulas =  new Dictionary<string, string>(); 
		private static Dictionary<string, ushort> _wireValues = new Dictionary<string,ushort>();

		private static void Day7Setup()
		{
			foreach (var stuff in GetInput(7).Select(instruction => Regex.Split(instruction, " -> ")))
			{
				_formulas.Add(stuff[1], stuff[0]);
			}
		}

		private static ushort Day7B(string wireToOverwrite, string wireToWriteWith)
		{
			_formulas[wireToOverwrite] = Day7(wireToWriteWith).ToString();
			_wireValues = new Dictionary<string, ushort>();

			return Day7(wireToWriteWith);
		}

		private static ushort Day7(string wire)
		{


			ushort value;
			if (ushort.TryParse(wire, out value))
			{
				return value;
			}
			ushort result =0;

			if (!_wireValues.ContainsKey(wire))
			{ 
				var parts = _formulas[wire].Split(' ');
				ushort left;
				if (parts[0] == "NOT")
				{
					left = (ushort)~Day7(parts[1]);
				}
				else
				{
					left = Day7(parts[0]);
				}
			
				if (parts.Length < 3)
					result = left;
				else
				{
					var right = Day7(parts[2]);

					switch (parts[1])
					{
						case "AND":
							result = (ushort)(left & right);
							break;
						case "OR":
							result = (ushort)(left | right);
							break;
						case "RSHIFT":
							result = (ushort)(left >> right);
							break;
						case "LSHIFT":
							result = (ushort)(left << right);
							break;
					}

				}

				_wireValues[wire] = result;

			}

			return _wireValues[wire];

		}

		private static void ParseLights(string s, int[,] grid)
		{
			var m = Regex.Match(s, "(.*) ([0-9]*),([0-9]*) through ([0-9]*),([0-9]*)", RegexOptions.Compiled);

			var instr = m.Groups[1].Value.ToLower();
			var x1 = int.Parse(m.Groups[2].Value);
			var y1 = int.Parse(m.Groups[3].Value);
			var x2 = int.Parse(m.Groups[4].Value);
			var y2 = int.Parse(m.Groups[5].Value);

			for(var x = x1; x <= x2; x++)
			{
				for (var y = y1; y <= y2; y++)
				{
					if (instr.Contains("turn on"))
						grid[x, y] = 1;
					else if (instr.Contains("turn off"))
						grid[x, y] = 0;
					else if (instr.Contains("toggle"))
						grid[x, y] = grid[x, y] == 0 ? 1 : 0;

				}
			}

		}

		private static void ParseLights2(string s, int[,] grid)
		{
			var m = Regex.Match(s, "(.*) ([0-9]*),([0-9]*) through ([0-9]*),([0-9]*)", RegexOptions.Compiled);

			var instr = m.Groups[1].Value.ToLower();
			var x1 = int.Parse(m.Groups[2].Value);
			var y1 = int.Parse(m.Groups[3].Value);
			var x2 = int.Parse(m.Groups[4].Value);
			var y2 = int.Parse(m.Groups[5].Value);

			for (var x = x1; x <= x2; x++)
			{
				for (var y = y1; y <= y2; y++)
				{
					if (instr.Contains("turn on"))
						grid[x, y]++;
					else if (instr.Contains("turn off"))
						grid[x, y] = grid[x,y] == 0 ? 0 : grid[x,y]--;
					else if (instr.Contains("toggle"))
						grid[x, y]+=2;

				}
			}

		}

		private static bool IsDay5BValid(string input)
		{
			
			var doublePair = Regex.IsMatch(input, @"(..).*\1");
			var sandwich = Regex.IsMatch(input, @"(.).\1");

			return doublePair && sandwich;

		}

		private static bool IsDay5AValid(string input)
		{
			var vowels = Regex.Matches(input, "[aeiou]").Count;
			var hasRepeat = Regex.IsMatch(input, @"(.)\1");
			var hasBad = Regex.IsMatch(input, "ab|cd|pq|xy");

			return vowels >= 3 && hasRepeat && !hasBad;
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
