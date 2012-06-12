using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {

	class Program {
		static void Main(string[] args) {
			Item.Attribute.Initialize();
			Item.RarityLevel.Initialize();
			Item.Kind.Initialize();
			Random r = new Random();
			for (; ; ) {
				Item.Generate(1, r).WriteStatBlock();
				Console.WriteLine();
				Console.WriteLine();
				Console.ReadKey(false);
			}
		}
	}

	public static class StringUtils {
		public static string PadCenter(this string s, int width, char c) {
			if (s == null || width <= s.Length) return s;

			int padding = width - s.Length;
			return s.PadLeft((padding / 2) + s.Length, c).PadRight(width, c);
		}

		public static string DeCamelCase(this string s) {
			return System.Text.RegularExpressions.Regex.Replace(s, @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", @" ${a}");
		}

		public static string UpperCaseFirst(this string s) {
			return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}
	}

}
