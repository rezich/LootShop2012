using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	class LoadingScreen : GameScreen {
		bool loadingIsSlow;
		bool otherScreensAreGone;
		GameScreen[] screensToLoad;

		private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad) {
			this.loadingIsSlow = loadingIsSlow;
			this.screensToLoad = screensToLoad;
		}

		public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? controllingPlayer, params GameScreen[] screensToLoad) {
			foreach (GameScreen screen in screenManager.GetScreens()) screen.ExitScreen();
			LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);
			screenManager.AddScreen(loadingScreen, controllingPlayer);
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if (otherScreensAreGone) {
				ScreenManager.RemoveScreen(this);

				foreach (GameScreen screen in screensToLoad) {
					if (screen != null) {
						ScreenManager.AddScreen(screen, ControllingPlayer);
					}
				}

				ScreenManager.Game.ResetElapsedTime();
			}
		}

		public override void Draw(GameTime gameTime) {
			if ((ScreenState == ScreenState.Active) && (ScreenManager.GetScreens().Length == 1)) {
				otherScreensAreGone = true;
			}

			if (loadingIsSlow) {
				SpriteFont font = GameSession.Current.UIFontMedium;

				const string message = "Loading...";

				// Center the text in the viewport.
				Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
				Vector2 viewportSize = new Vector2(Resolution.Right, Resolution.Bottom);
				Vector2 textSize = font.MeasureString(message);
				Vector2 textPosition = (viewportSize - textSize) / 2;

				Color color = Color.White;

				// Draw the text.
				ScreenManager.BeginSpriteBatch();
				ScreenManager.SpriteBatch.DrawStringOutlined(font, message, textPosition, color);
				ScreenManager.SpriteBatch.End();
			}
		}
	}
}
