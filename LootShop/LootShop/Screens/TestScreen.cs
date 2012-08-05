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

		public TestScreen() {
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public override void Initialize() {
			//item = Item.Generate(GameSession.Random.Next(1, 50), GameSession.Random);
			testBlock = new TextBlock("#MENU_ACCEPT# Generate loot #NL# #MENU_CANCEL# Return to main menu");
		}

		public override void HandleInput(InputState input) {
			if (input.IsInput(Inputs.MenuAccept, ControllingPlayer)) {
				GameSession.Current.MenuAccept.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				item = Item.Generate(GameSession.Random.Next(1, 50), GameSession.Random);
			}
			if (input.IsInput(Inputs.MenuCancel, ControllingPlayer)) {
				//GameSession.Current.StartFromSplashScreens();
				ExitScreen();
			}
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Black * TransitionAlphaSquared);
			if (item != null) StatBlock.Draw(ScreenManager.SpriteBatch, item);
			testBlock.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(Resolution.Left + 8, Resolution.Top + 8), TextBlock.TextAlign.Left, null, TransitionAlphaSquared);
			ScreenManager.SpriteBatch.End();
		}
	}
}
