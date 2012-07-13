﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LootSystem;

namespace LootTester {
	class Program {
		static void Main(string[] args) {
			bool done = false;
			Random r = new Random();
			Time time = new Time();
			time.Month = 12;
			time.Day = 29;
			ConsoleKeyInfo cki;
			Item.Initialize();
			do {
				cki = Console.ReadKey(true);
				switch (cki.Key) {
					case ConsoleKey.Enter:
						goto case ConsoleKey.Spacebar;
					case ConsoleKey.Spacebar:
						//WriteStatBlock(Item.Generate(r.Next(1, 20), r));
						time.TimeOfDay++;
						//time.Day += 2;
						Console.WriteLine(time);
						Console.WriteLine();
						break;
					case ConsoleKey.Escape:
						done = true;
						break;
				}
			} while (!done);
		}

		static void WriteStatBlock(Item i) {
			int width = 34;
			int padding = 3;
			string leftPadding = new String(' ', 40 - (width / 2));
			string bar = "│";
			//string line = "+" + new String('─', width - 2) + "+";
			string line = new String('─', width - 2);
			string empty = bar + new String(' ', width - 2) + bar;

			Console.WriteLine(leftPadding + "┌" + line + "┐");
			List<string> name = i.Name.ToUpper().Wrap(width - 4);
			foreach (string s in name) {
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(leftPadding + bar + " ");
				Console.ForegroundColor = RarityToConsoleColor(i.Rarity);
				Console.Write(s.PadCenter(width - 4, ' '));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(" " + bar + "\n");
			}
			//Console.WriteLine(Name.ToUpper().PadCenter(width, ' '));
			Console.ResetColor();
			Console.WriteLine(leftPadding + "├" + line + "┤");

			string type = (i.Rarity.Name == Item.RarityLevel.Type.Normal ? "" : i.Rarity.ToString() + " ") + i.Variety.Name;

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(leftPadding + bar + " ");

			Console.ForegroundColor = RarityToConsoleColor(i.Rarity);
			Console.Write(type);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(i.Variety.Slot.ToString().DeCamelCase().PadLeft(width - type.Length - 4) + " " + bar + "\n");
			Console.WriteLine(leftPadding + empty);
			Console.ResetColor();

			foreach (KeyValuePair<Item.Attribute.Type, double> kvp in i.StandardAttributes) {
				string key = new String(' ', padding) + kvp.Key.ToString().DeCamelCase();
				string num = ((Item.Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Item.Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(leftPadding + bar + key);
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(num.PadLeft(width - key.Length - padding - 2) + new String(' ', padding));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(bar + "\n");
				Console.ResetColor();
			}

			Console.WriteLine(leftPadding + empty);

			foreach (KeyValuePair<Item.Attribute.Type, double> kvp in i.NonstandardAttributes) {
				string num = ((Item.Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Item.Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				List<string> key = Item.Attribute.Lookup(kvp.Key).NonstandardListing.Replace("@", num).Wrap(width - 4);

				foreach (string s in key) {
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(leftPadding + bar + " ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(s.PadCenter(width - 4, ' '));
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(" " + bar + "\n");
				}

				Console.ResetColor();
			}

			if (i.NonstandardAttributes.Count > 0) Console.WriteLine(leftPadding + empty);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(leftPadding + bar);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(("Required Level: " + i.Level).PadCenter(width - 2, ' '));
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(bar + "\n");
			Console.ResetColor();
			Console.WriteLine(leftPadding + "└" + line + "┘");
		}

		static ConsoleColor RarityToConsoleColor(Item.RarityLevel raritylevel) {
			switch (raritylevel.Name) {
				case Item.RarityLevel.Type.Garbage:
					return ConsoleColor.DarkGray;
				case Item.RarityLevel.Type.Normal:
					return ConsoleColor.White;
				case Item.RarityLevel.Type.Magic:
					return ConsoleColor.DarkCyan;
				case Item.RarityLevel.Type.Rare:
					return ConsoleColor.Yellow;
				case Item.RarityLevel.Type.Legendary:
					return ConsoleColor.DarkMagenta;
				case Item.RarityLevel.Type.Unique:
					return ConsoleColor.Magenta;
				default:
					return ConsoleColor.Red;
			}
		}
	}
}
