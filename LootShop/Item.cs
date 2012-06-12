using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootShop {
	public class Item {
		public class RarityLevel {
			public enum Type {
				Garbage,
				Normal,
				Magic,
				Rare,
				Legendary,
				Unique
			}

			public Type Name;
			public Range AttributeCount;
			public Range AttributeModifier;

			public static List<RarityLevel> List = new List<RarityLevel>();

			public static void Initialize() {
				List.Add(new RarityLevel(Type.Garbage,		new Range(0, 0),	new Range(0, 0.75)));
				List.Add(new RarityLevel(Type.Normal,		new Range(0, 0),	new Range(0.75, 1.25)));
				List.Add(new RarityLevel(Type.Magic,		new Range(2, 4),	new Range(1.25, 1.75)));
				List.Add(new RarityLevel(Type.Rare,			new Range(3, 5),	new Range(1.5, 2.0)));
				List.Add(new RarityLevel(Type.Legendary,	new Range(4, 6),	new Range(1.75, 2.5)));
				List.Add(new RarityLevel(Type.Unique,		new Range(5, 7),	new Range(2.5, 3)));
			}

			public static RarityLevel Lookup(RarityLevel.Type type) {
				RarityLevel level =
					(from r in RarityLevel.List
					 where r.Name == type
					 select r).FirstOrDefault<RarityLevel>();
				return level;
			}

			public override string ToString() {
				return Name.ToString().DeCamelCase();
			}

			public RarityLevel(Type name, Range attributeCount, Range attributeRange) {
				Name = name;
				AttributeCount = attributeCount;
				AttributeModifier = attributeRange;
			}
		}
		public class Attribute {
			public enum Type {
				Strength,
				Dexterity,
				Intelligence,
				Vitality,
				MagicFind,
				GoldFind,
				AttackSpeed
			}

			public Type Name;

			public static List<Attribute> List = new List<Attribute>();

			public static void Initialize() {
				List.Add(new Attribute(Type.Strength));
				List.Add(new Attribute(Type.Dexterity));
				List.Add(new Attribute(Type.Intelligence));
				List.Add(new Attribute(Type.Vitality));
				List.Add(new Attribute(Type.MagicFind));
				List.Add(new Attribute(Type.GoldFind));
				List.Add(new Attribute(Type.AttackSpeed));
			}

			public static Attribute Lookup(Attribute.Type type) {
				Attribute attr =
					(from r in Attribute.List
					 where r.Name == type
					 select r).FirstOrDefault<Attribute>();
				return attr;
			}

			public Attribute(Type name) {
				Name = name;
			}

		}

		public Dictionary<Attribute.Type, int> Attributes = new Dictionary<Attribute.Type, int>();
		public RarityLevel Rarity;
		public int Level;

		public static Item Generate(int level, Random r) {
			Item i = new Item();
			i.Level = level;

			// Choose a random rarity
			// TODO: Weight this shit
			Item.RarityLevel.Type[] rarityValues = (Item.RarityLevel.Type[])Enum.GetValues(typeof(Item.RarityLevel.Type));
			Item.RarityLevel.Type selectedRarity = rarityValues[r.Next(0, rarityValues.Length)];
			i.Rarity = RarityLevel.Lookup(selectedRarity);

			List<Item.Attribute> attrs = new List<Item.Attribute>();
			int attrCount = i.Rarity.AttributeCount.RandomInt(r);
			List<Item.Attribute.Type> takenAttrs = new List<Attribute.Type>();
			while (attrs.Count < attrCount) {
				Item.Attribute.Type[] values = (Item.Attribute.Type[]) Enum.GetValues(typeof(Item.Attribute.Type));
				Item.Attribute.Type selectedName = values[r.Next(0, values.Length)];
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
				int attrVal = 1;
				int baseVal = 10;

				attrVal += Math.Max((baseVal * Math.Max(i.Level - 1, 1)) + r.Next(baseVal * 2), 1);
				attrVal = Math.Max((int)((double)attrVal * i.Rarity.AttributeModifier.RandomDouble(r)), 1);

				i.Attributes.Add(a.Name, attrVal);
			}

			return i;
		}
		public static ConsoleColor RarityToConsoleColor(RarityLevel raritylevel) {
			switch (raritylevel.Name) {
				case RarityLevel.Type.Garbage:
					return ConsoleColor.DarkGray;
				case RarityLevel.Type.Normal:
					return ConsoleColor.White;
				case RarityLevel.Type.Magic:
					return ConsoleColor.DarkCyan;
				case RarityLevel.Type.Rare:
					return ConsoleColor.Yellow;
				case RarityLevel.Type.Legendary:
					return ConsoleColor.DarkMagenta;
				case RarityLevel.Type.Unique:
					return ConsoleColor.Magenta;
				default:
					return ConsoleColor.Red;
			}
		}

		public string Name {
			get {
				return "Item Name";
			}
		}
		public void WriteStatBlock() {
			int width = 25;
			string line = new String('-', width);
			string _slot = "1-Hand";
			string _type = "Axe";
			Console.WriteLine(line);
			Console.ForegroundColor = RarityToConsoleColor(Rarity);
			Console.WriteLine(Name.ToUpper().PadCenter(width, ' '));
			Console.ForegroundColor = RarityToConsoleColor(Rarity);

			string type = (Rarity.Name == RarityLevel.Type.Normal ? "" : Rarity.ToString() + " ") + _type;

			Console.Write(type);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(_slot.PadLeft(width - type.Length) + "\n");
			Console.WriteLine();
			Console.ResetColor();
			if (Attributes.Count > 0) {
				foreach (KeyValuePair<Attribute.Type, int> kvp in Attributes) {
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(kvp.Key.ToString() + "\t");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write(kvp.Value.ToString() + "\n");
					Console.ResetColor();
				}
				Console.WriteLine();
			}
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("Required Level\t");
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write(Level + "\n");
			Console.ResetColor();
			Console.WriteLine(line);
		}

	}
}