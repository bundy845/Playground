using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent
{
	class Values
	{
		public int Min { get; set; }
		public int Max { get; set; }
	}

	class Reindeer
	{
		public string Name { get; set; }
		public int Speed { get; set; }
		public int FlyTime { get; set; }
		public int RestTime { get; set; }
		public int CurrentDistance { get; set; }
		public int Score { get; set; }

	}

	class Ingredient
	{
		public string Name { get; set; }
		public int Capacity { get; set; }
		public int Durability { get; set; }
		public int Flavor { get; set; }
		public int Texture { get; set; }
		public int Calories { get; set; }
	}

	class AuntSue
	{
		
		public int? Children { get; set; }
		public int? Cats { get; set; }
		public int? Samoyeds { get; set; }
		public int? Pomeranians { get; set; }
		public int? Akitas { get; set; }
		public int? Goldfish { get; set; }
		public int? Trees { get; set; }
		public int? Cars { get; set; }
		public int? Perfumes { get; set; }
		public int Number { get; set; }
		public int? Vizslas { get; set; }


	}

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
			//Console.Write("Day 8 " + Day8());
			//Console.WriteLine("Day 9 " + Day9());
			//Console.WriteLine("Day 10 " + Day10());
			//var password = Day11("cqjxjnds");
			//Console.WriteLine("Day 11 " + password);
			//Console.WriteLine("Day 11 again " + Day11(password));
			//Console.WriteLine("Day 12 " + Day12A());
			//Console.WriteLine("Day 12 " + Day12B());
			//Console.WriteLine("Day 13A ");
			//Day13();
			//Console.WriteLine("Day 13B ");
			//Day13(true);
			//Day14();

			//var sw = new Stopwatch();
			//sw.Start();
			//Day15();
			//sw.Stop();
			//Console.WriteLine($"Day 15 elapsed time {sw.ElapsedMilliseconds}" );
			//Day17();
			//Console.WriteLine("Day 16 " + Day16());
			//Console.WriteLine("Day 17 " + Day17());
			//Day18();
			Day19();
			Console.ReadLine();
		}

		public static void Day19()
		{
			var input = "CRnCaSiRnBSiRnFArTiBPTiTiBFArPBCaSiThSiRnTiBPBPMgArCaSiRnTiMgArCaSiThCaSiRnFArRnSiRnFArTiTiBFArCaCaSiRnSiThCaCaSiRnMgArFYSiRnFYCaFArSiThCaSiThPBPTiMgArCaPRnSiAlArPBCaCaSiRnFYSiThCaRnFArArCaCaSiRnPBSiRnFArMgYCaCaCaCaSiThCaCaSiAlArCaCaSiRnPBSiAlArBCaCaCaCaSiThCaPBSiThPBPBCaSiRnFYFArSiThCaSiRnFArBCaCaSiRnFYFArSiThCaPBSiThCaSiRnPMgArRnFArPTiBCaPRnFArCaCaCaCaSiRnCaCaSiRnFYFArFArBCaSiThFArThSiThSiRnTiRnPMgArFArCaSiThCaPBCaSiRnBFArCaCaPRnCaCaPMgArSiRnFYFArCaSiThRnPBPMgAr";
			var stuff = GetInput(19);

			var set = new HashSet<string>();
			foreach (var replacement in stuff)
			{
				var x = Regex.Split(replacement, " => ");
				var y = x.First();
				var allOccurrences = input.AllIndexesOf(y);

                foreach (var index in allOccurrences)
				{
					var newChemical = "";
					if (index > 0)
						newChemical += input.Substring(0, index);

					newChemical = newChemical + x.Last();

					if (index < input.Length - y.Length)
						newChemical = newChemical + input.Substring(index  + y.Length);

					set.Add(newChemical);
				}

			}

			Console.WriteLine($"Number of chemicals {set.Count}");
		}

		public static void Day18()
		{
			var input = GetInput(18);
			var dimensionSize = 100;
			var runs = 100;

			var lights = new int[dimensionSize, dimensionSize];

			for (var i = 0; i < dimensionSize; i++)
			{
				var row = input.Skip(i).Take(1).Single().ToCharArray();
				for (var j = 0; j < dimensionSize; j++)
				{
					lights[i, j] = row[j] == '#' ? 1 : 0;
				}
			}

			//DisplayLights(lights);
			InitLightsForConway(lights);

			for (var i = 0; i < runs; i++)
			{
				var dummyArray = new int[dimensionSize, dimensionSize];
				Array.Copy(lights, dummyArray, lights.Length);

				for (var row = 0; row < dimensionSize; row++)
				{
					for (var column = 0; column < dimensionSize; column++)
					{
						var thisValue = lights[row, column];


						var neighborsOn = NeighborsOn(lights, row, column);

						if (thisValue == 1)
						{
							if (neighborsOn == 2 || neighborsOn == 3)
								dummyArray[row, column] = 1;
							else
								TurnOffLights(dummyArray, row, column);
						}
						else
						{
							if (neighborsOn == 3)
								dummyArray[row, column] = 1;
							else
							{
								TurnOffLights(dummyArray, row, column);
							}
						}

					}
				}

				Array.Copy(dummyArray, lights, lights.Length);
				//DisplayLights(lights);
			}

			var lightsOn = 0;
			for (var row = 0; row < dimensionSize; row++)
			{
				for (var column = 0; column < dimensionSize; column++)
				{
					if (lights[row, column] == 1) lightsOn++;
				}
			}


			Console.WriteLine("Lights on " + lightsOn);
		}

		private static void InitLightsForConway(int[,] lights)
		{
			var nonZeroEdge = lights.GetLength(0) - 1;

			lights[0, 0] = 1;
			lights[0, nonZeroEdge] = 1;
			lights[nonZeroEdge, nonZeroEdge] = 1;
			lights[nonZeroEdge, 0] = 1;
		}
		private static void TurnOffLights(int[,] lights, int row, int column)
		{
			var nonZeroEdge = lights.GetLength(0) - 1;
			if ((row == 0 && column == 0) ||
			    (row == 0 && column == nonZeroEdge) ||
			    (row == nonZeroEdge && column == nonZeroEdge) ||
			    (row == nonZeroEdge && column == 0))
			{
				return;
			}

			lights[row, column] = 0;
		}

		private static int NeighborsOn(int[,] lights, int row, int column)
		{
			return GetValueWithWrap(lights, row, column - 1) +
			       GetValueWithWrap(lights, row, column + 1) +
			       GetValueWithWrap(lights, row + 1, column + 1) +
			       GetValueWithWrap(lights, row + 1, column) +
			       GetValueWithWrap(lights, row + 1, column - 1) +
			       GetValueWithWrap(lights, row - 1, column - 1) +
			       GetValueWithWrap(lights, row - 1, column) +
			       GetValueWithWrap(lights, row - 1, column + 1);

		}

		private static int GetValueWithWrap(int[,] lights, int row, int column)
		{
			if (row < 0 || row > lights.GetLength(0) -1) return 0;
			if (column < 0 || column > lights.GetLength(1) - 1) return 0;

			return lights[row, column];
		}


		private static void DisplayLights(int[,] lights)
		{
			Console.WriteLine("&&&&&&&&&&");
			for (var row = 0; row < lights.GetLength(0); row++)
			{
				for (var column = 0; column < lights.GetLength(1); column++)
				{
					Console.Write(lights[row, column]);
				}
				Console.WriteLine();
			}

		}

		public static int Day17()
		{
			var list = new [] { 11, 30, 47, 31, 32, 36, 3, 1, 5, 3, 32, 36, 15, 11, 46, 26, 28, 1, 19, 3 };
			var size = 150;

			var count = 0;
			var min = 0;
			for (var i = 2; i <= list.Length; i++)
			{
				var x = list.Combinations(i).ToList();
				var y  = x.Count(a => a.Sum() == size);

				if (y > 0 && min == 0)
					min = y;

				count += y;

			}

			Console.WriteLine("Min containers for day 17 " + min);

			return count;
		}

		public static int Day16()
		{
			var sample = new AuntSue
			{
				Children = 3,
				Cats = 7,
				Samoyeds = 2,
				Akitas = 0,
				Pomeranians = 3,
				Goldfish = 5,
				Vizslas = 0,
                Trees = 3,
				Cars = 2,
				Perfumes = 1
			};

			var aunts = new List<AuntSue>();
			foreach (var line in GetInput(16))
			{
				//Sue 470: cats: 1, perfumes: 5, vizslas: 9
				var aunt = new AuntSue();

				var firstColon = line.IndexOf(":");

				aunt.Number = int.Parse(line.Substring(0, firstColon).Replace("Sue ", "").Replace(":", ""));

				var parts = line.Substring(firstColon+2).Split(',');
				foreach(var part in parts)
				{
					var x = part.Split(':');
					var value = int.Parse(x[1]);
					var property = x[0].Replace(":","").Trim().ToLower();

					switch (property)
					{
						case "cats":
							aunt.Cats = value;
							break;
						case "children":
							aunt.Children = value;
							break;
						case "samoyeds":
							aunt.Samoyeds = value;
							break;
						case "akitas":
							aunt.Akitas = value;
							break;
						case "goldfish":
							aunt.Goldfish = value;
							break;
						case "trees":
							aunt.Trees = value;
							break;
						case "cars":
							aunt.Cars = value;
							break;
						case "pomeranians":
							aunt.Pomeranians = value;
							break;
						case "perfumes":
							aunt.Perfumes = value;
							break;
						case "vizslas":
							aunt.Vizslas = value;
							break;
                        default:
							throw new Exception();
					}
					
				}
				aunts.Add(aunt);
			}

			var possible = aunts.Where(a => a.Cars == sample.Cars || a.Cars == null)
				.Where(a => a.Akitas == sample.Akitas || a.Akitas == null)
				.Where(a => a.Cats > sample.Cats || a.Cats == null)
				.Where(a => a.Children == sample.Children || a.Children == null)
				.Where(a => a.Goldfish < sample.Goldfish || a.Goldfish == null)
				.Where(a => a.Trees > sample.Trees || a.Trees == null)
				.Where(a => a.Perfumes == sample.Perfumes || a.Perfumes == null)
				.Where(a => a.Pomeranians < sample.Pomeranians || a.Pomeranians == null)
				.Where(a => a.Vizslas == sample.Vizslas || a.Vizslas == null)
				.Where(a => a.Samoyeds == sample.Samoyeds || a.Samoyeds == null);

			return possible.FirstOrDefault().Number;
		}

	    private static void Day15()
	    {
		    var input = GetInput(15);

		    var ingredients = input.Select(ingredient => ingredient.Split(' ')).Select(z => new Ingredient
		    {
			    Name = z[0].Replace(":", ""),
				Calories = int.Parse(z[10].Replace(",", "")),
				Texture = int.Parse(z[8].Replace(",", "")),
				Flavor = int.Parse(z[6].Replace(",", "")),
				Durability = int.Parse(z[4].Replace(",", "")),
				Capacity = int.Parse(z[2].Replace(",", "")),
		    }).ToList();

		    var maxCount = 100;
		    var bestScore = 0;
		    var bestCalorieScore = 0;
		    var calorieCount = 500;
		    for (var i = 0; i <= maxCount; i++)
		    {
			    for (var j = 0; j <= (maxCount - i); j++)
			    {
				    for (var k = 0; k <= (maxCount - i - j); k++)
				    {
					    var l = maxCount - i - j - k;

						var c = BoundNumber(ingredients[0].Capacity * i + 
											ingredients[1].Capacity * j +
											ingredients[2].Capacity * k +
											ingredients[3].Capacity * l);
						var d = BoundNumber(ingredients[0].Durability * i +
											ingredients[1].Durability * j +
											ingredients[2].Durability *k +
											ingredients[3].Durability * l);
						var f = BoundNumber(ingredients[0].Flavor * i + 
											ingredients[1].Flavor * j +
											ingredients[2].Flavor * k +
											ingredients[3].Flavor * l);
						var t = BoundNumber(ingredients[0].Texture * i + 
											ingredients[1].Texture * j +
											ingredients[2].Texture * k +
											ingredients[3].Texture * l);

					    var calories = BoundNumber(ingredients[0].Calories*i +
					                               ingredients[1].Calories*j +
					                               ingredients[2].Calories*k +
					                               ingredients[3].Calories*l);

						var score = c * d * f * t;

					    if (calories == calorieCount)
						    bestCalorieScore = Math.Max(bestCalorieScore, score);

						bestScore = Math.Max(bestScore, score);
					}
				}
			}
			Console.WriteLine($"Day 15 Best Score {bestScore}");
			Console.WriteLine($"Day 15 Best Score {bestCalorieScore}");
		}

	    private static int BoundNumber(int number)
	    {
		    return number < 0 ? 0 : number;
	    }

	    private static void Day14()
		{
			var input = GetInput(14);
			var flightTime = 2503;

			var r = input.Select(reindeer => reindeer.Split(' ')).Select(parts => new Reindeer
			{
				Name = parts[0], Speed = int.Parse(parts[3]), FlyTime = int.Parse(parts[6]), RestTime = int.Parse(parts[13])
			}).ToList();

			var max = 0;
			foreach (var reindeer in r)
			{
				var d = DistanceAfterTime(reindeer.Speed, reindeer.FlyTime, reindeer.RestTime, flightTime);

				max = Math.Max(max, d);
				Console.WriteLine($"{reindeer.Name} totalDistance = {d}");
			}

			Console.WriteLine($"Further = {max}");


			for(var i = 1; i <= flightTime; i++)
			{
				foreach (var reindeer in r)
				{
					reindeer.CurrentDistance = DistanceAfterTime(reindeer.Speed, reindeer.FlyTime, reindeer.RestTime, i);
				}

				var furthest = r.Max(a => a.CurrentDistance);
				foreach (var reindeer in r.Where(a => a.CurrentDistance == furthest))
				{
					reindeer.Score++;
				}
			}

			var mostPoints = r.OrderByDescending(a => a.Score).First();
			Console.WriteLine($"Point winner is {mostPoints.Name} with {mostPoints.Score}");
		}

		private static int DistanceAfterTime(int speed, int flyTime, int restTime, int totalTime)
		{
			var fullSegmentTime = restTime + flyTime;
			var wholeSegments = totalTime/ fullSegmentTime;

			var distance = speed*flyTime*wholeSegments;

			var x = totalTime - wholeSegments* fullSegmentTime;

			if (x > flyTime)
				distance += speed*flyTime;
			else
				distance += speed*x;

			return distance;
		}

		private static void Day13(bool addExtra = false, int happyness = 0)
		{
			var input = GetInput(13);

			var rules = new Dictionary<string, int>();
			var people = new List<string>();

			foreach (var item in input)
			{
				var parts = item.Replace(".", "").Split(' ');

				var value = int.Parse(parts[3]);

				AddItemIfNotExist(people, parts[0]);
				AddItemIfNotExist(people, parts[10]);

				if (parts[2] == "lose") value = -1 * value;

				AddItemIfNotExist(rules, $"{parts[0]}{parts[10]}", value);
			}

			if (addExtra)
			{
				foreach (var person in people)
				{
					AddItemIfNotExist(rules, $"RickB{person}", happyness);
					AddItemIfNotExist(rules, $"{person}RickB", happyness);
				}
				people.Add("RickB");
			}

			var values = FindMinMax(people.ToArray(), rules, true);
			Console.WriteLine("Max = " + values.Max);
			Console.WriteLine("Min = " + values.Min);
		}

		private static void AddItemIfNotExist<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (!dict.ContainsKey(key))
				dict.Add(key, value);
		}
		private static void AddItemIfNotExist(ICollection<string> list, string item)
		{
			if (!list.Contains(item))
				list.Add(item);
		}

		private static long Day12B()
		{
			var input = GetStringInput(12);

			dynamic x = JsonConvert.DeserializeObject(input);

			return GetSum(x, "red");
		}

		private static long GetSum(JObject o, string avoid)
		{
			var shouldSkip = o.Properties()
				.Select(a => a.Value).OfType<JValue>()
				.Select(v => v.Value).Contains(avoid);

			return shouldSkip ? 0 : o.Properties().Sum((dynamic a) => (long) GetSum(a.Value, avoid));
		}

		static long GetSum(JArray arr, string avoid) => arr.Sum((dynamic a) => (long)GetSum(a, avoid));

		static long GetSum(JValue val, string avoid) => val.Type == JTokenType.Integer ? (long)val.Value : 0;

		private static int Day12A()
		{
			var input = GetStringInput(12);
			var matches = Regex.Matches(input, @"-?[0-9]+");

			return matches.Cast<object>().Sum(match => int.Parse(match.ToString()));
		}

		private static string Day11(string input)
		{
			input = AdvancePassword(input);
			while (!IsPasswordValid(input))
			{
				input = AdvancePassword(input);
			}
			return input;
		}

		private static bool IsPasswordValid(string password)
		{

			var consecutive = false;

			for (var i = 0; i < password.Length - 2; i++)
			{
				var firstCharacter = password[i];
				var secondChacter = password[i + 1];
				if (secondChacter == IncrementChar(firstCharacter, 1) &&
				    password[i + 2] == IncrementChar(secondChacter, 1))
				{
					consecutive = true;
					break;
				}
			}

			return consecutive && ThirdRule(password);
		}

		private static bool ThirdRule(string s)
		{
			char first = ' ', second = ' ';
			for (var i = 0; i < s.Length - 1; i++)
			{
				if (s[i] == s[i + 1])
				{
					if (first == ' ')
						first = s[i];
					else
						second = s[i];
				}
			}
			return first != ' ' && second != ' ' && first != second;
		}

		private static char IncrementChar(char c, int amount)
		{
			return (char) (Convert.ToUInt16(c) + amount);
		}
		private static string AdvancePassword(string current)
		{
			var r = current.Reverse();
			var sb = new StringBuilder();

			var done = false;

			foreach (var c in r)
			{
				if (!done)
				{
					done = true;

					if (c == 'z')
					{
						sb.Append('a');
						done = false;
					}
					else if (c == 'h' || c == 'n' || c == 'k')
						sb.Append(IncrementChar(c,2));
					else
						sb.Append(IncrementChar(c,1));
				}
				else
					sb.Append(c);
			}

			Reverse(sb);
			return sb.ToString();
		}

		public static void Reverse( StringBuilder text)
		{
			if (text.Length <= 1) return;
			var pivotPos = text.Length / 2;
			for (var i = 0; i < pivotPos; i++)
			{
				var iRight = text.Length - (i + 1);
				var rightChar = text[i];
				var leftChar = text[iRight];
				text[i] = leftChar;
				text[iRight] = rightChar;
			}
		}

		private static int Day10()
		{
			var input = "3113322113";
			var times = 50;

			//Console.WriteLine(input);
			for (var i = 0; i < times; i++)
			{ 
				var temp = new StringBuilder();
				var previous = input.First();
				var counter = 0;
				foreach (var c in input)
				{
					if (previous == c)
						counter++;
					else
					{
						temp.Append($"{counter}{previous}");
						counter = 1;
						previous = c;
					}
				}
				temp.Append($"{counter}{previous}");
				//Console.WriteLine(temp);
				input = temp.ToString();
			}

			return input.Length;

		}

		private static int Day9()
		{
			var lines = GetInput(9);

			//Tristram to AlphaCentauri = 34

			//FUCK ME why didn't MST work? Oh well, let's brute force it
			var x = new List<string>();
			var sizes = new Dictionary<string, int>();
			foreach (var parts in lines.Select(line => line.Split(' ')))
			{
				if (!x.Contains(parts[0]))
					x.Add(parts[0]);

				if (!x.Contains(parts[2]))
					x.Add(parts[2]);

				sizes.Add($"{parts[0]}{parts[2]}", int.Parse(parts[4]));
				sizes.Add($"{parts[2]}{parts[0]}", int.Parse(parts[4]));
			}

			var values = FindMinMax(x.ToArray(), sizes);
			Console.WriteLine("Min " + values.Min);
			Console.WriteLine("Max " + values.Max);
			return 0;
		}

		private static Values FindMinMax(string[] values, IReadOnlyDictionary<string, int> sizes, bool addStart = false )
		{
			var perms = GetPermutations(values);
			var min = int.MaxValue;
			var max = 0;
			var longestList = new List<string>();

			foreach (var perm in perms)
			{
				var x = new List<string>(perm);

				if (addStart) x.Add(perm[0]);

				var distance = 0;
				for (var i = 0; i < x.Count - 1; i++)
				{
					distance += sizes[$"{x[i]}{x[i + 1]}"];
					if (addStart)
						distance+= sizes[$"{x[i+1]}{x[i]}"];
				}

				if (max > distance)
					longestList = x;

				min = Math.Min(min, distance);
				max = Math.Max(max, distance);
			}

			Console.WriteLine(string.Join("->", longestList));

			return new Values {Min = min, Max = max};
		}

		private static IEnumerable<T[]> GetPermutations<T>(T[] values)
		{
			if (values.Length == 1)
				return new[] { values };

			return values.SelectMany(v => GetPermutations(values.Except(new[] { v }).ToArray()),
				(v, p) => new[] { v }.Concat(p).ToArray());
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

		private static string GetStringInput(int day)
		{
			return File.ReadAllText($@"Input\Day{day}.txt");
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
