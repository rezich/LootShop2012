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

		public ScreenManager(Game game)
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

		public void RemoveScreen(GameScreen screen) {
			screen.UnloadContent();
			screens.Remove(screen);
		}

		public void ReplaceScreen(GameScreen screen, PlayerIndex? controllingPlayer) {
			if (screens.Count > 0) RemoveScreen(screens[screens.Count - 1]);
			AddScreen(screen, controllingPlayer);
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
		}
	}
}
