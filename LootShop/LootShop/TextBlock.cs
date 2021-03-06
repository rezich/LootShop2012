using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	public class TextBlock {
		public enum TextAlign {
			Left,
			Center,
			Right
		}
		public List<Word> Words;
		public bool FullyTyped = true;
		public int CurrentWordIndex;

		public string Text {
			get {
				return String.Join(" ", Words.Select(x => x.Text).ToArray());
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

		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position) {
			Draw(spriteBatch, font, position, TextAlign.Left, null);
		}
		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, TextAlign align) {
			Draw(spriteBatch, font, position, align, null);
		}
		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, TextAlign align, int? width) {
			Draw(spriteBatch, font, position, align, width, 1f);
		}
		public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, TextAlign align, int? width, float alpha) {
			Vector2 offset = new Vector2(0, 0);
			//foreach (Word w in Words) {
			for (int i = 0; i <= (FullyTyped ? Words.Count - 1 : CurrentWordIndex); i++) {
				Word w = Words[i];
				switch (w.Type) {
					case Word.WordType.Newline:
						offset.X = 0;
						offset.Y += font.LineSpacing;
						break;
					case Word.WordType.Text:
						if (offset.X + font.MeasureString(w.Text + " ").X > width) {
							offset.X = 0;
							offset.Y += font.LineSpacing;
						}
						spriteBatch.DrawStringOutlined(font, w.Text + " ", position + offset, w.Color * alpha);
						offset.X += font.MeasureString(w.Text + " ").X;
						break;
					case Word.WordType.Icon:
						int iconHeight = (int)font.LineSpacing;

						if (w.Icon == GameSession.Current.Pixel) {
							spriteBatch.DrawStringOutlined(GameSession.Current.KeyboardFont, w.Text, position + offset, w.Color * alpha);
							offset.X += GameSession.Current.KeyboardFont.MeasureString(w.Text).X + font.MeasureString(" ").X;
						}
						else {
							int iconWidth = w.Icon.Width / w.Icon.Height * iconHeight;
							if (offset.X + w.Icon.Width > width) {
								offset.X = 0;
								offset.Y += font.LineSpacing;
							}
							spriteBatch.Draw(w.Icon, new Rectangle((int)(position.X + offset.X), (int)(position.Y + offset.Y), iconWidth, iconHeight), Color.White * alpha);
							offset.X += iconWidth + font.MeasureString(" ").X;
						}
						break;
				}
			}
		}

		public void Type() {
			if (CurrentWordIndex < Words.Count - 1) CurrentWordIndex++;
			if (CurrentWordIndex == Words.Count - 1) FullyTyped = true;
		}

		public TextBlock(string text) {
			text = text.Replace("[", " #C_SPECIAL# ").Replace("]", " #C_NORMAL# ").Trim();
			Words = new List<Word>();
			Color color = Color.White;
			foreach (string w in text.Split(' ')) {
				Color? newColor = Word.StringToColor(w);
				if (newColor != null) {
					color = (Color)newColor;
					continue;
				}
				if (w != "") Words.Add(new Word(w, color));
			}
			CurrentWordIndex = 0;
		}

		public class Word {
			public enum WordType {
				Text,
				Icon,
				Newline
			}
			public WordType Type;
			public string Text {
				get {
					switch (text) {
						case "#MENU_ACCEPT#": return "x";
						case "#MENU_CANCEL#": return "z";
					}
					return text;
				}
			}
			private string text;
			public Texture2D Icon {
				get {
					return StringToIcon(text);
				}
			}
			public Color Color;

			public static Texture2D StringToIcon(string text) {
				switch (text) {
					case "#MENU_ACCEPT#": return InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.A] : GameSession.Current.KeyImages["X"];
					case "#MENU_CANCEL#": return InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.B] : GameSession.Current.KeyImages["Z"];

					case "#GAME_PAUSE#": return InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Microsoft.Xna.Framework.Input.Buttons.Start] : GameSession.Current.KeyImages["Esc"]; ;
				}
				return null;
			}

			public static Color? StringToColor(string text) {
				switch (text) {
					case "#C_NORMAL#": return Color.White;
					case "#C_SPECIAL#": return Color.Violet;
				}
				return null;
			}

			public Word(string text, Color color) {
				Color = color;
				this.text = text.Trim();
				Texture2D icon = StringToIcon(this.text);
				if (this.text == "#NL#") {
					Type = WordType.Newline;
					return;
				}
				if (icon == null) {
					Type = WordType.Text;
					return;
				}
				Type = WordType.Icon;
			}

			public override string ToString() {
				return Text;
			}
		}
	}
}
