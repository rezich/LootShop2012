using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

namespace LootShop {
	public class TownMap : GenericMenu {
		public TownMap()
			: base("Town Map", true, true) {
				DimBackground = false;
				Description = new TextBlock("Choose where you want to go around town.");
				Entry entryShop = new Entry("My Shop");
				entryShop.Content = new TextBlock("Your shop! Manage your shop's inventory, put items out on display, and, most importantly, open your shop for business.");
				entryShop.Selected += ToMyShop;
				MenuEntries.Add(entryShop);

				Entry entryTownSquare = new Entry("Town Square");
				entryTownSquare.Content = new TextBlock("Hear the latest rumors and gossip about the town.");
				entryTownSquare.Selected += ToTownSquare;
				MenuEntries.Add(entryTownSquare);

				Entry entryTavern = new Entry("Tavern");
				entryTavern.Content = new TextBlock("Find and hire new adventurers to expand your loot-obtaining capabilities.");
				MenuEntries.Add(entryTavern);

				Entry entryDungeon = new Entry("Dungeon");
				entryDungeon.Content = new TextBlock("Delve into the dungeons in search of loot and more loot!");
				MenuEntries.Add(entryDungeon);
		}

		public override void Initialize() {
			MediaPlayer.Play(GameSession.Current.TownTheme);
		}

		void ToMyShop(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ReplaceScreen(new MyShop(), ControllingPlayer); 
		}

		void ToTownSquare(object sender, PlayerIndexEventArgs e) {
			/*ScreenManager.AddScreen(new CutsceneScreen(new List<CutsceneAction>() {
				new DialogueAction("There is nobody in the town square at the moment.")
			}), ControllingPlayer);*/
			ScreenManager.AddScreen(new CutsceneScreen("Opening"), ControllingPlayer);
		}

		protected override void OnCancel(PlayerIndex playerIndex) {
			ScreenManager.BackToTitle(ControllingPlayer);
		}
	}
}
