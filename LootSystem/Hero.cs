using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public class Hero {
		public string Name;
		public int Level;
		public int XP;
		public List<Item> Items = new List<Item>();
	}
}
