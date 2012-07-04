using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

namespace LootShop {
	public class LootShop : Microsoft.Xna.Framework.Game {
		GraphicsDeviceManager graphics;
		ScreenManager screenManager;

		static readonly string[] preloadAssets = {
        };

		public LootShop() {
			Content.RootDirectory = "Content";

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;

			screenManager = new ScreenManager(this);

			Components.Add(screenManager);

			//screenManager.AddScreen(new GameplayScreen(), PlayerIndex.One); // TODO: make PlayerIndex null so the main menu can determine the player or whatever
			screenManager.AddScreen(new BackgroundScreen(), null);
			screenManager.AddScreen(new MainMenuScreen(), null);

		}

		protected override void Initialize() {
			base.Initialize();
		}

		protected override void LoadContent() {
			foreach (string asset in preloadAssets) {
				Content.Load<object>(asset);
			}
		}

		protected override void UnloadContent() {
		}

		protected override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			graphics.GraphicsDevice.Clear(Color.Black);

			base.Draw(gameTime);
		}
	}
}
