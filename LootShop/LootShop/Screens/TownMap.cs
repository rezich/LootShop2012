﻿using System;
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
				Entry entryShop = new Entry("My Shop", new TextBlock("Your shop! Manage your shop's inventory, put items out on display, and, most importantly, open your shop for business."));
				entryShop.Selected += ToMyShop;
				Entry entryTownSquare = new Entry("Town Square", new TextBlock("Hear the latest rumors and gossip about the town."));
				Entry entryTavern = new Entry("Tavern", new TextBlock("Find and hire new adventurers to expand your loot-obtaining capabilities."));
				Entry entryDungeon = new Entry("Dungeon", new TextBlock("Delve into the dungeons in search of loot and more loot!"));

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
