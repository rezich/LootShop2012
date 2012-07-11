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
		public bool Cancelable = true;
		private List<Entry> entries = new List<Entry>();

		private int selectedIndex = 0;

		public Vector2 Origin = Vector2.Zero;

		public override void Update(GameTime gameTime) {
			foreach (Entry e in entries) e.IsSelected = false;
			entries[selectedIndex].IsSelected = true;
		}

		protected IList<Entry> MenuEntries {
			get { return entries; }
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
			if (input.IsMenuSelect(ControllingPlayer, out playerIndex)) {
				OnSelectEntry(selectedIndex, playerIndex);
			}
			if (input.IsMenuCancel(ControllingPlayer, out playerIndex) && Cancelable) {
				OnCancel(playerIndex);
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

		protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex) {
			entries[entryIndex].OnSelectEntry(playerIndex);
		}
		protected virtual void OnCancel(PlayerIndex playerIndex) {
			ScreenManager.RemoveScreen(this);
		}
		protected void OnCancel(object sender, PlayerIndexEventArgs e) {
			OnCancel(e.PlayerIndex);
		}

		public GenericMenu(string menuTitle) {
			Title = menuTitle;
		}

		public GenericMenu(string menuTitle, bool cancelable) {
			Title = menuTitle;
			Cancelable = cancelable;
		}

		public class Entry {
			public string Text;
			public bool IsSelected = false;

			public event EventHandler<PlayerIndexEventArgs> Selected;

			protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) {
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
			}

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, int position) {
				spriteBatch.DrawString(LootShop.CurrentGame.UIFontSmall, Text, origin + new Vector2(0, position * 20), IsSelected ? Color.White : Color.Gray);
			}

			public Entry(string text) {
				Text = text;
			}
		}
	}
}
