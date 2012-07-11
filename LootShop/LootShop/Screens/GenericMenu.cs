using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	public class GenericMenu {
		public string Title;
		public string Description;
		private List<Entry> entries = new List<Entry> {
			new Entry("Thing 1"),
			new Entry("Thing 2"),
			new Entry("Thing 3")
		};

		private int selectedIndex = 0;

		public Vector2 Origin = Vector2.Zero;

		private class Entry {
			public string Text;
			public bool Selected = false;

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, int position) {
				spriteBatch.DrawString(LootShop.CurrentGame.UIFontSmall, Text, origin + new Vector2(0, position * 20), Selected ? Color.White : Color.Gray);
			}

			public Entry(string text) {
				Text = text;
			}
		}

		public void Update(GameTime gameTime) {
			foreach (Entry e in entries) e.Selected = false;
			entries[selectedIndex].Selected = true;
		}

		public void HandleInput(InputState input, int playerIndex) {
			if (input == null)
				throw new ArgumentNullException("input");

			// Look up inputs for the active player profile.

			KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
			KeyboardState lastKeyboardState = input.LastKeyboardStates[playerIndex];
			GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
			GamePadState lastGamePadState = input.LastGamePadStates[playerIndex];

			/*if (input.IsMenuDown((PlayerIndex)playerIndex)) {
				selectedIndex++;
				if (selectedIndex >= entries.Count) selectedIndex -= entries.Count;
			}

			if (input.IsMenuUp((PlayerIndex)playerIndex)) {
				selectedIndex--;
				if (selectedIndex < 0) selectedIndex += entries.Count;
			}*/
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) {
			for (int i = 0; i < entries.Count; i++) {
				entries[i].Draw(spriteBatch, gameTime, Origin, i);
			}
		}
	}
}
