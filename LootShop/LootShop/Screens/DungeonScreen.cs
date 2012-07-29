using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	class DungeonScreen : GameScreen {
		ContentManager content;
		Texture2D tileTest;
		Texture2D tileCube;
		Texture2D man;
		Actor player;
		List<StageObject> stageObjects = new List<StageObject> {
			new Terrain(new Vector2(0, 0), true),
			new Terrain(new Vector2(1, 0), false),
			new Terrain(new Vector2(3, 0), true)
		};

		public DungeonScreen() {
			player = new Actor(new Vector2(3, 0));
			stageObjects.Add(player);
		}

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");
			tileTest = content.Load<Texture2D>(@"Tiles\tileTest");
			tileCube = content.Load<Texture2D>(@"Tiles\tileCube");
			man = content.Load<Texture2D>(@"Tiles\man");
			ScreenManager.Game.ResetElapsedTime();
		}
		public override void UnloadContent() {
			content.Unload();
		}

		public override void Draw(GameTime gameTime) {
			Vector2 offset = new Vector2(0, Resolution.Bottom / 2);
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Gray);
			stageObjects = stageObjects.OrderBy(t => (t.Position.Y * StageObject.TileHeight / 2) - (t.Position.X * StageObject.TileHeight / 2)).ToList();
			foreach (StageObject o in stageObjects) {
				/*if (o is Terrain) {
					if (((Terrain)o).IsBox) ScreenManager.SpriteBatch.Draw(tileTest, offset + TranslateCoordinates(new Vector2(o.Position.X, o.Position.Y)), null, Color.White, 0f, new Vector2(0, tileTest.Height - TileHeight), 1f, SpriteEffects.None, 1f);
					else ScreenManager.SpriteBatch.Draw(tileCube, offset + TranslateCoordinates(new Vector2(o.Position.X, o.Position.Y)), null, Color.White, 0f, new Vector2(0, tileCube.Height - TileHeight), 1f, SpriteEffects.None, 1f);
				}
				if (o is Actor) ScreenManager.SpriteBatch.Draw(man, offset + TranslateCoordinates(new Vector2(o.Position.X, o.Position.Y)), null, Color.White, 0f, new Vector2(0, man.Height - TileHeight), 1f, SpriteEffects.None, 1f);*/
			}

			if (InputState.InputMethod == InputMethods.KeyboardMouse) ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 8, 32), Color.White);

			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			if (input.IsInput(Inputs.MenuUp, ControllingPlayer)) player.Position.Y -= 0.2f;
			if (input.IsInput(Inputs.MenuDown, ControllingPlayer)) player.Position.Y += 0.2f;
			if (input.IsInput(Inputs.MenuLeft, ControllingPlayer)) player.Position.X -= 0.2f;
			if (input.IsInput(Inputs.MenuRight, ControllingPlayer)) player.Position.X += 0.2f;
		}
	}

	class Terrain : StageObject {
		public bool IsBox = false;

		public Terrain(Vector2 position, bool isBox) {
			Position = position;
			IsBox = isBox;
		}
		public override void Draw(SpriteBatch spriteBatch) {
		}
	}

	class Actor : StageObject {
		public Actor(Vector2 position) {
			Position = position;
		}
		public override void Draw(SpriteBatch spriteBatch) {
		}
	}

	abstract class StageObject {
		public Vector2 Position;
		public abstract void Draw(SpriteBatch spriteBatch);
		public static int TileWidth = 128;
		public static int TileHeight = 64;
		protected Vector2 TranslateCoordinates(Vector2 coordinates) {
			return new Vector2((coordinates.X * TileWidth / 2) + (coordinates.Y * TileWidth / 2), (coordinates.Y * TileHeight / 2) - (coordinates.X * TileHeight / 2));
		}
	}
}
