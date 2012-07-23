using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LootSystem {
	public class Cutscene {
		public static List<Cutscene> List = new List<Cutscene>();
		public List<CutsceneAction> Actions = new List<CutsceneAction>();

		public Cutscene(List<CutsceneAction> actions) {
			Actions = actions;
		}
	}

	public abstract class CutsceneAction {
	}

	public class DialogueAction : CutsceneAction {
		public string Text;
		public string Speaker;
		public DialogueAction(string text) {
			Text = text;
		}
		public DialogueAction(string text, string speaker) {
			Text = text;
			Speaker = speaker;
		}
	}
}
