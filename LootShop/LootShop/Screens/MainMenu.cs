using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
				Entry LootTest = new Entry("Loot Test");
				LootTest.Selected += StartNewGame;
				MenuEntries.Add(new Entry("New Game"));
				MenuEntries.Add(new Entry("Continue"));
				MenuEntries.Add(new Entry("Load Game"));
				MenuEntries.Add(new Entry("Options"));
				MenuEntries.Add(LootTest);
				Entry itemExit = new Entry("Quit");
				itemExit.Selected += QuitGame;
				MenuEntries.Add(itemExit);
		}

		void StartNewGame(object sender, PlayerIndexEventArgs e) {
			ScreenManager.ClearScreens();
			ScreenManager.AddScreen(new TestScreen(), ControllingPlayer);
		}

		void QuitGame(object sender, PlayerIndexEventArgs e) {
			LootShop.CurrentGame.Exit();
		}
	}
}
