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
	class TitleScreen : GameScreen {
		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			string title = "Loot Shop";
			string pressStart = "PRESS START";
			string copyright = "Copyright © 2012 108 Studios, all rights reserved.";
			ScreenManager.SpriteBatch.DrawString(Game.Current.UIFontMedium, title, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 13), Color.White, 0.0f, Game.Current.UIFontMedium.MeasureString(title) / 2, 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.DrawString(Game.Current.UIFontSmall, pressStart, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 11), Color.Lerp(Color.Yellow, Color.Black, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5)), 0.0f, Game.Current.UIFontSmall.MeasureString(pressStart) / 2, 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.DrawString(Game.Current.UIFontSmall, copyright, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height), Color.Gray, 0.0f, new Vector2(Game.Current.UIFontSmall.MeasureString(copyright).X / 2, Game.Current.UIFontSmall.MeasureString(copyright).Y), 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsPressStart(null, out playerIndex)) ScreenManager.AddScreen(new MainMenu(), playerIndex);
		}
	}
}
