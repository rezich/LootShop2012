using System;
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
			/*SpriteBatch.Begin();
			SpriteBatch.Draw(GameSession.Current.TestBackground, VectorsToRect(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White);
			SpriteBatch.End();*/

			foreach (GameScreen screen in screens) {
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
			Color red = new Color(1f, 0f, 0f, 0.05f);
			SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Height)), red);
			SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), red);
			SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.Y), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.TitleSafeArea.Top)), red);
			SpriteBatch.Draw(GameSession.Current.Pixel, RectangleHelper.FromVectors(new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left, GraphicsDevice.Viewport.TitleSafeArea.Bottom), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Right, GraphicsDevice.Viewport.Height)), red);
			SpriteBatch.End();
		}
	}
}
