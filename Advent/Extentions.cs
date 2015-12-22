using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent
{
	public static class Extentions
	{
		public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
		{
			return k == 0 ? new[] { new T[0] } :
			  elements.SelectMany((e, i) =>
				elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
		}

		public static IEnumerable<int> AllIndexesOf(this string str, string value)
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentException("the string to find may not be empty", "value");

			for (var index = 0; ; index += value.Length)
			{
				index = str.IndexOf(value, index);
				if (index == -1)
					break;
				yield return index;
			}
		}
	}
}
