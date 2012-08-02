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
	class DungeonScreen : GameScreen {
		ContentManager content;
		Actor player;
		Stage stage;
		Song song;

		public DungeonScreen() {
			stage = new Stage();
			player = new Actor(stage, new Vector2(25, 100));
			stage.Objects.Add(player);
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					stage.Objects.Add(new Terrain(stage, new Vector2(j, i), false));
				}
			}
			//stage.Objects.Remove(stage.Objects.Find(x => x.TilePosition.X == 1 && x.TilePosition.Y == 1));
			stage.Objects.Add(new Terrain(stage, new Vector2(1, 1), true));
			//stage.Objects.Remove(stage.Objects.Find(x => x.TilePosition.X == 1 && x.TilePosition.Y == 2));
			stage.Objects.Add(new Terrain(stage, new Vector2(1, 2), true));
			//stage.Objects.Remove(stage.Objects.Find(x => x.TilePosition.X == 2 && x.TilePosition.Y == 1));
			stage.Objects.Add(new Terrain(stage, new Vector2(2, 1), true));

			stage.IntendedViewOffset = new Vector2(400, 400);
			stage.ViewOffset = stage.IntendedViewOffset;
		}

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");
			stage.Textures.Add("tileTest", content.Load<Texture2D>(@"Tiles\tileTest"));
			stage.Textures.Add("tileCube", content.Load<Texture2D>(@"Tiles\tileCube"));
			stage.Textures.Add("man", content.Load<Texture2D>(@"Tiles\man"));
			song = content.Load<Song>(@"Music\Thrilling");
			MediaPlayer.Play(song);
			ScreenManager.Game.ResetElapsedTime();
		}
		public override void UnloadContent() {
			content.Unload();
		}

		public override void Initialize() {
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			stage.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Gray);

			stage.Draw(ScreenManager.SpriteBatch);

			if (InputState.InputMethod == InputMethods.KeyboardMouse) ScreenManager.SpriteBatch.Draw(GameSession.Current.Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

			int itemOffset = 0;
			foreach (LootSystem.Item i in GameSession.Current.Campaign.PlayerCharacter.Inventory) {
				ScreenManager.SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, i.Name + " " + i.Variety.Slot.ToString(), new Vector2(0, itemOffset), Color.White);
				itemOffset += GameSession.Current.UIFontSmall.LineSpacing;
			}

			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			if (input.IsInput(Inputs.MenuUp, ControllingPlayer)) player.IntendedPosition.Y -= 8;
			if (input.IsInput(Inputs.MenuDown, ControllingPlayer)) player.IntendedPosition.Y += 8;
			if (input.IsInput(Inputs.MenuLeft, ControllingPlayer)) player.IntendedPosition.X -= 8;
			if (input.IsInput(Inputs.MenuRight, ControllingPlayer)) player.IntendedPosition.X += 8;
			if (input.IsInput(Inputs.GamePause, ControllingPlayer)) ScreenManager.AddScreen(new PauseScreen(), ControllingPlayer);

			player.IntendedPosition += input.LeftThumbstick(ControllingPlayer) * 4;
			stage.IntendedViewOffset += input.RightThumbstick(ControllingPlayer) * new Vector2(-1, -1) * 8;

			if (input.CurrentMouseState.LeftButton == ButtonState.Pressed) player.IntendedPosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y) - stage.ViewOffset;
		}
	}
}
