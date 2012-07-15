﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	public class ScreenManager : DrawableGameComponent {
		List<GameScreen> screens = new List<GameScreen>();
		InputState input = new InputState();

		public ScreenManager(GameSession game)
			: base(game) {
		}

		public SpriteBatch SpriteBatch {
			get { return spriteBatch; }
		}
		SpriteBatch spriteBatch;

		public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
			screen.ControllingPlayer = controllingPlayer;
			screen.ScreenManager = this;
			screens.Add(screen);
			screen.Initialize();
			screen.LoadContent(); // TODO: Move to loading screen
		}

		public void RemoveScreen() {
			RemoveScreen(screens[screens.Count - 1]);
		}

		public void RemoveScreen(GameScreen screen) {
			screen.UnloadContent();
			screens.Remove(screen);
		}

		public void ReplaceScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
			if (screens.Count > 0) RemoveScreen(screens[screens.Count - 1]);
			AddScreen(screen, controllingPlayer);
		}

		public void ReplaceAllScreens(GameScreen screen, PlayerIndex? controllingPlayer) {
			ClearScreens();
			AddScreen(screen, controllingPlayer);
		}

		public void ReplaceScreenProxy(GameScreen now, GameScreen after, PlayerIndex? controllingPlayer) {
			ReplaceScreen(new ScreenProxy(now, after), controllingPlayer);
		}

		public void BackToTitle(PlayerIndex? controllingPlayer) {
			ClearScreens();
			AddScreen(new TitleScreen(), controllingPlayer);
			AddScreen(new MainMenu(), null);
		}

		public void ClearScreens() {
			screens.Clear();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		public override void Update(GameTime gameTime) {
			input.Update();
			screens[screens.Count - 1].HandleInput(input);
			screens[screens.Count - 1].Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			foreach (GameScreen screen in screens) {
				screen.Draw(gameTime);
			}
			SpriteBatch.Begin();
			Vector2 origin = new Vector2(GraphicsDevice.Viewport.Width - 4, 4);
			foreach (GameScreen screen in screens) {
				string text = screen.GetType().ToString().Replace("LootShop.", "");
				SpriteBatch.DrawString(GameSession.Current.UIFontSmall, text, origin, Color.Yellow, 0f, new Vector2(GameSession.Current.UIFontSmall.MeasureString(text).X, 0), 1f, SpriteEffects.None, 1f);
				//DrawText(spriteBatch, GameSession.Current.UIFontSmall, screen.GetType().ToString(), Color.Black, Color.Yellow, 1f, 0f, origin);
				origin.Y += GameSession.Current.UIFontSmall.LineSpacing;
			}
			SpriteBatch.End();
		}

		private void DrawText(SpriteBatch spriteBatch, SpriteFont font, string text, Color backColor, Color frontColor, float scale, float rotation, Vector2 position) {
			Vector2 origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);

			spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, 1 * scale), backColor, rotation, origin, scale, SpriteEffects.None, 1f);

			spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, -1 * scale), backColor, rotation, origin, scale, SpriteEffects.None, 1f);
			spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, 1 * scale), backColor, rotation, origin, scale, SpriteEffects.None, 1f);

			spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, -1 * scale), backColor, rotation, origin, scale, SpriteEffects.None, 1f);
			spriteBatch.DrawString(font, text, position, frontColor, rotation, origin, scale, SpriteEffects.None, 1f);

		}
	}
}
