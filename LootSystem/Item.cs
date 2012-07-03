using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootSystem {
	public class Item {
		public static List<Tuple<string, WordQuality>> PreAdjectives = new List<Tuple<string, WordQuality>>();
		public static List<Tuple<string, WordQuality>> OfX = new List<Tuple<string, WordQuality>>();
		public enum Type {
			Greataxe,
			Longsword,
			Greatsword,
			Staff,
			Bow,
			Mace,
			Dagger,

			Helmet,
			Pauldrons,
			Armor,
			Pants,
			Gloves,
			Belt,
			Boots,

			Ring,
			Amulet
		}
		public enum Slot {
			OneHand,
			TwoHands,

			Head,
			Shoulders,
			Chest,
			Hands,
			Waist,
			Legs,
			Feet,

			Finger,
			Neck
		}
		public enum WordQuality {
			Neutral,
			Bad,
			Good
		}

		public class Kind {
			public Type Name;
			public Slot Slot;
			public List<Attribute> BaseAttributes;
			public List<string> Names;

			public static List<Item.Kind> List = new List<Item.Kind>();

			public static void Initialize() {
				List<Attribute> meleeAttr = new List<Attribute> { Attribute.Lookup(Attribute.Type.Damage), Attribute.Lookup(Attribute.Type.AttacksPerSecond) };
				List<Attribute> armorAttr = new List<Attribute> { Attribute.Lookup(Attribute.Type.Armor) };
				List<Attribute> pureAttr = new List<Attribute> { };

				// Weapons
				List.Add(new Item.Kind(Type.Longsword, Slot.OneHand, meleeAttr, new List<string> {
					"Sword", "Rapier", "Blade", "Cutlass", "Sabre", "Falchion", "Short Sword", "Longsword", "Scimitar"
				}));
				List.Add(new Item.Kind(Type.Greatsword, Slot.TwoHands, meleeAttr, new List<string> {
					"Greatsword", "Broadsword", "Bastard Sword", "Zweihander", "Claymore", "Flamberge"
				}));
				List.Add(new Item.Kind(Type.Dagger, Slot.OneHand, meleeAttr, new List<string> {
					"Dagger", "Knife", "Letter Opener", "Stabber", "Dirk", "Stiletto", "Point", "Spike"
				}));
				List.Add(new Item.Kind(Type.Greataxe, Slot.TwoHands, meleeAttr, new List<string> {
					"Axe", "Hatchet", "Pick", "Broad Axe", "Battleaxe", "Great Axe", "Giant Axe", "Cleaver", "Pickaxe"
				}));
				List.Add(new Item.Kind(Type.Staff, Slot.OneHand, meleeAttr, new List<string> {
					"Staff", "Rod", "Wand", "Implement", "Quarterstaff", "Walking Stick", "Channeler"
				}));
				List.Add(new Item.Kind(Type.Bow, Slot.TwoHands, meleeAttr, new List<string> {
					"Bow", "Shortbow", "Longbow", "Composite Bow", "War Bow", "Siege Bow", "Great Bow", "Cross Bow"
				}));
				List.Add(new Item.Kind(Type.Mace, Slot.OneHand, meleeAttr, new List<string> {
					"Mace", "Club", "Morningstar", "Flail", "War Hammer", "Maul", "Cudgel", "Truncheon", "Mallet", "Hammer"
				}));

				// Armor
				List.Add(new Item.Kind(Type.Helmet, Slot.Head, armorAttr, new List<string> {
					"Helmet", "Hat", "Cap", "Crown", "Casque", "Basinet", "Sallet", "Helm", "Headpiece"
				}));
				List.Add(new Item.Kind(Type.Armor, Slot.Chest, armorAttr, new List<string> {
					"Studded Armor", "Leather Armor", "Quilted Armor", "Ring Mail", "Scale Mail", "Chain Mail", "Breastplate", "Plate Mail", "Shirt", "Bodyshaft", "Sweater", "Rags", "Undershirt", "Robe", "Cloak", "Goatskin"
				}));
				List.Add(new Item.Kind(Type.Pants, Slot.Legs, armorAttr, new List<string> {
					"Pants", "Leggings", "Kneepads", "Chaps", "Leg-Wraps", "Lederhosen", "Kilt", "Faulds"
				}));
				List.Add(new Item.Kind(Type.Gloves, Slot.Hands, armorAttr, new List<string> {
					"Gloves", "Gauntlets", "Bracers", "Mitts", "Vambraces", "Graspers", "Stranglers"
				}));
				List.Add(new Item.Kind(Type.Belt, Slot.Waist, armorAttr, new List<string> {
					"Sash", "Belt", "Coil", "Girdle", "Belly-Wrap", "String", "Strap", "Strand", "Scabbard"
				}));
				List.Add(new Item.Kind(Type.Boots, Slot.Feet, armorAttr, new List<string> {
					"Shoes", "Boots", "Greaves", "Sandals", "Galoshes", "Kicks", "Treads"
				}));
				List.Add(new Item.Kind(Type.Pauldrons, Slot.Shoulders, armorAttr, new List<string> {
					"Shoulders", "Pauldrons", "Mantle", "Shoulderpads", "Spaulders"
				})); ;

				// Pure Attributehavers
				List.Add(new Item.Kind(Type.Ring, Slot.Finger, pureAttr, new List<string> {
					"Ring", "Band"
				}));
				List.Add(new Item.Kind(Type.Amulet, Slot.Neck, pureAttr, new List<string> {
					"Amulet", "Necklass", "Charm"
				}));
			}

			public static Kind Lookup(Type type) {
				Kind kind =
					(from r in Kind.List
					 where r.Name == type
					 select r).FirstOrDefault<Kind>();
				return kind;
			}

			public Kind(Type type, Slot slot, List<Attribute> baseAttributes, List<string> names) {
				Name = type;
				Slot = slot;
				BaseAttributes = baseAttributes;
				Names = names;
			}

		}
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
			public bool Magic;

			public static List<RarityLevel> List = new List<RarityLevel>();

			public static void Initialize() {
				List.Add(new RarityLevel(Type.Garbage,		new Range(0, 0),	new Range(-0.5, 0),		false));
				List.Add(new RarityLevel(Type.Normal,		new Range(0, 0),	new Range(0, 0.5),	false));
				List.Add(new RarityLevel(Type.Magic,		new Range(2, 4),	new Range(0.25, 0.75),	true));
				List.Add(new RarityLevel(Type.Rare,			new Range(3, 5),	new Range(1.5, 2.0),	true));
				List.Add(new RarityLevel(Type.Legendary,	new Range(4, 6),	new Range(1.75, 2.5),	true));
				List.Add(new RarityLevel(Type.Unique,		new Range(5, 7),	new Range(2.5, 3),		true));
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

			public RarityLevel(Type name, Range attributeCount, Range attributeRange, bool magic) {
				Name = name;
				AttributeCount = attributeCount;
				AttributeModifier = attributeRange;
				Magic = magic;
			}
		}
		public class Attribute {
			public enum Type {
				Damage,
				AttacksPerSecond,
				Armor,
				Strength,
				Dexterity,
				Intelligence,
				Vitality,
				MagicFind,
				GoldFind,
				AttackSpeed,
				ResistFire,
				ResistLightning,
				ResistPoison,
				LifeOnHit,
				ReplenishLife,
			}

			public Type Name;
			public bool BaseStat = false;
			public double BaseValue;
			public double ModAdd;
			public double ModMultiply;
			public double LevelBaseValue;
			public double LevelModAdd;
			public double LevelModMultiply;
			public double RarityBaseValue;
			public double RarityModAdd;
			public double RarityModMultiply;

			public bool Rounded;
			public bool Percentage;
			public bool Addition;

			public static List<Attribute> List = new List<Attribute>();

			public static void Initialize() {
				//																				Base	Mod+	Mod*	LBase	LMod+	LMod*	RBase	RMod+	RMod*
				List.Add(new Attribute(Type.Damage,				true,	true,	false,	false,		0,		0,		0,		15,		14,		0,		15,		15,		0));
				List.Add(new Attribute(Type.AttacksPerSecond,	true,	false,	false,	false,		1.5,	1,		0,		0,		0,		0,		0,		0,		0));
				List.Add(new Attribute(Type.Armor,				true,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0));
				List.Add(new Attribute(Type.Strength,			false,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0));
				List.Add(new Attribute(Type.Dexterity,			false,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0));
				List.Add(new Attribute(Type.Intelligence,		false,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0));
				List.Add(new Attribute(Type.Vitality,			false,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0));
				List.Add(new Attribute(Type.MagicFind,			false,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0));
				List.Add(new Attribute(Type.GoldFind,			false,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0));
				List.Add(new Attribute(Type.AttackSpeed,		false,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0));
				List.Add(new Attribute(Type.ResistFire,			false,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0));
				List.Add(new Attribute(Type.ResistLightning,	false,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0));
				List.Add(new Attribute(Type.ResistPoison,		false,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0));
				List.Add(new Attribute(Type.LifeOnHit,			false,	true,	false,	false,		0,		0,		0,		4,		4,		0,		4,		4,		0));
				List.Add(new Attribute(Type.ReplenishLife,		false,	true,	false,	false,		0,		0,		0,		4,		4,		0,		4,		4,		0));
			}

			public static Attribute Lookup(Attribute.Type type) {
				Attribute attr =
					(from r in Attribute.List
					 where r.Name == type
					 select r).FirstOrDefault<Attribute>();
				return attr;
			}

			public double Generate(RarityLevel rarity, int level, Random r) {
				double val = BaseValue;

				val += LevelBaseValue * level;

				val += RarityBaseValue * rarity.AttributeModifier.RandomDouble(r);

				if (ModAdd != 0) val = val - ModAdd + (r.NextDouble() * ModAdd * 2);

				if (RarityModAdd != 0) val += rarity.AttributeModifier.RandomDouble(r) * RarityModAdd;

				return Math.Round(val, (Rounded ? 0 : 2));
			}

			public Attribute(Type name, bool baseStat, bool rounded, bool addition, bool percentage, double baseValue, double modAdd, double modMultiply, double levelBaseValue, double levelModAdd, double levelModMultiply, double rarityBaseValue, double rarityModAdd, double rarityModMultiply) {
				Name = name;
				BaseStat = baseStat;
				Rounded = rounded;
				Percentage = percentage;
				Addition = addition;

				BaseValue = baseValue;					// y = x
				ModAdd = modAdd;						// y += x
				ModMultiply = modMultiply;				// ???

				LevelBaseValue = levelBaseValue;		// y += level * x
				LevelModAdd = levelModAdd;				// y += (level * x) - x + (x * 2)
				LevelModMultiply = levelModMultiply;	// ???

				RarityBaseValue = rarityBaseValue;		// y += rarity * x
				RarityModAdd = rarityModAdd;			// y += (rarity * x) - x + (x * 2)
				RarityModMultiply = rarityModMultiply;	// ???
			}

		}
		public class Modifier : INotifyPropertyChanged {
			public enum Type {
				Adjective,
				OfTheX
			}

			protected string name;
			public string Name {
				get { return name; }
				set { name = value; OnPropertyChanged("Name"); }
			}

			protected List<string> tags;
			public List<string> Tags {
				get { return tags; }
				set { tags = value; OnPropertyChanged("Tags"); }
			}

			protected Type kind;
			public Type Kind {
				get { return kind; }
				set { kind = value; OnPropertyChanged("Kind"); }
			}

			public List<Submodifier> Submodifiers;

			public static Item.Modifier.ListType List = new Item.Modifier.ListType();

			public Modifier(string name, Type kind, List<string> tags) {
				Name = name;
				Kind = kind;
				Tags = tags;
				List.Add(this);
			}

			public Modifier() {
				List.Add(this);
			}

			public event PropertyChangedEventHandler PropertyChanged;

			protected void OnPropertyChanged(string info) {
				PropertyChangedEventHandler handler = PropertyChanged;
				if (handler != null) {
					handler(this, new PropertyChangedEventArgs(info));
				}
			}

			public override string ToString() {
				return Name;
			}

			public class ListType : ObservableCollection<Item.Modifier> {
				public static ListType Load() {
					// TODO: Load logic
					return new ListType();
				}
				public void Save() {
					// TODO: Save logic
				}
			}
		}
		public class Submodifier {
			public List<Type> Types;
			public List<Modification> Modifications;
		}
		public class Modification {
		}

		public Dictionary<Attribute.Type, double> Attributes = new Dictionary<Attribute.Type, double>();
		public RarityLevel Rarity;
		public int Level;
		public Kind Variety;

		public static void Initialize() {
			foreach (string s in System.IO.File.ReadAllLines(@"Data\PreAdjectives.txt")) {
				if (s == "") continue;
				WordQuality q = WordQuality.Neutral;
				string str = s;
				if (s.Substring(s.Length - 1, 1) == "+") {
					q = WordQuality.Good;
					str = s.Substring(0, s.Length - 1);
				}
				if (s.Substring(s.Length - 1, 1) == "-") {
					q = WordQuality.Bad;
					str = s.Substring(0, s.Length - 1);
				}
				Item.PreAdjectives.Add(new Tuple<string, WordQuality>(str, q));
			}
			foreach (string s in System.IO.File.ReadAllLines(@"Data\OfX.txt")) {
				if (s == "") continue;
				WordQuality q = WordQuality.Neutral;
				string str = s;
				if (s.Substring(s.Length - 1, 1) == "+") {
					q = WordQuality.Good;
					str = s.Substring(0, s.Length - 1);
				}
				if (s.Substring(s.Length - 1, 1) == "-") {
					q = WordQuality.Bad;
					str = s.Substring(0, s.Length - 1);
				}
				Item.OfX.Add(new Tuple<string, WordQuality>(str, q));
			}
			//Item.PreAdjectives = System.IO.File.ReadAllLines(@"Data\PreAdjectives.txt").ToList<string>();
			//Item.OfX = System.IO.File.ReadAllLines(@"Data\OfX.txt").ToList<string>();
			Attribute.Initialize();
			RarityLevel.Initialize();
			Kind.Initialize();
		}
		public static Item Generate(int level, Random r) {
			Item i = new Item();
			i.Level = level;

			// Choose a random item kind
			Item.Type[] kindValues = (Item.Type[])Enum.GetValues(typeof(Item.Type));
			Item.Type selectedKind = kindValues[r.Next(0, kindValues.Length)];
			i.Variety = Kind.Lookup(selectedKind);

			// Choose a random rarity
			// TODO: Weight this shit
			Item.RarityLevel.Type[] rarityValues = (Item.RarityLevel.Type[])Enum.GetValues(typeof(Item.RarityLevel.Type));
			Item.RarityLevel.Type selectedRarity = RarityLevel.Type.Normal;

			// Pick a random rarity, but if the type of item is one with no base stats (ring, amulet),
			// keep rolling until you get a "Magic" one (blue or higher)
			do {
				selectedRarity = rarityValues[r.Next(0, rarityValues.Length)];
			} while (i.Variety.BaseAttributes.Count == 0 && !RarityLevel.Lookup(selectedRarity).Magic);

			i.Rarity = RarityLevel.Lookup(selectedRarity);

			List<Item.Attribute> attrs = new List<Item.Attribute>();

			foreach (Attribute attr in i.Variety.BaseAttributes) {
				attrs.Add(attr);
			}

			int attrCount = i.Rarity.AttributeCount.RandomInt(r);
			List<Item.Attribute.Type> takenAttrs = new List<Attribute.Type>();
			while (attrs.Count < attrCount) {
				Item.Attribute.Type[] values = (Item.Attribute.Type[]) Enum.GetValues(typeof(Item.Attribute.Type));
				Item.Attribute.Type selectedName = values[r.Next(0, values.Length)];
				Item.Attribute newAttr =
					(from t in Item.Attribute.List
					where t.Name == selectedName && !takenAttrs.Contains(t.Name) && t.BaseStat == false
					select t).FirstOrDefault<Item.Attribute>();
				if (newAttr != null) {
					attrs.Add(newAttr);
					takenAttrs.Add(newAttr.Name);
				}
			}

			foreach (Item.Attribute a in attrs) {
				//*int attrVal = 1;
				//int baseVal = 10;

				//attrVal += Math.Max((baseVal * Math.Max(i.Level - 1, 1)) + r.Next(baseVal * 2), 1);
				//attrVal = Math.Max((int)((double)attrVal * i.Rarity.AttributeModifier.RandomDouble(r)), 1);*/

				i.Attributes.Add(a.Name, a.Generate(i.Rarity, i.Level, r));
			}

			// GENERATE THE NAME!!
			int oddsOfOfX = 2;
			string name = "";
			List<Tuple<string, WordQuality>> preAdjectives;
			List<Tuple<string, WordQuality>> ofX;

			// TODO: Rewrite to use RarityLevel's "good" and "bad" word odds
			switch (i.Rarity.Name) {
				case RarityLevel.Type.Garbage:
					preAdjectives = (from w in PreAdjectives
									 where w.Item2 == WordQuality.Bad
									 select w).ToList<Tuple<string, WordQuality>>();
					ofX = (from w in OfX
						   where w.Item2 == WordQuality.Bad
						   select w).ToList<Tuple<string, WordQuality>>();
					break;
				case RarityLevel.Type.Normal:
					preAdjectives = (from w in PreAdjectives
									 where w.Item2 != WordQuality.Bad && w.Item2 != WordQuality.Good
									 select w).ToList<Tuple<string, WordQuality>>();
					ofX = (from w in OfX
						   where w.Item2 != WordQuality.Bad && w.Item2 != WordQuality.Good
						   select w).ToList<Tuple<string, WordQuality>>();
					break;
				default:
					preAdjectives = (from w in PreAdjectives
									 where w.Item2 != WordQuality.Bad
									 select w).ToList<Tuple<string, WordQuality>>();
					ofX = (from w in OfX
									 where w.Item2 != WordQuality.Bad
									 select w).ToList<Tuple<string, WordQuality>>();
					break;
			}

			name = i.Variety.Names[r.Next(i.Variety.Names.Count - 1)];
			name = preAdjectives[r.Next(preAdjectives.Count - 1)].Item1 + " " + name;
			if (r.Next(0, oddsOfOfX) == 0) name += " of " + ofX[r.Next(ofX.Count - 1)].Item1;
			i.Name = name;
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

		public string Name = "!!OSHIT NO NAME GENERATED!!";
		public void WriteStatBlock() {
			int width = 34;
			int padding = 3;
			string leftPadding = new String(' ', 40 - (width / 2));
			string bar = "│";
			//string line = "+" + new String('─', width - 2) + "+";
			string line = new String('─', width - 2);
			string empty = bar + new String(' ', width - 2) + bar;

			Console.WriteLine(leftPadding + "┌" + line + "┐");
			List<string> name = Name.ToUpper().Wrap(width - 4);
			foreach (string s in name) {
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(leftPadding + bar + " ");
				Console.ForegroundColor = RarityToConsoleColor(Rarity);
				Console.Write(s.PadCenter(width - 4, ' '));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(" " + bar + "\n");
			}
			//Console.WriteLine(Name.ToUpper().PadCenter(width, ' '));
			Console.ResetColor();
			Console.WriteLine(leftPadding + "├" + line + "┤");

			string type = (Rarity.Name == RarityLevel.Type.Normal ? "" : Rarity.ToString() + " ") + Variety.Name;

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(leftPadding + bar + " ");

			Console.ForegroundColor = RarityToConsoleColor(Rarity);
			Console.Write(type);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(Variety.Slot.ToString().DeCamelCase().PadLeft(width - type.Length - 4) + " " + bar + "\n");
			Console.WriteLine(leftPadding + empty);
			Console.ResetColor();

			foreach (KeyValuePair<Attribute.Type, double> kvp in Attributes) {
				string key = new String(' ', padding) + kvp.Key.ToString().DeCamelCase();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(leftPadding + bar + key);
				Console.ForegroundColor = ConsoleColor.White;
				string num = ((Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				Console.Write(num.PadLeft(width - key.Length - padding - 2) + new String(' ', padding));
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(bar + "\n");
				Console.ResetColor();
			}
			Console.WriteLine(leftPadding + empty);
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(leftPadding + bar);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write(("Required Level: " + Level).PadCenter(width - 2, ' '));
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(bar + "\n");
			Console.ResetColor();
			Console.WriteLine(leftPadding + "└" + line + "┘");
		}

	}
}