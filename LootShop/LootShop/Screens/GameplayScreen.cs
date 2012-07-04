using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	public class GameplayScreen : GameScreen {
		ContentManager content;
		Vector2 playerPosition = new Vector2(100, 100);

		public GameplayScreen() {
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");

			Thread.Sleep(1000);
			ScreenManager.Game.ResetElapsedTime();
		}

		public override void UnloadContent() {
			content.Unload();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void HandleInput(InputState input) {
			if (input == null)
				throw new ArgumentNullException("input");

			// Look up inputs for the active player profile.
			int playerIndex = (int)ControllingPlayer.Value;

			KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
			GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

			// The game pauses either if the user presses the pause button, or if
			// they unplug the active gamepad. This requires us to keep track of
			// whether a gamepad was ever plugged in, because we don't want to pause
			// on PC if they are playing with a keyboard and have no gamepad at all!
			bool gamePadDisconnected = !gamePadState.IsConnected &&
									   input.GamePadWasConnected[playerIndex];

			if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected) {
				//ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
			}
			else {
				// Otherwise move the player position.
				Vector2 movement = Vector2.Zero;

				if (keyboardState.IsKeyDown(Keys.Left))
					movement.X--;

				if (keyboardState.IsKeyDown(Keys.Right))
					movement.X++;

				if (keyboardState.IsKeyDown(Keys.Up))
					movement.Y--;

				if (keyboardState.IsKeyDown(Keys.Down))
					movement.Y++;

				Vector2 thumbstick = gamePadState.ThumbSticks.Left;

				movement.X += thumbstick.X;
				movement.Y -= thumbstick.Y;

				if (movement.Length() > 1)
					movement.Normalize();

				playerPosition += movement * 2;
			}
		}

		public override void Draw(GameTime gameTime) {
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

			spriteBatch.Begin();

			if (IsActive) {
				spriteBatch.DrawString(font, "TEST", playerPosition, Color.White);
			}

			spriteBatch.End();
		}
	}
}
