using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootShop {
	public class Item {
		public static List<string> PreAdjectives = new List<string> {
			"Fantastic", "Amazing", "Fuckin' Metal", "Preposterous", "Ridiculously Overpowered", "Slithering", "Rock-Hard", "Aching", "Throbbing", "Tedious", "Lengthy", "Fond", "Horrible", "Ghostly", "Sweaty", "Charming",
			"Rifting", "Lofty", "Perfectly Ordinary", "Jubilant", "Lightly Seasoned", "Seasoned", "Withered", "Humiliating", "Meaningful", "Far-Fetched", "Controversial", "Stolen", "Imbued", "Bashful", "Delicate",
			"Righteous", "Encouraging", "Baffling", "Brilliant", "Rotten", "Rotting", "Roaring", "Seamless", "Forboding", "Tormented", "Scattered", "Demeaning", "Well-Crafted", "Epic", "Unfinished", "Half-Assed",
			"Rustic", "Rusted", "Stained", "Becoming", "Plentiful", "Breached", "Shady", "Ransacked", "Virgin", "Well-Trimmed", "Well-Taught", "Charitable", "Resplendent", "Regular-Ass", "Gaping", "Stout", "Unfortunate",
			"Famished", "Impoverished", "Unheard-Of", "Ineffable", "Affable", "Unusable", "Super Shiny", "Gas-Operated", "Tony's", "Moist", "Bleached", "Well-Bathed", "Static", "Well-Tuned", "Unstrung", "Shoddy", "Cross-Hatched",
			"Strange", "Unremarkable", "Scarcely Lethal", "Mildly Menacing", "Somewhat Threatening", "Uncharitable", "Notably Dangerous", "Sufficiently Lethal", "Truly Feared", "Wicked Nasty", "Totally Ordinary", "Australian", //lol tf2
			"Glamorous", "Fabulous", "Stingy", "Unplanned", "Vaguely Erotic", "Seductive", "Burnt", "Chiseled", "Gloomy", "Shiny", "Musty", "Moldy", "Drab", "Worn", "Deflated", "Shriveled", "Spotted", "Termite-Covered", "Polished",
			"Enrapturing", "Warped", "Kinda Shitty", "Mediocre", "Scratched", "Dried", "Thrown Out", "Discarded", "Broken", "Irrepairable", "Unbelievable", "Indispensable", "Overprepared", "Succulent", "Subpar", "Asymmetrical",
			"Negligent", "Gentleman's", "Lady-Like", "Manly", "Strict", "Greedy", "Notable", "Distorted", "Sight-Seeing", "Sexy", "Painful", "Legless", "Brass", "Fallen", "Tacky", "Bundled", "Business", "Requested",
			"Summoned", "Ripped", "Twelfth", "Poorly-Wrapped", "My Wife's", "Butlers' Choice", "Scammed", "Cascading", "Uplifting", "Drafty", "Sinking", "Golden", "Plated", "Gilded", "Loot-Worthy", "Medieval", "Tribal", "Master",
			"Brutal", "Brütal", "Everburning", "Giggling", "Prancing", "Balanced", "Silken"
		};
		public static List<string> OfX = new List<string> {
			"The Boar", "Legend", "Lore", "Christmas Past", "Biblical Proportions", "China", "The Beast", "Reckoning", "The End Times", "Poverty", "The Jews", "Unbelievable Power", "Ridicule", "Greed", "Extra Shininess", "Shit",
			"Embarassment", "The Wildebeast", "Worth", "Note", "Hope", "Survivability", "Reminiscence", "The Tourist", "The Lion", "The Loins", "The Hawk", "The Prisoner", "Portability", "The Clockmaker", "The Fool", "Sex",
			"Sacrifice", "Smithing", "Disaster", "Completion", "Wrath", "Ascension", "Treasure", "Snakes", "Light", "Darkness", "The Owl", "Horror", "Vegetation", "Lies", "Greatness", "Due Time", "Patience", "Uncertainty",
			"Quality", "\"Quality\"", "Celebration", "Burning", "Giggling", "Dancing", "The Journey", "Request", "Balance", "Cleansing", "Stability", "Teaching", "Speculation", "Wizardry", "The Church"
		};
		public enum Type {
			Greataxe,
			Longsword,
			//Staff,
			//Bow,

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
					"Sword", "Rapier", "Blade", "Pointy Stick", "Cutlass", "Zweihander", "Sabre", "Falchion"
				}));
				List.Add(new Item.Kind(Type.Greataxe, Slot.TwoHands, meleeAttr, new List<string> {
					"Axe", "Hatchet", "Pick", "Broad Axe", "Battleaxe", "Great Axe", "Giant Axe", "Cleaver"
				}));

				// Armor
				List.Add(new Item.Kind(Type.Helmet, Slot.Head, armorAttr, new List<string> {
					"Helmet", "Hat", "Cap", "Crown", "Casque", "Basinet", "Sallet"
				}));
				List.Add(new Item.Kind(Type.Armor, Slot.Chest, armorAttr, new List<string> {
					"Studded Armor", "Leather Armor", "Quilted Armor", "Ring Mail", "Scale Mail", "Chain Mail", "Breastplate", "Plate Mail", "Shirt", "Bodyshaft", "Sweater", "Rags", "Undershirt", "Robe", "Cloak", "Goatskin"
				}));
				List.Add(new Item.Kind(Type.Pants, Slot.Legs, armorAttr, new List<string> {
					"Pants", "Leggings", "Kneepads", "Chaps", "Leg-Wraps"
				}));
				List.Add(new Item.Kind(Type.Gloves, Slot.Hands, armorAttr, new List<string> {
					"Gloves", "Gauntlets", "Bracers", "Mitts", "Vambraces", "Graspers"
				}));
				List.Add(new Item.Kind(Type.Belt, Slot.Waist, armorAttr, new List<string> {
					"Sash", "Belt", "Coil", "Girdle", "Belly-Wrap", "String"
				}));
				List.Add(new Item.Kind(Type.Boots, Slot.Feet, armorAttr, new List<string> {
					"Shoes", "Boots", "Greaves", "Sandals", "Galoshes"
				}));
				List.Add(new Item.Kind(Type.Pauldrons, Slot.Shoulders, armorAttr, new List<string> {
					"Shoulders", "Pauldrons", "Epaulets", "Mantle", "Shoulderpads"
				})); ;

				// Pure Attributehavers
				List.Add(new Item.Kind(Type.Ring, Slot.Finger, pureAttr, new List<string> {
					"Ring", "Band", "Finger-Wrap"
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
				List.Add(new RarityLevel(Type.Garbage,		new Range(0, 0),	new Range(0, 0.75),		false));
				List.Add(new RarityLevel(Type.Normal,		new Range(0, 0),	new Range(0.75, 1.25),	false));
				List.Add(new RarityLevel(Type.Magic,		new Range(2, 4),	new Range(1.25, 1.75),	true));
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
				AttackSpeed
			}

			public Type Name;
			public bool BaseStat = false;

			public static List<Attribute> List = new List<Attribute>();

			public static void Initialize() {
				List.Add(new Attribute(Type.Damage, true));
				List.Add(new Attribute(Type.AttacksPerSecond, true));
				List.Add(new Attribute(Type.Armor, true));
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

			public Attribute(Type name, bool baseStat) {
				Name = name;
				BaseStat = baseStat;
			}

		}

		public Dictionary<Attribute.Type, int> Attributes = new Dictionary<Attribute.Type, int>();
		public RarityLevel Rarity;
		public int Level;
		public Kind Variety;

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
				int attrVal = 1;
				int baseVal = 10;

				attrVal += Math.Max((baseVal * Math.Max(i.Level - 1, 1)) + r.Next(baseVal * 2), 1);
				attrVal = Math.Max((int)((double)attrVal * i.Rarity.AttributeModifier.RandomDouble(r)), 1);

				i.Attributes.Add(a.Name, attrVal);
			}

			// GENERATE THE NAME!!
			int oddsOfOfX = 1;
			string name = "";
			name = i.Variety.Names[r.Next(i.Variety.Names.Count - 1)];
			name = Item.PreAdjectives[r.Next(Item.PreAdjectives.Count - 1)] + " " + name;
			if (r.Next(0, oddsOfOfX) == 0) name += " of " + Item.OfX[r.Next(Item.OfX.Count - 1)];
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
			int width = 32;
			int padding = 6;
			string line = new String('-', width);
			Console.WriteLine(line);
			Console.ForegroundColor = RarityToConsoleColor(Rarity);
			List<string> name = Name.ToUpper().Wrap(width);
			foreach (string s in name) Console.WriteLine(s.PadCenter(width, ' '));
			//Console.WriteLine(Name.ToUpper().PadCenter(width, ' '));
			Console.ResetColor();
			Console.WriteLine(line);

			string type = " " + (Rarity.Name == RarityLevel.Type.Normal ? "" : Rarity.ToString() + " ") + Variety.Name;

			Console.ForegroundColor = RarityToConsoleColor(Rarity);
			Console.Write(type);

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write(Variety.Slot.ToString().DeCamelCase().PadLeft(width - type.Length - 1) + "\n");
			Console.WriteLine();
			Console.ResetColor();

			foreach (KeyValuePair<Attribute.Type, int> kvp in Attributes) {
				string key = new String(' ', padding) + kvp.Key.ToString().DeCamelCase();
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write(key);
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(kvp.Value.ToString().PadLeft(width - key.Length - padding) + "\n");
				Console.ResetColor();
			}
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(("Required Level: " + Level).PadCenter(width, ' '));
			Console.ResetColor();
			Console.WriteLine(line);
		}

	}
}