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
	class MyShopOpen : GenericMenu {
		public MyShopOpen()
			: base("My Shop - OPEN", false, true) {
				Entry entryCloseShop = new Entry("Close Shop");
				entryCloseShop.Content = new TextBlock("Close the shop.");
				entryCloseShop.IsCancel = true;
				entryCloseShop.Selected += CloseShop;
				MenuEntries.Add(entryCloseShop);
		}

		void CloseShop(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.Campaign.Time.TimeOfDay++;
			ScreenManager.ReplaceScreen(new MyShopMenu(), ControllingPlayer);
		}
	}
}
