using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public abstract class Creature : IDamageable {
		private int hp;
		public int HP { get { return hp; } set { hp = value; } }
		private int maxHP;
		public int MaxHP { get { return maxHP; } set { maxHP = value; } }
		public int Level;
		public static Creature Create(CreatureType type) {
			Creature creature = null;
			switch (type) {
				case CreatureType.Zombie:
					return new Enemy.Zombie();
			}
			return creature;
		}
	}

	public enum CreatureType {
		Zombie
	}
	public class Hero : Creature {
		public string Name;
		public int XP;
		public List<Item> Inventory = new List<Item>();
	}

	public class Enemy : Creature {
		public class Zombie : Enemy {
			public Zombie() {
				MaxHP = 1;
				HP = 1;
			}
		}
	}

	public interface IDamageable {
		int HP { get; set; }
		int MaxHP { get; set; }
	}
}
