using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {	
	class SplashScreen : GameScreen {
		string text;
		int countdown = 2200;

		public SplashScreen(string text) {
			this.text = text;
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (TransitionPosition == 0) countdown -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (countdown <= 0 && !IsExiting) {
				ExitScreen();
			}
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			SpriteFont font = GameSession.Current.UIFontMedium;
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawStringOutlined(font, text, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White * TransitionAlphaSquared, Color.Black, 0f, font.MeasureString(text) / 2, 1f + TransitionPosition * 5f);
			ScreenManager.SpriteBatch.End();
		}
	}
}
