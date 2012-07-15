using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	public class DialogueBox {
		public TextBlock Text;

		public DialogueBox(TextBlock text) {
			Text = text;
			Text.FullyTyped = false;
		}
	}
}
