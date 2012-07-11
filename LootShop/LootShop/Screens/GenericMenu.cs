using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	public class GenericMenu : GameScreen {
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

		public override void Update(GameTime gameTime) {
			foreach (Entry e in entries) e.Selected = false;
			entries[selectedIndex].Selected = true;
		}

		public override void HandleInput(InputState input) {
			if (input == null)
				throw new ArgumentNullException("input");

			if (input.IsMenuDown(ControllingPlayer)) {
				selectedIndex++;
				if (selectedIndex >= entries.Count) selectedIndex -= entries.Count;
			}

			if (input.IsMenuUp(ControllingPlayer)) {
				selectedIndex--;
				if (selectedIndex < 0) selectedIndex += entries.Count;
			}

			PlayerIndex playerIndex;
			if (input.IsMenuCancel(ControllingPlayer, out playerIndex)) {
				ScreenManager.RemoveScreen(this);
			}
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(LootShop.CurrentGame.Pixel, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), new Color(0f, 0f, 0f, 0.85f));
			for (int i = 0; i < entries.Count; i++) {
				entries[i].Draw(ScreenManager.SpriteBatch, gameTime, Origin, i);
			}
			ScreenManager.SpriteBatch.End();
		}
	}
}
