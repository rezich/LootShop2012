using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootShop {
	public class Cutscene {
		public List<DialogueBox> DialogueBoxes;

		public Cutscene(List<DialogueBox> dialogueBoxes) {
			DialogueBoxes = dialogueBoxes;
		}
	}
}
