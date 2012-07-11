using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace LootSystem {
	public class Item {
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
					"Dagger", "Knife", "Letter Opener", "Stabber", "Dirk", "Stiletto", "Point", "Spike", "Piercer", "Fleshripper", "Skincutter", "Fang"
				}));
				List.Add(new Item.Kind(Type.Greataxe, Slot.TwoHands, meleeAttr, new List<string> {
					"Axe", "Hatchet", "Pick", "Broad Axe", "Battleaxe", "Great Axe", "Giant Axe", "Cleaver", "Pickaxe", "Reaver"
				}));
				List.Add(new Item.Kind(Type.Staff, Slot.OneHand, meleeAttr, new List<string> {
					"Staff", "Rod", "Wand", "Implement", "Quarterstaff", "Walking Stick", "Channeler", "Spellbinder", "Branch"
				}));
				List.Add(new Item.Kind(Type.Bow, Slot.TwoHands, meleeAttr, new List<string> {
					"Bow", "Shortbow", "Longbow", "Composite Bow", "War Bow", "Siege Bow", "Great Bow", "Cross Bow", "Sniper Bow"
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
					"Pants", "Leggings", "Kneepads", "Chaps", "Leg-Wraps", "Lederhosen", "Kilt", "Faulds", "Leg Plates"
				}));
				List.Add(new Item.Kind(Type.Gloves, Slot.Hands, armorAttr, new List<string> {
					"Gloves", "Gauntlets", "Bracers", "Mitts", "Vambraces", "Graspers", "Stranglers"
				}));
				List.Add(new Item.Kind(Type.Belt, Slot.Waist, armorAttr, new List<string> {
					"Sash", "Belt", "Coil", "Girdle", "Belly-Wrap", "String", "Strap", "Strand", "Scabbard", "Chain"
				}));
				List.Add(new Item.Kind(Type.Boots, Slot.Feet, armorAttr, new List<string> {
					"Shoes", "Boots", "Greaves", "Sandals", "Galoshes", "Kicks", "Treads"
				}));
				List.Add(new Item.Kind(Type.Pauldrons, Slot.Shoulders, armorAttr, new List<string> {
					"Shoulders", "Pauldrons", "Mantle", "Shoulderpads", "Spaulders", "Single Shoulderpad"
				})); ;

				// Pure Attributehavers
				List.Add(new Item.Kind(Type.Ring, Slot.Finger, pureAttr, new List<string> {
					"Ring", "Band", "Finger Protector"
				}));
				List.Add(new Item.Kind(Type.Amulet, Slot.Neck, pureAttr, new List<string> {
					"Amulet", "Necklace", "Charm"
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
				List.Add(new RarityLevel(Type.Normal,		new Range(0, 0),	new Range(0, 0.5),		false));
				List.Add(new RarityLevel(Type.Magic,		new Range(4, 6),	new Range(0.25, 0.75),	true));
				List.Add(new RarityLevel(Type.Rare,			new Range(6, 8),	new Range(1.5, 2.0),	true));
				List.Add(new RarityLevel(Type.Legendary,	new Range(8, 10),	new Range(1.75, 2.5),	true));
				List.Add(new RarityLevel(Type.Unique,		new Range(10, 14),	new Range(2.5, 3),		true));
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
				LootFind,
				GoldFind,
				AttackSpeed,
				ResistBees,
				ResistCold,
				ResistFire,
				ResistLightning,
				ResistPoison,
				LifeOnHit,
				ReplenishLife,
				Indestructible,
				Ethereal,
				GibOnKill,
				SelfRepairing,
				SkeletonizeOnKill,
				DisintegrateOnKill
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
			public bool Randomable;

			public bool Rounded;
			public bool Percentage;
			public bool Addition;

			public string NonstandardListing;

			public static List<Attribute> List = new List<Attribute>();

			public static void Initialize() {
				//																						Base	Mod+	Mod*	LBase	LMod+	LMod*	RBase	RMod+	RMod*
				List.Add(new Attribute(Type.Damage,				true,	false,	true,	false,	false,		0,		0,		0,		15,		14,		0,		15,		15,		0, null));
				List.Add(new Attribute(Type.AttacksPerSecond,	true,	false,	false,	false,	false,		1.5,	1,		0,		0,		0,		0,		0,		0,		0, null));
				List.Add(new Attribute(Type.Armor,				true,	false,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0, null));
				List.Add(new Attribute(Type.Strength,			false,	true,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0, null));
				List.Add(new Attribute(Type.Dexterity,			false,	true,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0, null));
				List.Add(new Attribute(Type.Intelligence,		false,	true,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0, null));
				List.Add(new Attribute(Type.Vitality,			false,	true,	true,	false,	false,		0,		0,		0,		8,		7,		0,		8,		7,		0, null));
				List.Add(new Attribute(Type.LootFind,			false,	true,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0, null));
				List.Add(new Attribute(Type.GoldFind,			false,	true,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0, null));
				List.Add(new Attribute(Type.AttackSpeed,		false,	true,	true,	true,	true,		0,		0,		0,		1,		1,		0,		1,		1,		0, null));
				List.Add(new Attribute(Type.ResistBees,			false,	true,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0, null));
				List.Add(new Attribute(Type.ResistCold,			false,	true,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0, null));
				List.Add(new Attribute(Type.ResistFire,			false,	true,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0, null));
				List.Add(new Attribute(Type.ResistLightning,	false,	true,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0, null));
				List.Add(new Attribute(Type.ResistPoison,		false,	true,	true,	false,	false,		20,		10,		0,		0,		0,		0,		0,		10,		0, null));
				List.Add(new Attribute(Type.LifeOnHit,			false,	true,	true,	false,	false,		0,		0,		0,		4,		4,		0,		4,		4,		0, null));
				List.Add(new Attribute(Type.ReplenishLife,		false,	true,	true,	false,	false,		0,		0,		0,		4,		4,		0,		4,		4,		0, null));
				List.Add(new Attribute(Type.Indestructible,		false,	true,	false,	false,	false,		0,		0,		0,		0,		0,		0,		0,		0,		0, "Indestructible"));
				List.Add(new Attribute(Type.Ethereal,			false,	true,	false,	false,	false,		0,		0,		0,		0,		0,		0,		0,		0,		0, "Ethereal"));
				List.Add(new Attribute(Type.SelfRepairing,		false,	true,	false,	false,	false,		0,		0,		0,		0,		0,		0,		0,		0,		0, "Self-Repairing"));
				List.Add(new Attribute(Type.GibOnKill,			false,	true,	true,	true,	true,		50,		50,		0,		0,		0,		0,		0,		0,		0, "@ chance to gib on kill"));
				List.Add(new Attribute(Type.SkeletonizeOnKill,	false,	true,	true,	true,	true,		50,		50,		0,		0,		0,		0,		0,		0,		0, "@ chance to skeletonize on kill"));
				List.Add(new Attribute(Type.DisintegrateOnKill,	false,	true,	true,	true,	true,		50,		50,		0,		0,		0,		0,		0,		0,		0, "@ chance to disintegrate on kill"));
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

			public Attribute(Type name, bool baseStat, bool randomable, bool rounded, bool addition, bool percentage, double baseValue, double modAdd, double modMultiply, double levelBaseValue, double levelModAdd, double levelModMultiply, double rarityBaseValue, double rarityModAdd, double rarityModMultiply, string nonstandardListing) {
				Name = name;
				BaseStat = baseStat;
				Randomable = randomable;
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

				NonstandardListing = nonstandardListing;
			}

		}
		public class Modifier : INotifyPropertyChanged, IComparable {
			[XmlType("ItemModifierType")]
			public enum Type {
				Adjective,
				OfX
			}

			protected string name;
			public string Name {
				get { return name; }
				set { name = value; OnPropertyChanged("Name"); }
			}
			public string NameWithQuality {
				get { return Name + (tags.Contains("good") ? "+" : "") + (tags.Contains("bad") ? "-" : ""); }
			}

			protected Item.Modifier.Type kind;
			public Item.Modifier.Type Kind {
				get { return kind; }
				set { kind = value; OnPropertyChanged("Kind"); }
			}

			protected List<string> tags;
			public List<string> Tags {
				get { return tags; }
				set { tags = value; OnPropertyChanged("Tags"); }
			}

			public List<Submodifier> Submodifiers;

			//public static Item.Modifier.ListType List = new Item.Modifier.ListType();

			public static List<Modifier> List = new List<Modifier>();

			public int CompareTo(object obj) {
				Modifier mod = obj as Modifier;
				if (mod == null) throw new ArgumentException("Object is not Modifer");
				return Name.CompareTo(mod.Name);
			}

			public Modifier(string name, Item.Modifier.Type kind, List<string> tags) {
				Name = name;
				Kind = kind;
				Tags = tags;
				List.Add(this);
			}

			public Modifier() {
				List.Add(this);
			}

			public static List<Modifier> FindByTag(string tag) {
				List<Modifier> list = new List<Modifier>();
				list = (from m in Item.Modifier.List
						where m.Tags.Contains(tag)
						select m).ToList<Modifier>();
				return list;
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

			public static void Load() {
				Item.Modifier.List.Clear();
				List<Modifier> loadedData = LoadFromFile();
				foreach (Item.Modifier m in loadedData) {
					Item.Modifier.List.Add(m);
					// NO IDEA WHY THE FUCK THE NEXT LINE OF CODE WORKS, BUT IT DOES
					// (update: slept on it, figured it out, should probably rewrite other stuff to make it not needed but w/e)
					Item.Modifier.List.RemoveAt(Item.Modifier.List.Count - 1);
				}
			}
			public static List<Modifier> LoadFromFile() {
				//StreamWriter writer;
				Stream stream;
				Assembly assembly = Assembly.GetExecutingAssembly();
				List<Modifier> item = new List<Modifier>();

				try {
					stream = assembly.GetManifestResourceStream("LootSystem.Modifiers.xml");
				}
				catch {
					throw new Exception("Aw shit son!");
				}
				XmlSerializer serializer = new XmlSerializer(typeof(List<Modifier>));
				XmlReader reader = XmlReader.Create(stream);
				item = (List<Modifier>)serializer.Deserialize(reader);

				return item;
			}

			/*public void Save(string fileName) {
				XmlSerializer writer = new XmlSerializer(typeof(List<Modifier>));
				System.IO.StreamWriter file = new StreamWriter(fileName);
				writer.Serialize(file, this);
				file.Close();
			}*/

			/*public class ListType : ObservableCollection<Item.Modifier> {
				public static void Load(string fileName) {
					Item.Modifier.List.Clear();
					Item.Modifier.ListType loadedData = Item.Modifier.ListType.LoadFromFile(fileName);
					foreach (Item.Modifier m in loadedData) {
						Item.Modifier.List.Add(m);
						// NO IDEA WHY THE FUCK THE NEXT LINE OF CODE WORKS, BUT IT DOES
						// (update: slept on it, figured it out, should probably rewrite other stuff to make it not needed but w/e)
						Item.Modifier.List.RemoveAt(Item.Modifier.List.Count - 1);
					}
				}
				public static ListType LoadFromFile(string fileName) {
					XmlSerializer reader = new XmlSerializer(typeof(Item.Modifier.ListType));
					System.IO.StreamReader file = new System.IO.StreamReader(fileName);
					ListType item = new ListType();
					item = (ListType)reader.Deserialize(file);
					file.Close();
					return item;
				}

				public void Save(string fileName) {
					XmlSerializer writer = new XmlSerializer(typeof(Item.Modifier.ListType));
					System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
					writer.Serialize(file, this);
					file.Close();
				}
			}*/
		}
		public class Submodifier {
			public List<Type> Types;
			public List<Modification> Modifications;

			public Submodifier(List<Type> types, List<Modification> modifications) {
				Types = types;
				Modifications = modifications;
			}
			public Submodifier() {
				Types = new List<Type>();
				Modifications = new List<Modification>();
			}
		}
		public class Modification {
			//[XmlAttribute("ModifiedAttribute")]
			Attribute.Type Attribute;
			bool Guaranteed;

			public Modification(Attribute.Type attribute, bool guaranteed) {
				Attribute = attribute;
				Guaranteed = guaranteed;
			}
			public Modification() {
				Attribute = Item.Attribute.Type.LootFind;
				Guaranteed = false;
			}
		}

		public Dictionary<Attribute.Type, double> Attributes = new Dictionary<Attribute.Type, double>();
		public RarityLevel Rarity;
		public int Level;
		public Kind Variety;

		public Dictionary<Attribute.Type, double> StandardAttributes {
			get {
				var items = Attributes.Where(x => Attribute.Lookup(x.Key).NonstandardListing == null).ToDictionary(x => x.Key, x => x.Value);
				return (Dictionary<Attribute.Type, double>)items;
			}
		}
		public Dictionary<Attribute.Type, double> NonstandardAttributes {
			get {
				var items = Attributes.Where(x => Attribute.Lookup(x.Key).NonstandardListing != null).ToDictionary(x => x.Key, x => x.Value);
				return (Dictionary<Attribute.Type, double>)items;
			}
		}

		public static void Initialize() {
			Item.Modifier.Load();
			Attribute.Initialize();
			RarityLevel.Initialize();
			Kind.Initialize();
		}
		public static Item Generate(int level, Random r) {
			// Choose a random rarity
			Item.RarityLevel.Type[] rarityValues = (Item.RarityLevel.Type[])EnumHelper.GetValues<Item.RarityLevel.Type>();
			Item.RarityLevel.Type selectedRarity = RarityLevel.Type.Normal;

			// TODO: Replace this shitty thing with a real thing
			int rand = r.Next(100);
			if (rand <= 25) selectedRarity = RarityLevel.Type.Garbage;
			if (rand > 25 && rand <= 75) selectedRarity = RarityLevel.Type.Normal;
			if (rand > 75 && rand <= 95) selectedRarity = RarityLevel.Type.Magic;
			if (rand > 95 && rand <= 98) selectedRarity = RarityLevel.Type.Rare;
			if (rand > 98) selectedRarity = RarityLevel.Type.Legendary;


			// Pick a random rarity, but if the type of item is one with no base stats (ring, amulet),
			// keep rolling until you get a "Magic" one (blue or higher)
			/*do {
				selectedRarity = rarityValues[r.Next(0, rarityValues.Length)];
			} while (i.Variety.BaseAttributes.Count == 0 && !RarityLevel.Lookup(selectedRarity).Magic);*/
			return Generate(level, selectedRarity, r);
		}
		public static Item Generate(int level, RarityLevel.Type rarity, Random r) {
			Item i = new Item();
			i.Level = level;

			// Choose a random item kind
			//Item.Type[] kindValues = (Item.Type[])Enum.GetValues(typeof(Item.Type));
			
			Item.Type[] kindValues =  (Item.Type[])EnumHelper.GetValues<Item.Type>();

			Item.Type selectedKind = Type.Armor;
			do {
				selectedKind = kindValues[r.Next(0, kindValues.Length)];
			}
			while (Item.Kind.Lookup(selectedKind).BaseAttributes.Count == 0 && RarityLevel.Lookup(rarity).Magic);
			i.Variety = Kind.Lookup(selectedKind);

			i.Rarity = RarityLevel.Lookup(rarity);

			List<Item.Attribute> attrs = new List<Item.Attribute>();

			foreach (Attribute attr in i.Variety.BaseAttributes) {
				attrs.Add(attr);
			}

			int attrCount = i.Rarity.AttributeCount.RandomInt(r);
			List<Item.Attribute.Type> takenAttrs = new List<Attribute.Type>();
			while (attrs.Count < attrCount) {
				Item.Attribute.Type[] values = (Item.Attribute.Type[]) EnumHelper.GetValues<Item.Attribute.Type>();
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
			List<Modifier> preAdjectives;
			List<Modifier> ofX;

			// TODO: Rewrite to use RarityLevel's "good" and "bad" word odds
			switch (i.Rarity.Name) {
				case RarityLevel.Type.Garbage:
					preAdjectives = (from w in Item.Modifier.List
									 where w.Tags.Contains("bad") && w.Kind == Modifier.Type.Adjective
									 select w).ToList<Modifier>();
					ofX = (from w in Item.Modifier.List
						   where w.Tags.Contains("bad") && w.Kind == Modifier.Type.OfX
						   select w).ToList<Modifier>();
					break;
				case RarityLevel.Type.Normal:
					preAdjectives = (from w in Item.Modifier.List
									 where !w.Tags.Contains("good") && !w.Tags.Contains("bad") && w.Kind == Modifier.Type.Adjective
									 select w).ToList<Modifier>();
					ofX = (from w in Item.Modifier.List
						   where !w.Tags.Contains("good") && !w.Tags.Contains("bad") && w.Kind == Modifier.Type.OfX
						   select w).ToList<Modifier>();
					break;
				default:
					preAdjectives = (from w in Item.Modifier.List
									 where !w.Tags.Contains("bad") && w.Kind == Modifier.Type.Adjective
									 select w).ToList<Modifier>();
					ofX = (from w in Item.Modifier.List
						   where !w.Tags.Contains("bad") && w.Kind == Modifier.Type.OfX
						   select w).ToList<Modifier>();
					break;
			}

			name = i.Variety.Names[r.Next(i.Variety.Names.Count - 1)];
			name = preAdjectives[r.Next(preAdjectives.Count - 1)].Name + " " + name;
			if (r.Next(0, oddsOfOfX) == 0) name += " of " + ofX[r.Next(ofX.Count - 1)].Name;
			i.Name = name;
			return i;
		}

		public string Name = "!!OSHIT NO NAME GENERATED!!";

	}
}