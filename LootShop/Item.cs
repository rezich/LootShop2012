using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {
	public enum Attribute {
		Strength,
		Intelligence,
		Dexterity,
		Vitality,
		AttackSpeed,
		GoldFind,
		MagicFind
	}
	public enum RarityLevel {
		Garbage,
		Normal,
		Magic,
		Rare,
		Legendary,
		Unique
	}
	class Item {
		public Dictionary<Attribute, int> Attributes = new Dictionary<Attribute, int>();
		public RarityLevel Rarity;
		public static Item Generate(Random r) {
			Item i = new Item();
			i.Rarity = (RarityLevel)r.Next(Enum.GetValues(typeof(RarityLevel)).Length);
			List<Attribute> attrs = Enum.GetValues(typeof(Attribute)).Cast<Attribute>().ToList<Attribute>();
			for (int a = 0; a < 1 + r.Next(attrs.Count - 1); a++) {
				int n = r.Next(attrs.Count - 1);
				Attribute attr = attrs[n];
				i.Attributes.Add(attr, r.Next(32));
				attrs.RemoveAt(n);
			}
			return i;
		}

		public string Name {
			get {
				return "[ITEM NAME]";
			}
		}

		public static ConsoleColor RarityToConsoleColor(RarityLevel raritylevel) {
			switch (raritylevel) {
				case RarityLevel.Garbage:
					return ConsoleColor.Gray;
				case RarityLevel.Normal:
					return ConsoleColor.White;
				case RarityLevel.Magic:
					return ConsoleColor.DarkCyan;
				case RarityLevel.Rare:
					return ConsoleColor.Yellow;
				case RarityLevel.Legendary:
					return ConsoleColor.DarkMagenta;
				case RarityLevel.Unique:
					return ConsoleColor.Magenta;
				default:
					return ConsoleColor.Cyan;
			}
		}

		public void WriteStatBlock() {
			Console.ForegroundColor = RarityToConsoleColor(Rarity);
			Console.WriteLine(Name);
			switch (Rarity) {
				case RarityLevel.Garbage:
					Console.WriteLine("(Garbage item)");
					break;
				case RarityLevel.Magic:
					Console.WriteLine("(Magic item)");
					break;
				case RarityLevel.Rare:
					Console.WriteLine("(Rare item)");
					break;
				case RarityLevel.Legendary:
					Console.WriteLine("(Legendary item)");
					break;
				case RarityLevel.Unique:
					Console.WriteLine("(Unique item)");
					break;
			}
			Console.ResetColor();
			foreach (KeyValuePair<Attribute, int> kvp in Attributes) {
				Console.WriteLine(kvp.Key.ToString() + "\t" + kvp.Value.ToString());
			}
		}

	}
}