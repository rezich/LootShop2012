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
		Campaign Campaign = new Campaign();

		public GameplayScreen() {
		}

		public override void Initialize() {
			ScreenManager.AddScreen(new TownMap(Campaign), ControllingPlayer);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(GameSession.Current.UIFontMedium, ((Time.MonthName)Campaign.Time.Month).ToString() + " " + Campaign.Time.Day + ", " + Campaign.Time.Year + "\n" + ((Time.TimeOfDayName)Campaign.Time.TimeOfDay).ToString().DeCamelCase(), Vector2.Zero, Color.White);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
