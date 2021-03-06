﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {	
	class SplashScreen : GameScreen {
		string text;
		TimeSpan countdown;

		public SplashScreen(string text, TimeSpan length) {
			this.text = text;
			countdown = length;
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			/*if (TransitionPosition == 0)*/ countdown -= gameTime.ElapsedGameTime;
			if (countdown <= TimeSpan.Zero && !IsExiting) {
				ExitScreen();
			}
		}

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			SpriteFont font = GameSession.Current.UIFontMedium;
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.DrawStringOutlined(font, text, new Vector2(Resolution.Right / 2, Resolution.Bottom / 2), Color.White * TransitionAlphaSquared, Color.Black, 0f, font.MeasureString(text) / 2, 1f + TransitionPosition * 5f);
			ScreenManager.SpriteBatch.End();
		}
	}
}
