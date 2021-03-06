﻿using System;
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
		Cutscene Cutscene;
		int actionIndex = 0;

		public CutsceneScreen(string name) {
			Cutscene = Cutscene.Lookup(name);
		}

		public CutsceneScreen(List<CutsceneAction> actions) {
			Cutscene = new Cutscene(null, actions);
		}

		public override void Initialize() {
			if (Cutscene.Actions.Count == 0) throw new Exception("Empty cutscene?!");
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (TopActive) {
				if (actionIndex < Cutscene.Actions.Count) {
					if (Cutscene.Actions[actionIndex] is DialogueAction) {
						ScreenManager.AddScreen(new DialogueScreen(new DialogueBox(new TextBlock(((DialogueAction)Cutscene.Actions[actionIndex]).Text), ((DialogueAction)Cutscene.Actions[actionIndex]).Speaker)), ControllingPlayer);
						actionIndex++;
					}
				}
				else ExitScreen();
			}
		}
	}
}
