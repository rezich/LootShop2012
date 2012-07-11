using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	class PressStartScreen : GameScreen {
		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			string title = "Loot Shop";
			string pressStart = "PRESS START";
			string copyright = "Copyright © 2012 108 Studios, all rights reserved.";
			ScreenManager.SpriteBatch.DrawString(LootShop.CurrentGame.UIFontMedium, title, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 12), Color.White, 0.0f, LootShop.CurrentGame.UIFontMedium.MeasureString(title) / 2, 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.DrawString(LootShop.CurrentGame.UIFontSmall, pressStart, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 10), Color.Lerp(Color.White, Color.Black, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5)), 0.0f, LootShop.CurrentGame.UIFontSmall.MeasureString(pressStart) / 2, 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.DrawString(LootShop.CurrentGame.UIFontSmall, copyright, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height), Color.Gray, 0.0f, new Vector2(LootShop.CurrentGame.UIFontSmall.MeasureString(copyright).X / 2, LootShop.CurrentGame.UIFontSmall.MeasureString(copyright).Y), 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsPressStart(null, out playerIndex)) ScreenManager.ReplaceScreen(new TestScreen(), playerIndex);
		}
	}
}
