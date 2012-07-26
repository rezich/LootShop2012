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
		public TitleScreen() {
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.BeginSpriteBatch();
			string title = "Loot Shop";
			string pressStart = InputState.InputMethod == InputMethods.Gamepad ? "PRESS START" : "PRESS ENTER";
			string copyright = "Copyright © 2012 108 Studios, all rights reserved.";
			ScreenManager.SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, pressStart, new Vector2(Resolution.Right / 2, Resolution.Bottom / 2 + 12), Color.Lerp(Color.Yellow, Color.Black, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5)) * TransitionAlphaSquared, Color.Black, 0.0f, GameSession.Current.UIFontSmall.MeasureString(pressStart) / 2, 1.0f);
			ScreenManager.SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontLarge, title, new Vector2(Resolution.Right / 2, Resolution.Bottom / 2 - 15), Color.White * TransitionAlphaSquared, Color.Black, 0.0f, GameSession.Current.UIFontLarge.MeasureString(title) / 2, 1f + 4f * TransitionPositionSquared);
			ScreenManager.SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, copyright, new Vector2(Resolution.Right / 2, Resolution.Bottom), Color.Gray * TransitionAlphaSquared, Color.Black, 0.0f, new Vector2(GameSession.Current.UIFontSmall.MeasureString(copyright).X / 2, GameSession.Current.UIFontSmall.MeasureString(copyright).Y), 1f);
			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsPressStart(null, out playerIndex)) {
				ExitScreen();
				ScreenManager.AddScreen(new MainMenu(), playerIndex);
			}
		}
	}
}
