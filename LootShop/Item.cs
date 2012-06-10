using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootShop {
	public enum RarityLevel {
		Garbage,
		Normal,
		Magic,
		Rare,
		Legendary,
		Unique
	}
	public class Item {

		public class Attribute {

			public enum Names {
				Strength,
				Dexterity,
				Intelligence,
				Vitality,
				MagicFind,
				GoldFind,
				AttackSpeed
			}

			public Names Name;

			public static List<Attribute> List {
				get {
					List<Attribute> l = new List<Attribute>();
					l.Add(new Attribute(Names.Strength));
					l.Add(new Attribute(Names.Dexterity));
					l.Add(new Attribute(Names.Intelligence));
					l.Add(new Attribute(Names.Vitality));
					l.Add(new Attribute(Names.MagicFind));
					l.Add(new Attribute(Names.GoldFind));
					l.Add(new Attribute(Names.AttackSpeed));
					return l;
				}
			}
			public Attribute(Names name) {
				Name = name;
			}
		}

		public Dictionary<Attribute.Names, int> Attributes = new Dictionary<Attribute.Names, int>();
		public RarityLevel Rarity;
		public int Level;
		public static Item Generate(int level, Random r) {
			Item i = new Item();
			i.Level = level;
			i.Rarity = (RarityLevel)r.Next(Enum.GetValues(typeof(RarityLevel)).Length);

			List<Item.Attribute> attrs = new List<Item.Attribute>();
			int attrCount = 4;
			List<Item.Attribute.Names> takenAttrs = new List<Attribute.Names>();
			while (attrs.Count < attrCount) {
				Item.Attribute.Names[] values = (Item.Attribute.Names[]) Enum.GetValues(typeof(Item.Attribute.Names));
				Item.Attribute.Names selectedName = values[r.Next(0, values.Length)];
				Item.Attribute newAttr =
					(from t in Item.Attribute.List
					where t.Name == selectedName && !takenAttrs.Contains(t.Name)
					select t).FirstOrDefault<Item.Attribute>();
				if (newAttr != null) {
					attrs.Add(newAttr);
					takenAttrs.Add(newAttr.Name);
				}
			}

			foreach (Item.Attribute a in attrs) {
				i.Attributes.Add(a.Name, r.Next(64));
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
			Console.WriteLine("--------------------");
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
			Console.WriteLine();
			Console.ResetColor();
			foreach (KeyValuePair<Attribute.Names, int> kvp in Attributes) {
				Console.WriteLine(kvp.Key.ToString() + "\t" + kvp.Value.ToString());
			}
			Console.WriteLine();
			Console.WriteLine("Required Level\t" + Level);
			Console.WriteLine("--------------------");
		}

	}
}