#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;
#endregion

namespace LootShop {
	/// <summary>
	/// This screen implements the actual game logic. It is just a
	/// placeholder to get the idea across: you'll probably want to
	/// put some more interesting gameplay in here!
	/// </summary>
	class GameplayScreen : GameScreen {
		#region Fields

		ContentManager content;
		SpriteFont gameFont;

		SpriteFont UIFontSmall;

		Vector2 playerPosition = new Vector2(100, 100);
		Vector2 enemyPosition = new Vector2(100, 100);

		Random random = new Random();

		GenericMenu testMenu = new GenericMenu();

		float pauseAlpha;

		Item item;

		#endregion

		#region Initialization


		/// <summary>
		/// Constructor.
		/// </summary>
		public GameplayScreen() {
			TransitionOnTime = TimeSpan.FromSeconds(1.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			testMenu.Title = "Test Menu!";
			testMenu.Description = "This is just a test menu to see the generic menu system in action.";
		}


		/// <summary>
		/// Load graphics content for the game.
		/// </summary>
		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");

			gameFont = content.Load<SpriteFont>("menufont");
			UIFontSmall = content.Load<SpriteFont>("UIFontSmall");

			// A real game would probably have more content than this sample, so
			// it would take longer to load. We simulate that by delaying for a
			// while, giving you a chance to admire the beautiful loading screen.
			Thread.Sleep(1000);

			// once the load has finished, we use ResetElapsedTime to tell the game's
			// timing mechanism that we have just finished a very long frame, and that
			// it should not try to catch up.
			ScreenManager.Game.ResetElapsedTime();

			Item.Initialize();
			item = Item.Generate(random.Next(1, 50), random);
		}


		/// <summary>
		/// Unload graphics content used by the game.
		/// </summary>
		public override void UnloadContent() {
			content.Unload();
		}


		#endregion

		#region Update and Draw


		/// <summary>
		/// Updates the state of the game. This method checks the GameScreen.IsActive
		/// property, so the game will stop updating when the pause menu is active,
		/// or if you tab away to a different application.
		/// </summary>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen) {
			base.Update(gameTime, otherScreenHasFocus, false);

			// Gradually fade in or out depending on whether we are covered by the pause screen.
			if (coveredByOtherScreen)
				pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
			else
				pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

			if (IsActive) {
				// Apply some random jitter to make the enemy move around.
				const float randomization = 10;

				enemyPosition.X += (float)(random.NextDouble() - 0.5) * randomization;
				enemyPosition.Y += (float)(random.NextDouble() - 0.5) * randomization;

				// Apply a stabilizing force to stop the enemy moving off the screen.
				Vector2 targetPosition = new Vector2(
					ScreenManager.GraphicsDevice.Viewport.Width / 2 - gameFont.MeasureString("Insert Gameplay Here").X / 2,
					200);

				enemyPosition = Vector2.Lerp(enemyPosition, targetPosition, 0.05f);

				testMenu.Update(gameTime);

				// TODO: this game isn't very fun! You could probably improve
				// it by inserting something more interesting in this space :-)
			}
		}


