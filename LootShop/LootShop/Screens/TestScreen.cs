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
			testBlock = new TextBlock("#MENU_ACCEPT# Generate loot #NL# #MENU_CANCEL# Return to main menu");
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsMenuSelect(ControllingPlayer, out playerIndex)) item = Item.Generate(GameSession.Random.Next(1, 50), GameSession.Random);
			if (input.IsMenuCancel(ControllingPlayer, out playerIndex)) {
				ScreenManager.BackToTitle(ControllingPlayer);
			}
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			StatBlock.Draw(ScreenManager.SpriteBatch, item);
			testBlock.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Left + 8, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Top + 8));
			ScreenManager.SpriteBatch.End();
		}
	}
}
