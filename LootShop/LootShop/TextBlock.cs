using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

	public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position) {
		Vector2 offset = new Vector2(0, 0);
		foreach (Word w in Words) {
			spriteBatch.DrawString(font, w.Text + " ", position + offset, Color.White);
			offset.X += font.MeasureString(w.Text + " ").X;
		}
	}

	public TextBlock(string text) {
		Words = new List<Word>();
		foreach (string w in text.Split(' ')) {
			Words.Add(new Word(w));
		}
	}

	public class Word {
		public enum WordType {
			Text,
			Icon
		}
		public WordType Type;
		public string Text;
		public Word(string text) {
			Type = WordType.Text;
			Text = text;
		}
	}
}