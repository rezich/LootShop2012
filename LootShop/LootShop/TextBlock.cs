using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	public class TextBlock {

		public List<Word> Words;

		public string Text {
			get {
				string ret = "";
				foreach (Word w in Words) ret += w.Text + " ";
				return ret.Trim();
			}
		}

		public List<string> WrappedText(SpriteFont font, int width) {
			Queue<string> words = new Queue<string>();
			List<string> final = new List<string> { "" };
			foreach (string w in Text.Split(' ')) words.Enqueue(w);
			while (words.Count > 0) {
				if (font.MeasureString(final[final.Count - 1] + (final[final.Count - 1] == "" ? "" : " ") + words.Peek()).X <= width) {
					final[final.Count - 1] += (final[final.Count - 1] == "" ? "" : " ") + words.Dequeue();
				}
				else final.Add(words.Dequeue());
			}
			return final;
		}

		public override string ToString() {
			return Text;
		}

		// TODO: maximum width, for text wrapping!
		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position) {
			float charHeight = font.MeasureString("W").Y;
			Vector2 offset = new Vector2(0, 0);
			foreach (Word w in Words) {
				switch (w.Type) {
					case Word.WordType.Newline:
						offset.X = 0;
						offset.Y += charHeight;
						break;
					case Word.WordType.Text:
						spriteBatch.DrawString(font, w.Text + " ", position + offset, Color.White);
						offset.X += font.MeasureString(w.Text + " ").X;
						break;
					case Word.WordType.Icon:
						int height = (int)charHeight;
						int width = w.Icon.Width / w.Icon.Height * height;
						spriteBatch.Draw(w.Icon, new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), width, height), Color.White);
						offset.X += width + font.MeasureString(" ").X;
						break;
				}
			}
		}

		public TextBlock(string text) {
			Words = new List<Word>();
			foreach (string w in text.Split(' ')) {
				if (w != "") Words.Add(new Word(w));
			}
		}

		public class Word {
			public enum WordType {
				Text,
				Icon,
				Newline
			}
			public WordType Type;
			public string Text;
			public Texture2D Icon;

			public static Texture2D StringToIcon(string text) {
				switch (text) {
					case "#A_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.A];
					case "#B_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.B];
					case "#X_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.X];
					case "#Y_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.Y];
					case "#START_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.Start];
					case "#BACK_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.Back];
					case "#LT_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.LeftTrigger];
					case "#LB_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.LeftShoulder];
					case "#RT_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.RightTrigger];
					case "#RB_BUTTON#": return LootShop.CurrentGame.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.RightShoulder];
				}
				return null;
			}

			public Word(string text) {
				text = text.Trim();
				Texture2D icon = StringToIcon(text);
				if (text == "#NL#") {
					Type = WordType.Newline;
					Text = text;
					return;
				}
				if (icon == null) {
					Type = WordType.Text;
					Text = text;
					return;
				}
				Type = WordType.Icon;
				Icon = icon;
			}
		}
	}
}