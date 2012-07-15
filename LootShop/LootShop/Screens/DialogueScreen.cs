using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	class DialogueScreen : GameScreen {
		DialogueBox DialogueBox;

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(32, ScreenManager.GraphicsDevice.Viewport.Height - 182, ScreenManager.GraphicsDevice.Viewport.Width - 64, 150), Color.DodgerBlue);
			DialogueBox.Text.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(32, ScreenManager.GraphicsDevice.Viewport.Height - 182), TextBlock.TextAlign.Left, ScreenManager.GraphicsDevice.Viewport.Width - 64);
			if (DialogueBox.Text.FullyTyped) ScreenManager.SpriteBatch.Draw(GameSession.Current.ButtonImages[Buttons.A], new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width - 64, ScreenManager.GraphicsDevice.Viewport.Height - 64, 32, 32), Color.White);
			ScreenManager.SpriteBatch.End();
		}

		public override void Update(GameTime gameTime) {
			DialogueBox.Text.Type();
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsMenuSelect(ControllingPlayer, out playerIndex)) {
				if (DialogueBox.Text.FullyTyped) ScreenManager.RemoveScreen();
				else DialogueBox.Text.FullyTyped = true;
			}
		}

		public DialogueScreen(DialogueBox dialogueBox) {
			DialogueBox = dialogueBox;
		}
	}
}
