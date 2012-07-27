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

			Color boxColor1 = new Color(0.2f, 0.2f, 0.2f) * TransitionAlpha;
			Color boxColor2 = new Color(0.3f, 0.3f, 0.3f) * TransitionAlpha;

			int boxWidth = Resolution.Right - margin * 2;
			int boxHeight = font.LineSpacing * 4 + padding * 2;
			int boxLeft = Resolution.Left + margin;
			int boxTop = Resolution.Bottom - margin - boxHeight;

			ScreenManager.BeginSpriteBatch();

			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(boxLeft, boxTop, boxWidth, boxHeight), boxColor1);
			DialogueBox.Text.Draw(ScreenManager.SpriteBatch, GameSession.Current.DialogueFont, new Vector2(boxLeft + padding, boxTop + padding), TextBlock.TextAlign.Left, boxWidth - padding * 2, TransitionAlpha);
			if (DialogueBox.Text.FullyTyped) ScreenManager.SpriteBatch.Draw(GameSession.Current.ButtonImages[Buttons.A], new Rectangle(boxLeft + boxWidth - padding - 32, boxTop + boxHeight - padding - 32, 32, 32), Color.White * TransitionAlpha);
			if (DialogueBox.Speaker != null) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(boxLeft, boxTop - font.LineSpacing - padding * 2, (int)GameSession.Current.DialogueFont.MeasureString(DialogueBox.Speaker).X + padding * 2, GameSession.Current.DialogueFont.LineSpacing + padding * 2), boxColor2);
				ScreenManager.SpriteBatch.DrawStringOutlined(font, DialogueBox.Speaker, new Vector2(boxLeft + padding, boxTop - font.LineSpacing - padding), Color.White * TransitionAlpha);
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
			if (input.IsInput(Inputs.MenuAccept, ControllingPlayer)) {
				if (DialogueBox.Text.FullyTyped) ExitScreen();
				else DialogueBox.Text.FullyTyped = true;
			}
		}

		public DialogueScreen(DialogueBox dialogueBox) {
			DialogueBox = dialogueBox;
			TransitionOnTime = TimeSpan.FromSeconds(0.2);
			TransitionOffTime = TimeSpan.FromSeconds(0.2);
		}
	}
}
