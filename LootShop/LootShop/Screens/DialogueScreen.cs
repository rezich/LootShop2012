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
		int textDelay = 2;
		int textCountdown = 0;

		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			int padding = 8;
			int margin = 8;
			SpriteFont font = GameSession.Current.DialogueFont;

			Color boxColor1 = new Color(0.2f, 0.2f, 0.2f);
			Color boxColor2 = new Color(0.3f, 0.3f, 0.3f);

			int boxWidth = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Width - margin * 2;
			int boxHeight = font.LineSpacing * 4 + padding * 2;
			int boxLeft = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Left + margin;
			int boxTop = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Bottom - margin - boxHeight;

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(boxLeft, boxTop, boxWidth, boxHeight), boxColor1);
			DialogueBox.Text.Draw(ScreenManager.SpriteBatch, GameSession.Current.DialogueFont, new Vector2(boxLeft + padding, boxTop + padding), TextBlock.TextAlign.Left, boxWidth - padding * 2);
			if (DialogueBox.Text.FullyTyped) ScreenManager.SpriteBatch.Draw(GameSession.Current.ButtonImages[Buttons.A], new Rectangle(boxLeft + boxWidth - padding - 32, boxTop + boxHeight - padding - 32, 32, 32), Color.White);
			if (DialogueBox.Speaker != null) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(boxLeft, boxTop - font.LineSpacing - padding * 2, (int)GameSession.Current.DialogueFont.MeasureString(DialogueBox.Speaker).X + padding * 2, GameSession.Current.DialogueFont.LineSpacing + padding * 2), boxColor2);
				ScreenManager.SpriteBatch.DrawStringOutlined(font, DialogueBox.Speaker, new Vector2(boxLeft + padding, boxTop - font.LineSpacing - padding), Color.White);
			}
			ScreenManager.SpriteBatch.End();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (textCountdown < textDelay) textCountdown++;
			else if (textCountdown == textDelay) {
				textCountdown = 0;
				DialogueBox.Text.Type();
			}
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
