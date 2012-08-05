using System;
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
				input.Update();
#if XBOX
				InputState.InputMethod = InputMethods.Gamepad;
#else
				InputState.InputMethod = InputMethods.KeyboardMouse;
#endif
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
			screens[index].UnloadContent();
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

		public void BeginSpriteBatch() {
			SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Resolution.getTransformationMatrix());
		}

		public void Begin3D() {
		}

		public void End3D() {
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
			//bool coveredByOtherScreen = false;

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

			Resolution.FullViewport();

			SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
			SpriteBatch.Draw(GameSession.Current.Border, new Vector2(0, 0), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
			SpriteBatch.End();

			Resolution.BeginDraw();

			BeginSpriteBatch();
			SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Black);
			SpriteBatch.End();

			foreach (GameScreen screen in screens) {
				if (screen.ScreenState == ScreenState.Hidden) continue;
				screen.Draw(gameTime);
			}

			BeginSpriteBatch();
			Vector2 origin = new Vector2(Resolution.Right - 4, Resolution.Top + 4);
			foreach (GameScreen screen in screens) {
				string text = screen.GetType().ToString().Replace("LootShop.", "");
				SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, text, origin, Color.Yellow, Color.Black, 0f, new Vector2(GameSession.Current.UIFontSmall.MeasureString(text).X, 0), 1f);
				origin.Y += GameSession.Current.UIFontSmall.LineSpacing;
			}
			SpriteBatch.End();

			/*spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
			SpriteBatch.Draw(GameSession.Current.Border, new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Height)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			SpriteBatch.Draw(GameSession.Current.Border, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Y), RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			SpriteBatch.Draw(GameSession.Current.Border, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Y), RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.TitleSafeArea.Top)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			SpriteBatch.Draw(GameSession.Current.Border, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Bottom), RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Bottom), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Height)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
			SpriteBatch.End();*/
		}
	}
}
