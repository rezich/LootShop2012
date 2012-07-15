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
	public class MyShopMenu : GenericMenu {
		public MyShopMenu()
			: base("My Shop", true, true) {
			DimBackground = false;
			Entry entryManageInventory = new Entry("Manage Inventory", new TextBlock("Manage your shop's inventory. #ACTION_0#"));
			MenuEntries.Add(entryManageInventory);

			Entry entryOpenShop = new Entry("Open Shop", new TextBlock("Open your shop for business! Can't be done at night. #ACTION_1#"));
			entryOpenShop.Selected += OpenShop;
			if (GameSession.Current.Campaign.Time.TimeOfDay == (int)Time.TimeOfDayName.Night) entryOpenShop.Visible = false;
			MenuEntries.Add(entryOpenShop);
		}

		void OpenShop(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ReplaceScreen(new MyShopOpen(), ControllingPlayer);
		}

		protected override void OnCancel(PlayerIndex playerIndex) {
			//ScreenManager.ReplaceAllScreens(new TownMap(), ControllingPlayer);
			ScreenManager.RemoveScreen();
			ScreenManager.RemoveScreen();
			ScreenManager.AddScreen(new TownMap(), ControllingPlayer);
		}
	}
}
