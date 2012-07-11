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
		public TextBlock Content = null;
		private List<Entry> entries = new List<Entry>();

		private int selectedIndex = 0;

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
			Vector2 entriesOrigin = new Vector2(8, (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (entriesHeight / 2));
			Vector2 titleOrigin = HasContent ? new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 3) * 2, 8) : new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2), 8);
			Rectangle contentRect = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 3), 60, (ScreenManager.GraphicsDevice.Viewport.Width / 3) * 2 - 40, 550);

			ScreenManager.SpriteBatch.Begin();

			ScreenManager.SpriteBatch.Draw(LootShop.CurrentGame.Pixel, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), new Color(0f, 0f, 0f, 0.85f));

			ScreenManager.SpriteBatch.DrawString(Font, Title, titleOrigin, Color.White, 0.0f, new Vector2(Font.MeasureString(Title).X / 2, 0), 1.0f, SpriteEffects.None, 0.0f);

			if (HasContent) {
				ScreenManager.SpriteBatch.Draw(LootShop.CurrentGame.Pixel, contentRect, Color.DarkOliveGreen);
				if (Content != null) Content.Draw(ScreenManager.SpriteBatch, LootShop.CurrentGame.UIFontSmall, new Vector2(contentRect.X, contentRect.Y), TextBlock.TextAlign.Left, contentRect.Width);
			}

			for (int i = 0; i < entries.Count; i++) {
				entries[i].Draw(ScreenManager.SpriteBatch, gameTime, entriesOrigin, i);
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
			get { return LootShop.CurrentGame.UIFontMedium; }
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

			public event EventHandler<PlayerIndexEventArgs> Selected;

			protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) {
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
			}

			public float Height {
				get { return Font.LineSpacing; }
			}

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, int position) {
				spriteBatch.DrawString(GenericMenu.Font, Text, origin + new Vector2(0, position * GenericMenu.Font.LineSpacing), IsSelected ? Color.White : Color.Gray);
			}

			public Entry(string text) {
				Text = text;
			}
			public Entry(string text, TextBlock content) {
				Text = text;
				Content = content;
			}
		}
	}
}
