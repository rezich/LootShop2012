using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
		public bool ShowCancel = true;
		public bool Centered = false;
		bool initialized = false;
		public TextBlock Content = null;
		private List<Entry> entries = new List<Entry>();

		public static int EntriesWidth = 500;

		public int Width {
			get {
				return EntriesWidth;
			}
		}

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
				GameSession.Current.MenuCursor.Play(GameSession.Current.SoundEffectVolume, 0, 0);
			}

			if (input.IsInput(Inputs.MenuUp, ControllingPlayer)) {
				do {
					selectedIndex--;
					if (selectedIndex < 0) selectedIndex += entries.Count;
				}
				while (!entries[selectedIndex].Enabled);
				GameSession.Current.MenuCursor.Play(GameSession.Current.SoundEffectVolume, 0, 0);
			}

			if (entries[selectedIndex].Content != null) Content = entries[selectedIndex].Content;
			else Content = null;

			PlayerIndex playerIndex;
			if (input.IsInput(Inputs.MenuAccept, ControllingPlayer, out playerIndex)) {
				if (entries[selectedIndex].IsCancel) GameSession.Current.MenuCancel.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				else if (entries[selectedIndex].HasSelected) GameSession.Current.MenuAccept.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				else GameSession.Current.MenuDeny.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				OnSelectEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuLeft, ControllingPlayer, out playerIndex)) {
				if (entries[selectedIndex].HasSwipeLeft) GameSession.Current.MenuCursor.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				OnSwipeLeftEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuRight, ControllingPlayer, out playerIndex)) {
				if (entries[selectedIndex].HasSwipeRight) GameSession.Current.MenuCursor.Play(GameSession.Current.SoundEffectVolume, 0, 0);
				OnSwipeRightEntry(selectedIndex, playerIndex);
			}
			if (input.IsInput(Inputs.MenuCancel, ControllingPlayer, out playerIndex) && Cancelable) {
				GameSession.Current.MenuCancel.Play(GameSession.Current.SoundEffectVolume, 0, 0);
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

			SpriteFont descriptionFont = GameSession.Current.UIFontSmall;
			SpriteFont backFont = GameSession.Current.UIFontSmall;

			int menuOffsetX = (!HasContent && Centered ? Resolution.Right / 2 - Width / 2 : 0);

			Vector2 entriesOrigin = new Vector2(left + margin + menuOffsetX - (Width) * (Centered ? 0 : TransitionPositionSquared), (Resolution.Bottom / 2) - (entriesHeight / 2));
			Rectangle descriptionRect = RectangleHelper.FromVectors(new Vector2(left + Width + margin * 2, bottom - descriptionFont.LineSpacing - margin - padding * 2), new Vector2(right - margin, bottom - margin));
			//descriptionRect.Height = Convert.ToInt32((float)descriptionRect.Height * (1 - TransitionPositionSquared));
			Rectangle cancelRect = RectangleHelper.FromVectors(new Vector2(left + margin + menuOffsetX, bottom - backFont.LineSpacing - margin - padding * 2), new Vector2(left + Width + margin, bottom - margin));
			Vector2 titleOrigin = new Vector2(0, top + margin);
			Rectangle contentRect = RectangleHelper.FromVectors(new Vector2(left + Width + margin * 2, titleOrigin.Y + Font.LineSpacing + margin), new Vector2(right - margin, Description == null ? bottom - margin : descriptionRect.Top - margin));
			contentRect.Height = Convert.ToInt32((float)contentRect.Height * (1 - TransitionPositionSquared));
			titleOrigin.X = (Centered ? Resolution.Right / 2 : contentRect.Center.X);
			if (Centered) cancelRect.X = Resolution.Right / 2 - cancelRect.Width / 2;

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

			if (Cancelable && ShowCancel) {
				ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, cancelRect, new Color(0.35f, 0.35f, 0.35f) * TransitionAlpha);
				TextBlock backBlock = new TextBlock("#MENU_CANCEL# Back");
				backBlock.Draw(ScreenManager.SpriteBatch, backFont, new Vector2(cancelRect.X + padding, cancelRect.Y + padding), TextBlock.TextAlign.Left, null, TransitionAlpha);
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
			public string Text2;
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
			public bool IsCancel = false;
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

			public bool HasSelected { get { return Selected != null; } }
			public bool HasSwipeLeft { get { return SwipeLeft != null; } }
			public bool HasSwipeRight { get { return SwipeRight != null; } }

			public virtual float Height {
				get { return Font.LineSpacing; }
			}

			public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 origin, float alpha) {
				Vector2 offset = new Vector2(36, 0);
				Vector2 imageOffset = new Vector2(0, 12);
				if (IsSelected) {
					if (Selected != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.A] : GameSession.Current.KeyImages["X"], new Rectangle((int)origin.X + (int)imageOffset.X, (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
					if (InputState.InputMethod == InputMethods.KeyboardMouse) { // TODO: Remove this once we get images for the gamepad left and right buttons
						if (SwipeLeft != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.DPadLeft] : GameSession.Current.KeyImages["Left"], new Rectangle((int)origin.X + (int)imageOffset.X, (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
						if (SwipeRight != null) spriteBatch.Draw(InputState.InputMethod == InputMethods.Gamepad ? GameSession.Current.ButtonImages[Buttons.DPadRight] : GameSession.Current.KeyImages["Right"], new Rectangle((int)origin.X + (Text2 == null ? (int)imageOffset.X + (int)offset.X + 4 + (int)GenericMenu.Font.MeasureString(Text).X : GenericMenu.EntriesWidth - (int)imageOffset.X - 32), (int)origin.Y + (int)imageOffset.Y, 32, 32), Color.White);
					}
				}
				spriteBatch.DrawStringOutlined(GenericMenu.Font, Text, origin + offset, Enabled ? (IsSelected ? Color.White : new Color(192, 192, 192)) : new Color(96, 96, 96) * alpha);
				if (Text2 != null) spriteBatch.DrawStringOutlined(GenericMenu.Font, Text2, origin - offset + new Vector2(GenericMenu.EntriesWidth - GenericMenu.Font.MeasureString(Text2).X, 0), Enabled ? (IsSelected ? Color.White : new Color(192, 192, 192)) : new Color(96, 96, 96) * alpha);
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
				//int width = (int)(GenericMenu.Font.MeasureString(Text).X * scale);
				int width = GenericMenu.EntriesWidth;
				spriteBatch.Draw(GameSession.Current.Pixel, new Rectangle((int)origin.X, (int)origin.Y + (int)(GenericMenu.Font.MeasureString(Text).Y * scale) - 6, width, 2), Color.Gray);
				spriteBatch.DrawStringOutlined(GenericMenu.Font, Text, origin, Color.Gray * alpha, Color.Black, 0f, Vector2.Zero, scale);
			}

			public override float Height {
				get { return (float)Font.LineSpacing * scale; }
			}
		}

		public class SpacerEntry : Entry {
			public SpacerEntry()
				: base("") {
					Enabled = false;
			}
		}
	}
}
