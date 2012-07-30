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
		public SpriteFont UIFontLarge;
		public SpriteFont KeyboardFont;
		public SpriteFont DialogueFont;
		public Texture2D Pixel;
		public Texture2D TestBackground;
		public Texture2D Border;
		public Texture2D Cursor;
		public Song TitleTheme;
		public Song ShopTheme;
		public Song TownTheme;
		public Song IntroTheme;
		public SoundEffect MenuAccept;
		public SoundEffect MenuCursor;
		public SoundEffect MenuCancel;
		public SoundEffect PressStart;
		public Dictionary<Buttons, Texture2D> ButtonImages = new Dictionary<Buttons, Texture2D>();
		public Dictionary<string, Texture2D> KeyImages = new Dictionary<string, Texture2D>();
		public Campaign Campaign;
		public static GameSession Current = null;
		public static Random Random = new Random();
		ScreenManager screenManager;
		FrameRateCounter fpsCounter;

		static readonly string[] preloadAssets = {
        };

		public GameSession() {
			Current = this;
			Content.RootDirectory = "Content";

			Item.Initialize();
			Cutscene.LoadFromFile();

			screenManager = new ScreenManager(this);
			fpsCounter = new FrameRateCounter(this);

			graphics = new GraphicsDeviceManager(this);
			Resolution.Init(ref graphics);
#if XBOX
			Resolution.SetResolution(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height, true);
#else
			Resolution.SetResolution(1280, 720, false);
#endif
			Resolution.SetVirtualResolution(1280, 720);

			Components.Add(screenManager);
			//Components.Add(fpsCounter);
			float totalTime = 8.45f;
			float firstTime = 3.85f;
			float secondTime = totalTime - firstTime;
			//screenManager.AddScreen(new ScreenProxy(new SplashScreen("108 Studios presents", TimeSpan.FromSeconds(firstTime)), new ScreenProxy(new SplashScreen("[second title card goes here]", TimeSpan.FromSeconds(secondTime)), new TitleScreen())), null);
			LoadingScreen.Load(screenManager, true, PlayerIndex.One, new DungeonScreen());
		}

		protected override void Initialize() {
			base.Initialize();
			MediaPlayer.Play(TitleTheme);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0;
		}

		protected override void LoadContent() {
			foreach (string asset in preloadAssets) {
				Content.Load<object>(asset);
			}
			UIFontSmall = Content.Load<SpriteFont>("UIFontSmall");
			UIFontMedium = Content.Load<SpriteFont>("UIFontMedium");
			UIFontLarge = Content.Load<SpriteFont>("UIFontLarge");
			KeyboardFont = Content.Load<SpriteFont>("KeyboardFont");
			DialogueFont = Content.Load<SpriteFont>("DialogueFont");
			Pixel = Content.Load<Texture2D>("blank");
			TestBackground = Content.Load<Texture2D>("testbackground");
			Border = Content.Load<Texture2D>("stonebackground");
			Cursor = Content.Load<Texture2D>("cursor");

			TitleTheme = Content.Load<Song>(@"Music\Loot Shop 1a");
			ShopTheme = Content.Load<Song>(@"Music\lootshop 6a medieval bossa");
			TownTheme = Content.Load<Song>(@"Music\Loot Shop 3a");
			IntroTheme = Content.Load<Song>(@"Music\Loot Shop 2a");

			MenuAccept = Content.Load<SoundEffect>(@"Sounds\menuAccept");
			MenuCursor = Content.Load<SoundEffect>(@"Sounds\menuCursor");
			MenuCancel = Content.Load<SoundEffect>(@"Sounds\menuCancel");
			PressStart = Content.Load<SoundEffect>(@"Sounds\pressStart");

			KeyImages.Add("X", Content.Load<Texture2D>(@"KeyImages\keyboardX"));
			KeyImages.Add("Z", Content.Load<Texture2D>(@"KeyImages\keyboardZ"));
			KeyImages.Add("Esc", Content.Load<Texture2D>(@"KeyImages\keyboardEsc"));
			KeyImages.Add("Up", Content.Load<Texture2D>(@"KeyImages\keyboardUp"));
			KeyImages.Add("Down", Content.Load<Texture2D>(@"KeyImages\keyboardDown"));
			KeyImages.Add("Left", Content.Load<Texture2D>(@"KeyImages\keyboardLeft"));
			KeyImages.Add("Right", Content.Load<Texture2D>(@"KeyImages\keyboardRight"));

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
			base.Draw(gameTime);
		}
	}
}
