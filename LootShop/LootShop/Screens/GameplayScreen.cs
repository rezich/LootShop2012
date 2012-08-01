using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

namespace LootShop {
	public class GameplayScreen : GameScreen {
		public GameplayScreen() {
			GameSession.Current.Campaign = new Campaign();
		}

		public override void LoadContent() {
			Thread.Sleep(1000);
		}

		public override void Initialize() {
			MediaPlayer.Play(GameSession.Current.IntroTheme);
			ScreenManager.AddScreen(new TownMap(), ControllingPlayer);
		}

		public override void Draw(GameTime gameTime) {
			int padding = 8;
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.DrawString(GameSession.Current.UIFontMedium, ((Time.MonthName)GameSession.Current.Campaign.Time.Month).ToString() + " " + GameSession.Current.Campaign.Time.Day + ", " + GameSession.Current.Campaign.Time.Year + "\n" + ((Time.TimeOfDayName)GameSession.Current.Campaign.Time.TimeOfDay).ToString().DeCamelCase(), new Vector2(Resolution.Left + padding, Resolution.Top + padding), Color.White);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
