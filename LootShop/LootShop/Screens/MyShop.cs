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
	public class MyShop : GenericMenu {
		public MyShop()
			: base("My Shop", true, true) {
				DimBackground = false;
				Entry entryManageInventory = new Entry("Manage Inventory");
				Entry entryOpenShop = new Entry("Open Shop");
				entryOpenShop.Selected += OpenShop;

				MenuEntries.Add(entryManageInventory);
				MenuEntries.Add(entryOpenShop);
		}

		void OpenShop(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.Campaign.Time.TimeOfDay++;
		}

		protected override void OnCancel(PlayerIndex playerIndex) {
			ScreenManager.ReplaceScreen(new TownMap(), ControllingPlayer);
		}
	}
}
