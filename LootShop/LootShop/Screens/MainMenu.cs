using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LootShop {
	public class MainMenu : GenericMenu {
		Texture2D background;
		ContentManager content;

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");
			background = content.Load<Texture2D>("title");
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.Draw(background, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}

		public MainMenu()
			: base(null, true, false) {
				DimBackground = false;
				
				Entry entryNewGame = new Entry("New Game");
				entryNewGame.Selected += NewGame;
				Entry entryContinue = new Entry("Continue", false);
				Entry entryLoadGame = new Entry("Load Game", false);
				Entry entryOptions = new Entry("Options", false);
				Entry entryLootTest = new Entry("Loot Test");
				entryLootTest.Selected += StartNewGame;
				Entry entryExit = new Entry("Quit");
				entryExit.Selected += QuitGame;

				MenuEntries.Add(entryNewGame);
				MenuEntries.Add(entryContinue);
				MenuEntries.Add(entryLoadGame);
				MenuEntries.Add(entryOptions);
				MenuEntries.Add(entryLootTest);
				MenuEntries.Add(entryExit);
		}

		void StartNewGame(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ClearScreens();
			ScreenManager.ReplaceAllScreens(new TestScreen(), ControllingPlayer);
		}

		void QuitGame(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.Exit();
		}

		void NewGame(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ClearScreens();
			MediaPlayer.Play(GameSession.Current.IntroTheme);
			ScreenManager.ReplaceScreenProxy(new CutsceneScreen("Opening"), new GameplayScreen(), ControllingPlayer);
			//ScreenManager.ReplaceAllScreens(new GameplayScreen(), ControllingPlayer);
		}
	}
}
