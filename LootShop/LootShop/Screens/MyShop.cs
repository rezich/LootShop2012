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
	public class MyShop : GameScreen {
		public override void Initialize() {
			ScreenManager.AddScreen(new MyShopMenu(), ControllingPlayer);
		}
	}
}
