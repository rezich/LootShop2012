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
		static int TileWidth = 128;
		static int TileHeight = 64;
		List<Tile> tiles = new List<Tile> {
			new Tile(new Vector2(0, 0)),
			new Tile(new Vector2(1, 0)),
			new Tile(new Vector2(3, 0))
		};

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");
			tileTest = content.Load<Texture2D>(@"Tiles\tileTest");
			tileCube = content.Load<Texture2D>(@"Tiles\tileCube");
			ScreenManager.Game.ResetElapsedTime();
		}
		public override void UnloadContent() {
			content.Unload();
		}

		public override void Draw(GameTime gameTime) {
			Vector2 offset = new Vector2(0, Resolution.Bottom / 2);
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Gray);
			tiles = tiles.OrderBy(t => (t.Position.Y * TileHeight / 2) - (t.Position.X * TileHeight / 2)).ToList();
			foreach (Tile t in tiles) {
				ScreenManager.SpriteBatch.Draw(tileCube, offset + TranslateCoordinates(new Vector2(t.Position.X, t.Position.Y)), Color.White);
			}
			/*for (int i = 0; i < 8; i++) {
				for (int j = 7; j >= 0; j--) {
					ScreenManager.SpriteBatch.Draw(tileCube, offset + TranslateCoordinates(new Vector2(i, j)), Color.White);
				}
			}*/
			ScreenManager.SpriteBatch.End();
		}

		private Vector2 TranslateCoordinates(Vector2 coordinates) {
			return new Vector2((coordinates.X * TileWidth / 2) + (coordinates.Y * TileWidth / 2), (coordinates.Y * TileHeight / 2) - (coordinates.X * TileHeight / 2));
		}
	}

	class Tile {
		public Vector2 Position;

		public Tile(Vector2 position) {
			Position = position;
		}
	}
}
