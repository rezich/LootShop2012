﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

namespace LootShop {
	public class ScreenManager : DrawableGameComponent {
		List<GameScreen> screens = new List<GameScreen>();
		List<GameScreen> screensToUpdate = new List<GameScreen>();
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
			//screens[screens.Count - 1].ExitScreen();
		}

		public void RemoveScreen(GameScreen screen) {
			screen.UnloadContent();
			screens.Remove(screen);
			//screen.ExitScreen();
		}

		public void RemoveScreenAt(int index) {
			screens.RemoveAt(index);
		}

		public void ReplaceScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
			//if (screens.Count > 0) RemoveScreen(screens[screens.Count - 1]);
			if (screens.Count > 0) screens[screens.Count - 1].ExitScreen();
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
			//AddScreen(new MainMenu(), null);
			MediaPlayer.Play(GameSession.Current.TitleTheme);
		}

		public void ClearScreens() {
			screens.Clear();
		}

		public GameScreen[] GetScreens() {
			return screens.ToArray();
		}

		public GameScreen LastNonExiting() {
			GameScreen screen = null;
			foreach (GameScreen s in screens) {
				if (!s.IsExiting) screen = s;
			}
			return screen;
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		public override void Update(GameTime gameTime) {
			input.Update();
			//screens[screens.Count - 1].HandleInput(input);
			//screens[screens.Count - 1].Update(gameTime);
			foreach (GameScreen screen in screens)
				screensToUpdate.Add(screen);

			bool otherScreenHasFocus = !Game.IsActive;
			bool coveredByOtherScreen = false;

			// Loop as long as there are screens waiting to be updated.
			while (screensToUpdate.Count > 0) {
				// Pop the topmost screen off the waiting list.
				GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

				screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

				// Update the screen.
				//screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
				screen.Update(gameTime);

				if (screen.ScreenState == ScreenState.TransitionOn ||
					screen.ScreenState == ScreenState.Active) {
					// If this is the first active screen we came across,
					// give it a chance to handle input.
					if (!otherScreenHasFocus) {
						screen.HandleInput(input);

						otherScreenHasFocus = true;
					}
				}
			}
		}

		public override void Draw(GameTime gameTime) {
			/*SpriteBatch.Begin();
			SpriteBatch.Draw(GameSession.Current.TestBackground, VectorsToRect(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White);
			SpriteBatch.End();*/

			foreach (GameScreen screen in screens) {
				if (screen.ScreenState == ScreenState.Hidden) continue;
				screen.Draw(gameTime);
			}

			SpriteBatch.Begin();

			//SpriteBatch.Draw(GameSession.Current.Border, new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

			Vector2 origin = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right - 4, GraphicsDevice.Viewport.TitleSafeArea.Top + 4);
			foreach (GameScreen screen in screens) {
				string text = screen.GetType().ToString().Replace("LootShop.", "");
				SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, text, origin, Color.Yellow, Color.Black, 0f, new Vector2(GameSession.Current.UIFontSmall.MeasureString(text).X, 0), 1f);
				origin.Y += GameSession.Current.UIFontSmall.LineSpacing;
			}
			Color red = new Color(1f, 0f, 0f, 0.001f);
			if (true) {
				SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Height)), red);
				SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), red);
				SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.TitleSafeArea.Top)), red);
				SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Bottom), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Height)), red);
			}
			SpriteBatch.End();
		}
	}
}
