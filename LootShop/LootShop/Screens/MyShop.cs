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
	public class MyShop : GameScreen {

		public override void Initialize() {
			MediaPlayer.Play(GameSession.Current.ShopTheme);
			ScreenManager.AddScreen(new MyShopMenu(), ControllingPlayer);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (ScreenManager.GetScreens().Last() == this) ScreenManager.AddScreen(new TownMap(), ControllingPlayer);
		}
	}
}
