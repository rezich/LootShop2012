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
		public bool HasContent = true;
		public bool DimBackground = true;
		bool initialized = false;
		public TextBlock Content = null;
		private List<Entry> entries = new List<Entry>();

		private int selectedIndex = 0;

		public override void Update(GameTime gameTime) {
			bool foundGoodOne = false;
			foreach (Entry e in entries) {
				if (!initialized) {
					if (!foundGoodOne && e.Enabled) {
						foundGoodOne = true;
						selectedIndex = entries.IndexOf(e);
						initialized = true;
					}
				}
				e.IsSelected = false;
			}
			entries[selectedIndex].IsSelected = true;
		}

		protected IList<Entry> MenuEntries {
			get { return entries; }
		}

		public override void HandleInput(InputState input) {
			if (input == null)
				throw new ArgumentNullException("input");

			if (input.IsMenuDown(ControllingPlayer)) {
				do {
					selectedIndex++;
					if (selectedIndex >= entries.Count) selectedIndex -= entries.Count;
				}
				while (!entries[selectedIndex].Enabled);
			}

			if (input.IsMenuUp(ControllingPlayer)) {
				do {
					selectedIndex--;
					if (selectedIndex < 0) selectedIndex += entries.Count;
				}
				while (!entries[selectedIndex].Enabled);

			}

			if (entries[selectedIndex].Content != null) Content = entries[selectedIndex].Content;
			else Content = null;

			PlayerIndex playerIndex;
			if (input.IsMenuSelect(ControllingPlayer, out playerIndex)) {
				OnSelectEntry(selectedIndex, playerIndex);
			}
			if (input.IsMenuCancel(ControllingPlayer, out playerIndex) && Cancelable) {
				OnCancel(playerIndex);
			}
		}

		public override void Draw(GameTime gameTime) {
			float entriesHeight = 0;
			foreach (Entry e in entries) {
				entriesHeight += e.Height;
			}
			float heightSoFar = 0;
			Vector2 entriesOrigin = new Vector2(24, (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (entriesHeight / 2));
			Vector2 titleOrigin = HasContent ? new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 3) * 2, 8) : new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2), 8);
			Rectangle contentRect = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 3), 60, (ScreenManager.GraphicsDevice.Viewport.Width / 3) * 2 - 40, 550);

			ScreenManager.SpriteBatch.Begin();

			if (DimBackground) ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), new Color(0f, 0f, 0f, 0.85f));

			if (Title != null) ScreenManager.SpriteBatch.DrawString(Font, Title, titleOrigin, Color.White, 0.0f, new Vector2(Font.MeasureString(Title).X / 2, 0), 1.0f, SpriteEffects.None, 0.0f);

			if (HasContent) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, contentRect, new Color(0.25f, 0.25f, 0.25f, 0.85f));
				if (Content != null) Content.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(contentRect.X, contentRect.Y), TextBlock.TextAlign.Left, contentRect.Width);
			}

			for (int i = 0; i < entries.Count; i++) {
				entries[i].Draw(ScreenManager.SpriteBatch, gameTime, entriesOrigin + new Vector2(0, heightSoFar));
				heightSoFar += entries[i].Height;
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

		public static SpriteFont Font {
			get { return GameSession.Current.UIFontMedium; }
		}

		public GenericMenu(string menuTitle) {
			Title = menuTitle;
		}

		public GenericMenu(string menuTitle, bool cancelable, bool hasContent) {
			Title = menuTitle;
			Cancelable = cancelable;
			HasContent = hasContent;
		}

		public class Entry {
			public string Text;
			public TextBlock Content;
			public bool IsSelected = false;
			public bool Enabled = true;

			public event EventHandler<PlayerIndexEventArgs> Selected;

			protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) {
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
			}

			public float Height {
				get { return Font.LineSpacing; }
			}

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin) {
				spriteBatch.DrawString(GenericMenu.Font, Text, origin, Enabled ? (IsSelected ? Color.White : new Color(128, 128, 128)) : new Color(64, 64, 64));
			}

			public Entry(string text) {
				Text = text;
			}
			public Entry(string text, TextBlock content) {
				Text = text;
				Content = content;
			}
			public Entry(string text, bool enabled) {
				Text = text;
				Enabled = enabled;
			}
		}
	}
}
