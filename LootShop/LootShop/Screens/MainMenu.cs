using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

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
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(background, Resolution.Rectangle, Color.White * TransitionAlpha);
			ScreenManager.SpriteBatch.End();
			base.Draw(gameTime);
		}

		public MainMenu()
			: base(null, true, false) {
				DimBackground = false;
				ShowCancel = false;
				
				Entry entryNewGame = new Entry("New Game");
				entryNewGame.Selected += NewGame;
				Entry entryContinue = new Entry("Continue");
				entryContinue.Enabled = false;
				Entry entryLoadGame = new Entry("Load Game");
				entryLoadGame.Enabled = false;
				Entry entryOptions = new Entry("Options");
				entryOptions.Selected += Options;
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
			GameSession.Current.Campaign = new Campaign();
		}

		void Options(object sender, PlayerIndexEventArgs e) {
			ScreenManager.AddScreen(new OptionsScreen(), ControllingPlayer);
		}

		void QuitGame(object sender, PlayerIndexEventArgs e) {
			GameSession.Current.Exit();
		}

		void NewGame(object sender, PlayerIndexEventArgs e) {
			//ScreenManager.ClearScreens();
			LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
		}

		protected override void OnCancel(PlayerIndex playerIndex) {
			ExitScreen();
			ScreenManager.AddScreen(new TitleScreen(), null);
		}
	}
}
