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
				Item.Generate(r.Next(1, 20), r).WriteStatBlock();
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

		public static List<string> Wrap(this string s, int width) {
			List<string> list = s.Split(' ').ToList<string>();
			Queue<string> queue = new Queue<string>(list);
			List<string> newList = new List<string>();
			while (queue.Count > 0) {
				string newLine = "";
				int chr = 0;
				while (chr < width && queue.Count > 0) {
					chr += queue.Peek().Length + (newLine == "" ? 0 : 1);
					if (chr <= width) newLine += (newLine == "" ? queue.Dequeue() : " " + queue.Dequeue());
				}
				newList.Add(newLine);
			}
			return newList;
		}

		public static int BreakLine(this string s, int pos, int max) {
			// Find last whitespace in line
			int i = max - 1;
			while (i >= 0 && !Char.IsWhiteSpace(s[pos + i]))
				i--;
			if (i < 0)
				return max; // No whitespace found; break at maximum length

			// Find start of whitespace
			while (i >= 0 && Char.IsWhiteSpace(s[pos + i]))
				i--;

			// Return length of text before whitespace
			return i + 1;
		}
	}

}
