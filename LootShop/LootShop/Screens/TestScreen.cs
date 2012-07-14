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
	public class TestScreen : GameScreen {
		Item item;
		TextBlock testBlock;

		public override void Initialize() {
			item = Item.Generate(GameSession.Random.Next(1, 50), GameSession.Random);
			testBlock = new TextBlock("#A_BUTTON# Generate loot #NL# #Y_BUTTON# Menu test #NL# #START_BUTTON# Return to main menu");
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsNewButtonPress(Buttons.A, ControllingPlayer, out playerIndex) || input.IsNewKeyPress(Keys.A, ControllingPlayer, out playerIndex)) item = Item.Generate(GameSession.Random.Next(1, 50), GameSession.Random);
			if (input.IsNewButtonPress(Buttons.Y, ControllingPlayer, out playerIndex) || input.IsNewKeyPress(Keys.Y, ControllingPlayer, out playerIndex)) ScreenManager.AddScreen(new TestMenu(), ControllingPlayer);
			if (input.IsPause(ControllingPlayer, out playerIndex)) {
				ScreenManager.BackToTitle(ControllingPlayer);
			}
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			StatBlock.Draw(ScreenManager.SpriteBatch, item);
			testBlock.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(8, 8));
			ScreenManager.SpriteBatch.End();
		}
	}
}
