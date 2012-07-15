using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	public class DialogueBox {
		public string Speaker;
		public TextBlock Text;

		public DialogueBox(TextBlock text) {
			Text = text;
			Text.FullyTyped = false;
		}
		public DialogueBox(TextBlock text, string speaker) {
			Text = text;
			Text.FullyTyped = false;
			Speaker = speaker;
		}
	}
}
