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

	public static class Extensions {
		/// <summary> 
		/// Removes all elements from the List that match the conditions defined by the specified predicate. 
		/// </summary> 
		/// <typeparam name="T">The type of elements held by the List.</typeparam> 
		/// <param name="list">The List to remove the elements from.</param> 
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to remove.</param> 
		public static int RemoveAll<T>(this System.Collections.Generic.List<T> list, Func<T, bool> match) {
			int numberRemoved = 0;

			// Loop through every element in the List, in reverse order since we are removing items. 
			for (int i = (list.Count - 1); i >= 0; i--) {
				// If the predicate function returns true for this item, remove it from the List. 
				if (match(list[i])) {
					list.RemoveAt(i);
					numberRemoved++;
				}
			}

			// Return how many items were removed from the List. 
			return numberRemoved;
		}

		/// <summary> 
		/// Returns true if the List contains elements that match the conditions defined by the specified predicate. 
		/// </summary> 
		/// <typeparam name="T">The type of elements held by the List.</typeparam> 
		/// <param name="list">The List to search for a match in.</param> 
		/// <param name="match">The Predicate delegate that defines the conditions of the elements to match against.</param> 
		public static bool Exists<T>(this System.Collections.Generic.List<T> list, Func<T, bool> match) {
			// Loop through every element in the List, until a match is found. 
			for (int i = 0; i < list.Count; i++) {
				// If the predicate function returns true for this item, return that at least one match was found. 
				if (match(list[i]))
					return true;
			}

			// Return that no matching elements were found in the List. 
			return false;
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
