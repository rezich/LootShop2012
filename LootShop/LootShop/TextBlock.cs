using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextBlock {

	// TODO: Later split this into words or some shit
	//       potentially in order to, for example,
	//       have button icons in the middle of on-screen
	//       text boxes.

	public string Text;

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

	public TextBlock(string text) {
		Text = text;
	}
}