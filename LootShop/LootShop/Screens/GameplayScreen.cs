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
	public class GameplayScreen : GameScreen {
		public GameplayScreen() {
			GameSession.Current.Campaign = new Campaign();
		}

		public override void Initialize() {
			ScreenManager.AddScreen(new TownMap(), ControllingPlayer);
		}

		public override void Draw(GameTime gameTime) {
			int padding = 8;
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(GameSession.Current.UIFontMedium, ((Time.MonthName)GameSession.Current.Campaign.Time.Month).ToString() + " " + GameSession.Current.Campaign.Time.Day + ", " + GameSession.Current.Campaign.Time.Year + "\n" + ((Time.TimeOfDayName)GameSession.Current.Campaign.Time.TimeOfDay).ToString().DeCamelCase(), new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Left + padding, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Top + padding), Color.White);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
