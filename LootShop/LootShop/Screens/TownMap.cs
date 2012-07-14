using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	public class TownMap : GenericMenu {
		public TownMap()
			: base("Town Map", true, true) {
				DimBackground = false;
				Entry entryShop = new Entry("My Shop");
				entryShop.Selected += ToMyShop;
				Entry entryTownSquare = new Entry("Town Square");
				Entry entryTavern = new Entry("Tavern");
				Entry entryDungeon = new Entry("Dungeon");

				MenuEntries.Add(entryShop);
				MenuEntries.Add(entryTownSquare);
				MenuEntries.Add(entryTavern);
				MenuEntries.Add(entryDungeon);
		}

		void ToMyShop(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ReplaceScreen(new MyShop(), ControllingPlayer); 
		}

		protected override void OnCancel(PlayerIndex playerIndex) {
			ScreenManager.BackToTitle(ControllingPlayer);
		}
	}
}
