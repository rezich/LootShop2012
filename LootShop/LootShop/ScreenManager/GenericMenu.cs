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

			if (input.IsInput(Inputs.MenuDown, ControllingPlayer)) {
				do {
					selectedIndex++;
					if (selectedIndex >= entries.Count) selectedIndex -= entries.Count;
				}
				while (!entries[selectedIndex].Enabled);
			}

			if (input.IsInput(Inputs.MenuUp, ControllingPlayer)) {
				do {
					selectedIndex--;
					if (selectedIndex < 0) selectedIndex += entries.Count;
				}
				while (!entries[selectedIndex].Enabled);

			}

			if (entries[selectedIndex].Content != null) Content = entries[selectedIndex].Content;
			else Content = null;

			PlayerIndex playerIndex;
			if (input.IsInput(Inputs.MenuAccept, ControllingPlayer, out playerIndex)) {
				OnSelectEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuLeft, ControllingPlayer, out playerIndex)) {
				OnSwipeLeftEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuRight, ControllingPlayer, out playerIndex)) {
				OnSwipeRightEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuCancel, ControllingPlayer, out playerIndex) && Cancelable) {
				OnCancel(playerIndex);
			}
		}

		public override void Draw(GameTime gameTime) {
			float entriesHeight = 0;
			foreach (Entry e in entries) {
				if (e.Visible) entriesHeight += e.Height;
			}
			float heightSoFar = 0;
			int left = Resolution.Left;
			int top = Resolution.Top;
			int right = Resolution.Right;
			int bottom = Resolution.Bottom;

			int padding = 8;
			int margin = 16;
			int entriesWidth = 388;

			SpriteFont descriptionFont = GameSession.Current.UIFontSmall;
			SpriteFont backFont = GameSession.Current.UIFontSmall;

			Vector2 entriesOrigin = new Vector2(left + margin - entriesWidth * (TransitionPositionSquared), (Resolution.Bottom / 2) - (entriesHeight / 2));
			Rectangle descriptionRect = RectangleHelper.FromVectors(new Vector2(left + entriesWidth + margin * 2, bottom - descriptionFont.LineSpacing - margin - padding * 2), new Vector2(right - margin, bottom - margin));
			//descriptionRect.Height = Convert.ToInt32((float)descriptionRect.Height * (1 - TransitionPositionSquared));
			Rectangle cancelRect = RectangleHelper.FromVectors(new Vector2(left + margin, bottom - backFont.LineSpacing - margin - padding * 2), new Vector2(left + entriesWidth + margin, bottom - margin));
			Vector2 titleOrigin = new Vector2(0, top + margin);
			Rectangle contentRect = RectangleHelper.FromVectors(new Vector2(left + entriesWidth + margin * 2, titleOrigin.Y + Font.LineSpacing + margin), new Vector2(right - margin, Description == null ? bottom - margin : descriptionRect.Top - margin));
			contentRect.Height = Convert.ToInt32((float)contentRect.Height * (1 - TransitionPositionSquared));
			titleOrigin.X = contentRect.Center.X;

			ScreenManager.BeginSpriteBatch();

			if (DimBackground) ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(0, 0, Resolution.Right, Resolution.Bottom), new Color(0f, 0f, 0f, 0.85f) * TransitionAlpha);

			if (Title != null) ScreenManager.SpriteBatch.DrawStringOutlined(Font, Title, titleOrigin, Color.White * TransitionAlpha, Color.Black, 0.0f, new Vector2(Font.MeasureString(Title).X / 2, 0), 1f);

			if (HasContent) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, contentRect, new Color(0.25f, 0.25f, 0.25f) * TransitionAlpha);
				if (Content != null) Content.Draw(ScreenManager.SpriteBatch, GameSession.Current.UIFontSmall, new Vector2(contentRect.X + padding, contentRect.Y + padding), TextBlock.TextAlign.Left, contentRect.Width - padding * 2, TransitionAlpha);
			}

			if (Description != null) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, descriptionRect, new Color(0.35f, 0.35f, 0.35f) * TransitionAlpha);
				Description.Draw(ScreenManager.SpriteBatch, descriptionFont, new Vector2(descriptionRect.X + padding, descriptionRect.Y + padding), TextBlock.TextAlign.Left, descriptionRect.Width - padding * 2, TransitionAlpha);
			}

			if (Cancelable && HasContent) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, cancelRect, new Color(0.35f, 0.35f, 0.35f) * TransitionAlpha);
				TextBlock backBlock = new TextBlock("#MENU_CANCEL# Back");
				backBlock.Draw(ScreenManager.SpriteBatch, backFont, new Vector2(cancelRect.X + padding, cancelRect.Y + padding));
			}

			for (int i = 0; i < entries.Count; i++) {
				if (entries[i].Visible) {
					entries[i].Draw(ScreenManager.SpriteBatch, gameTime, entriesOrigin + new Vector2(0, heightSoFar), TransitionAlpha);
					heightSoFar += entries[i].Height;
				}
			}

			ScreenManager.SpriteBatch.End();
		}

		protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex) {
			entries[entryIndex].OnSelectEntry(playerIndex);
		}
		protected virtual void OnSwipeLeftEntry(int entryIndex, PlayerIndex playerIndex) {
			entries[entryIndex].OnSwipeLeftEntry(playerIndex);
		}
		protected virtual void OnSwipeRightEntry(int entryIndex, PlayerIndex playerIndex) {
			entries[entryIndex].OnSwipeRightEntry(playerIndex);
		}
		protected virtual void OnCancel(PlayerIndex playerIndex) {
			ExitScreen();
		}
		protected void OnCancel(object sender, PlayerIndexEventArgs e) {
			OnCancel(e.PlayerIndex);
		}

		public static SpriteFont Font {
			get { return GameSession.Current.UIFontMedium; }
		}

		public GenericMenu(string menuTitle) {
			Title = menuTitle;
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public GenericMenu(string menuTitle, bool cancelable, bool hasContent) {
			Title = menuTitle;
			Cancelable = cancelable;
			HasContent = hasContent;
			TransitionOnTime = TimeSpan.FromSeconds(0.35);
			TransitionOffTime = TimeSpan.FromSeconds(0.25);
		}

		public class Entry {
			public string Text;
			public TextBlock Content;
			public bool IsSelected = false;
			public bool Enabled {
				get {
					return selectable && Visible;
				}
				set {
					selectable = value;
				}
			}
			public bool Visible = true;
			protected bool selectable = true;

			public event EventHandler<PlayerIndexEventArgs> Selected;
			public event EventHandler<PlayerIndexEventArgs> SwipeLeft;
			public event EventHandler<PlayerIndexEventArgs> SwipeRight;

			protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) {
				if (Selected != null)
					Selected(this, new PlayerIndexEventArgs(playerIndex));
			}

			protected internal virtual void OnSwipeLeftEntry(PlayerIndex playerIndex) {
				if (SwipeLeft != null)
					SwipeLeft(this, new PlayerIndexEventArgs(playerIndex));
			}

			protected internal virtual void OnSwipeRightEntry(PlayerIndex playerIndex) {
				if (SwipeRight != null)
					SwipeRight(this, new PlayerIndexEventArgs(playerIndex));
			}

			public float Height {
				get { return Font.LineSpacing; }
			}

			public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, float alpha) {
				Vector2 offset = new Vector2(36, 0);
				Vector2 imageOffset = new Vector2(0, 12);
				if (IsSelected) {
					if (Selected != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.A] : GameSession.Current.KeyImages["X"], new Rectangle((int)origin.X + (int)imageOffset.X, (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
					if (InputState.InputMethod == InputMethods.KeyboardMouse) { // TODO: Remove this once we get images for the gamepad left and right buttons
						if (SwipeLeft != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.DPadLeft] : GameSession.Current.KeyImages["Left"], new Rectangle((int)origin.X + (int)imageOffset.X, (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
						if (SwipeRight != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.DPadRight] : GameSession.Current.KeyImages["Right"], new Rectangle((int)origin.X + (int)imageOffset.X + 8 + 32 + (int)GenericMenu.Font.MeasureString(Text).X, (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
					}
				}
				spriteBatch.DrawStringOutlined(GenericMenu.Font, Text, origin + offset, Enabled ? (IsSelected ? Color.White : new Color(192, 192, 192)) : new Color(96, 96, 96) * alpha);
			}

			public Entry(string text) {
				Text = text;
			}
		}

		public class HeadingEntry : Entry {
			float scale = 0.7f;
			public HeadingEntry(string text) : base(text) {
				selectable = false;
			}
			public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, float alpha) {
				spriteBatch.DrawStringOutlined(GenericMenu.Font, Text, origin, Color.Gray * alpha, Color.Black, 0f, Vector2.Zero, scale);
			}

			public new float Height {
				get { return (float)Font.LineSpacing * scale; }
			}
		}
	}
}
