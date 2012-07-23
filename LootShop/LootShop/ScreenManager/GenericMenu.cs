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
		public TextBlock Description;
		public bool Cancelable = true;
		public bool HasContent = true;
		public bool DimBackground = true;
		bool initialized = false;
		public TextBlock Content = null;
		private List<Entry> entries = new List<Entry>();

		private int selectedIndex = 0;

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
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
				if (e.Visible) entriesHeight += e.Height;
			}
			float heightSoFar = 0;
			int left = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Left;
			int top = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Top;
			int right = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Right;
			int bottom = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Bottom;

			int padding = 8;
			int margin = 16;
			int entriesWidth = 270;

			SpriteFont descriptionFont = GameSession.Current.UIFontSmall;

			Vector2 entriesOrigin = new Vector2(left + margin, (ScreenManager.GraphicsDevice.Viewport.Height / 2) - (entriesHeight / 2));
			Rectangle descriptionRect = RectangleHelper.FromVectors(new Vector2(left + entriesWidth + margin * 2, bottom - descriptionFont.LineSpacing - margin - padding * 2), new Vector2(right - margin, bottom - margin));
			Vector2 titleOrigin = new Vector2(0, top + margin);
			Rectangle contentRect = RectangleHelper.FromVectors(new Vector2(left + entriesWidth + margin * 2, titleOrigin.Y + Font.LineSpacing + margin), new Vector2(right - margin, Description == null ? bottom - margin : descriptionRect.Top - margin));
			titleOrigin.X = contentRect.Center.X;

			ScreenManager.SpriteBatch.Begin();

			if (DimBackground) ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), new Color(0f, 0f, 0f, 0.85f));

			if (Title != null) ScreenManager.SpriteBatch.DrawStringOutlined(Font, Title, titleOrigin, Color.White, Color.Black, 0.0f, new Vector2(Font.MeasureString(Title).X / 2, 0), 1f);

			if (HasContent) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, contentRect, new Color(0.25f, 0.25f, 0.25f));
				if (Content != null) Content.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(contentRect.X + padding, contentRect.Y + padding), TextBlock.TextAlign.Left, contentRect.Width - padding * 2);
			}

			if (Description != null) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, descriptionRect, new Color(0.35f, 0.35f, 0.35f));
				Description.Draw(ScreenManager.SpriteBatch, descriptionFont, new Vector2(descriptionRect.X + padding, descriptionRect.Y + padding), TextBlock.TextAlign.Left, descriptionRect.Width - padding * 2);
			}

			for (int i = 0; i < entries.Count; i++) {
				if (entries[i].Visible) {
					entries[i].Draw(ScreenManager.SpriteBatch, gameTime, entriesOrigin + new Vector2(0, heightSoFar));
					heightSoFar += entries[i].Height;
				}
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
			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public GenericMenu(string menuTitle, bool cancelable, bool hasContent) {
			Title = menuTitle;
			Cancelable = cancelable;
			HasContent = hasContent;
			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public class Entry {
			public string Text;
			public TextBlock Content;
			public bool IsSelected = false;
			public bool Enabled {
				get {
					return enabled && Visible;
				}
				set {
					enabled = value;
				}
			}
			public bool Visible = true;
			bool enabled = true;

			public event EventHandler<PlayerIndexEventArgs> Selected;

			protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) {
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
			}

			public float Height {
				get { return Font.LineSpacing; }
			}

			public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin) {
				spriteBatch.DrawStringOutlined(GenericMenu.Font, Text, origin, Enabled ? (IsSelected ? Color.White : new Color(192, 192, 192)) : new Color(96, 96, 96));
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
