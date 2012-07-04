using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LootSystem {

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
			if (s != null) {
				if (s.Length > 1)
					return char.ToUpper(s[0]) + s.Substring(1);
				else
					return s.ToUpper();
			}
			return s;
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
	}

	public static class EnumHelper {
		public static T[] GetValues<T>() {
			Type enumType = typeof(T);

			if (!enumType.IsEnum) {
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			}

			List<T> values = new List<T>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (FieldInfo field in fields) {
				object value = field.GetValue(enumType);
				values.Add((T)value);
			}

			return values.ToArray();
		}

		public static object[] GetValues(Type enumType) {
			if (!enumType.IsEnum) {
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			}

			List<object> values = new List<object>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (FieldInfo field in fields) {
				object value = field.GetValue(enumType);
				values.Add(value);
			}

			return values.ToArray();
		}
	}
}
