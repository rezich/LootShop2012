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
	public class GameSession : Microsoft.Xna.Framework.Game {

		GraphicsDeviceManager graphics;
		public SpriteFont UIFontSmall;
		public SpriteFont UIFontMedium;
		public SpriteFont DialogueFont;
		public Texture2D Pixel;
		public Texture2D TestBackground;
		public Texture2D Border;
		public Dictionary<Buttons, Texture2D> ButtonImages = new Dictionary<Buttons, Texture2D>();
		public Campaign Campaign;
		public static GameSession Current = null;
		public static Random Random = new Random();
		ScreenManager screenManager;

		static readonly string[] preloadAssets = {
        };

		public GameSession() {
			Current = this;
			Content.RootDirectory = "Content";

			Item.Initialize();

			screenManager = new ScreenManager(this);

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;

			Components.Add(screenManager);

			screenManager.AddScreen(new TitleScreen(), null);
		}

		protected override void Initialize() {
			base.Initialize();
		}

		protected override void LoadContent() {
			foreach (string asset in preloadAssets) {
				Content.Load<object>(asset);
			}
			UIFontSmall = Content.Load<SpriteFont>("UIFontSmall");
			UIFontMedium = Content.Load<SpriteFont>("UIFontMedium");
			DialogueFont = Content.Load<SpriteFont>("DialogueFont");
			Pixel = Content.Load<Texture2D>("blank");
			TestBackground = Content.Load<Texture2D>("testbackground");
			Border = Content.Load<Texture2D>("border");
			ButtonImages.Add(Buttons.Back, Content.Load<Texture2D>(@"ButtonImages\xboxControllerBack"));
			ButtonImages.Add(Buttons.A, Content.Load<Texture2D>(@"ButtonImages\xboxControllerButtonA"));
			ButtonImages.Add(Buttons.B, Content.Load<Texture2D>(@"ButtonImages\xboxControllerButtonB"));
			ButtonImages.Add(Buttons.X, Content.Load<Texture2D>(@"ButtonImages\xboxControllerButtonX"));
			ButtonImages.Add(Buttons.Y, Content.Load<Texture2D>(@"ButtonImages\xboxControllerButtonY"));
			ButtonImages.Add(Buttons.LeftShoulder, Content.Load<Texture2D>(@"ButtonImages\xboxControllerLeftShoulder"));
			ButtonImages.Add(Buttons.LeftTrigger, Content.Load<Texture2D>(@"ButtonImages\xboxControllerLeftTrigger"));
			ButtonImages.Add(Buttons.RightShoulder, Content.Load<Texture2D>(@"ButtonImages\xboxControllerRightShoulder"));
			ButtonImages.Add(Buttons.RightTrigger, Content.Load<Texture2D>(@"ButtonImages\xboxControllerRightTrigger"));
			ButtonImages.Add(Buttons.Start, Content.Load<Texture2D>(@"ButtonImages\xboxControllerStart"));
		}

		protected override void UnloadContent() {
		}

		protected override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			graphics.GraphicsDevice.Clear(new Color(0.05f, 0.05f, 0.05f));

			base.Draw(gameTime);
		}
	}
}