		/// <summary>
		/// Lets the game respond to player input. Unlike the Update method,
		/// this will only be called when the gameplay screen is active.
		/// </summary>
		public override void HandleInput(InputState input) {
			if (input == null)
				throw new ArgumentNullException("input");

			// Look up inputs for the active player profile.
			int playerIndex = (int)ControllingPlayer.Value;

			KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
			GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
			GamePadState lastGamePadState = input.LastGamePadStates[playerIndex];

			// The game pauses either if the user presses the pause button, or if
			// they unplug the active gamepad. This requires us to keep track of
			// whether a gamepad was ever plugged in, because we don't want to pause
			// on PC if they are playing with a keyboard and have no gamepad at all!
			bool gamePadDisconnected = !gamePadState.IsConnected &&
									   input.GamePadWasConnected[playerIndex];

			if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected) {
				ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
			}
			else {
				testMenu.HandleInput(input, playerIndex);
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

				PlayerIndex bleh = new PlayerIndex();

				if (input.IsMenuSelect((PlayerIndex)playerIndex, out bleh)) item = Item.Generate(random.Next(1, 50), random);

				movement.X += thumbstick.X;
				movement.Y -= thumbstick.Y;

				if (movement.Length() > 1)
					movement.Normalize();

				playerPosition += movement * 2;
			}
		}


		/// <summary>
		/// Draws the gameplay screen.
		/// </summary>
		public override void Draw(GameTime gameTime) {
			// This game has a blue background. Why? Because!
			ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
											   Color.Black, 0, 0);

			// Our player and enemy are both actually just text strings.
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

			spriteBatch.Begin();

			#region Loot Test
			Color color;

			switch (item.Rarity.Name) {
				case Item.RarityLevel.Type.Garbage:
					color = Color.Gray;
					break;
				case Item.RarityLevel.Type.Normal:
					color = Color.White;
					break;
				case Item.RarityLevel.Type.Magic:
					color = Color.DarkCyan;
					break;
				case Item.RarityLevel.Type.Rare:
					color = Color.Yellow;
					break;
				case Item.RarityLevel.Type.Legendary:
					color = Color.DarkViolet;
					break;
				case Item.RarityLevel.Type.Unique:
					color = Color.Violet;
					break;
				default:
					color = Color.Red;
					break;
			}
			Vector2 origin = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, 100);

			SpriteFont lootFont = UIFontSmall;

			int width = 285;

			origin.X -= width / 2;

			int lineHeight = Convert.ToInt32(lootFont.MeasureString("W").Y * 0.8);
			int padding = 35;
			int line = 0;
			TextBlock name = new TextBlock(item.Name.ToUpper());
			List<string> nameList = name.WrappedText(lootFont, width);
			foreach (string n in nameList) {
				spriteBatch.DrawString(lootFont, n, origin + new Vector2(width / 2, line * lineHeight), color, 0.0f, new Vector2(lootFont.MeasureString(n).X, 0) / 2, 1.0f, SpriteEffects.None, 1.0f);
				line++;
			}
			line++;

			string type = (item.Rarity.Name == Item.RarityLevel.Type.Normal ? "" : item.Rarity.ToString() + " ") + item.Variety.Name;
			spriteBatch.DrawString(lootFont, type, origin + new Vector2(0, line * lineHeight), color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
			spriteBatch.DrawString(lootFont, item.Variety.Slot.ToString().DeCamelCase(), origin + new Vector2(width, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(item.Variety.Slot.ToString().DeCamelCase()).X, 0), 1.0f, SpriteEffects.None, 1.0f);
			line += 2;
			foreach (KeyValuePair<Item.Attribute.Type, double> kvp in item.Attributes) {
				string key = kvp.Key.ToString().DeCamelCase();
				string num = ((Item.Attribute.Lookup(kvp.Key).Addition ? "+" : "") + kvp.Value.ToString() + (Item.Attribute.Lookup(kvp.Key).Percentage ? "%" : ""));
				spriteBatch.DrawString(lootFont, key, origin + new Vector2(padding, line * lineHeight), Color.White);
				spriteBatch.DrawString(lootFont, num, origin + new Vector2(width - padding, line * lineHeight), Color.White, 0.0f, new Vector2(lootFont.MeasureString(num).X, 0), 1.0f, SpriteEffects.None, 1.0f);
				line++;
			}
			line++;
			string reqLevel = "Required Level: " + item.Level;
			spriteBatch.DrawString(lootFont, reqLevel, origin + new Vector2(width / 2, line * lineHeight), Color.Gray, 0.0f, new Vector2(lootFont.MeasureString(reqLevel).X / 2, 0), 1.0f, SpriteEffects.None, 1.0f);
			
			#endregion

			testMenu.Draw(spriteBatch, gameTime);

			spriteBatch.End();

			// If the game is transitioning on or off, fade it out to black.
			if (TransitionPosition > 0 || pauseAlpha > 0) {
				float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

				ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}


		#endregion
	}
}
