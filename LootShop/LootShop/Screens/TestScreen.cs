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
			item = Item.Generate(LootShop.Random.Next(1, 50), LootShop.Random);
			testBlock = new TextBlock("#A_BUTTON# Generate loot #NL# #Y_BUTTON# Menu test #NL# #START_BUTTON# Return to title");
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsNewButtonPress(Buttons.A, ControllingPlayer, out playerIndex)) item = Item.Generate(LootShop.Random.Next(1, 50), LootShop.Random);
			if (input.IsNewButtonPress(Buttons.Y, ControllingPlayer, out playerIndex)) ScreenManager.AddScreen(new TestMenu(), ControllingPlayer);
			if (input.IsPause(ControllingPlayer, out playerIndex)) ScreenManager.ReplaceScreen(new TitleScreen(), null);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			StatBlock.Draw(ScreenManager.SpriteBatch, item);
			testBlock.Draw(ScreenManager.SpriteBatch, LootShop.CurrentGame.UIFontSmall, new Vector2(8, 8));
			ScreenManager.SpriteBatch.End();
		}
	}
}
