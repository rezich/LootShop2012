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
	class CutsceneScreen : GameScreen {
		Cutscene Cutscene = new Cutscene(new List<DialogueBox>() {
			new DialogueBox(new TextBlock("The path of #C_SPECIAL# the righteous man #C_NORMAL# is beset on all sides by the iniquities of the selfish and the tyranny of evil men."), "???"),
			new DialogueBox(new TextBlock("Blessed is he who, in the name of charity and good will, shepherds the weak through the valley of darkness, for he is truly his brother's keeper and the finder of lost children."), "Samuel L. Jackson"),
			new DialogueBox(new TextBlock("And I will strike down upon thee with great vengeance and furious anger those who would attempt to poison and destroy My brothers. And you will know My name is the Lord when I lay My vengeance upon thee."), "Samuel L. Jackson"),
			new DialogueBox(new TextBlock("Normally, both your asses would be dead as #C_SPECIAL# fucking fried chicken #C_NORMAL# , but you happen to pull this shit while I'm in a transitional period so I don't wanna kill you, I wanna help you."), "Samuel L. Jackson"),
			new DialogueBox(new TextBlock("But I can't give you this case, it don't belong to me. Besides, I've already been through too much shit this morning over this case to hand it over to your dumb ass."), "Samuel L. Jackson")
		});
		int dialogueBoxIndex = 0;

		public override void Initialize() {
			if (Cutscene.DialogueBoxes.Count == 0) throw new Exception("Empty cutscene?!");
		}

		public override void Update(GameTime gameTime) {
			if (dialogueBoxIndex < Cutscene.DialogueBoxes.Count) {
				ScreenManager.AddScreen(new DialogueScreen(Cutscene.DialogueBoxes[dialogueBoxIndex]), ControllingPlayer);
				dialogueBoxIndex++;
			}
			else ScreenManager.RemoveScreen();
		}
	}
}
