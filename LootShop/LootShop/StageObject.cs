using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	abstract class StageObject {
		public Vector2 Position;
		public Vector2 TilePosition;
		public bool IsFlat = false;
		public abstract Texture2D CurrentFrame { get; }
		public abstract Vector2 Origin { get; }
		public float Priority;
		public int Z;
		public int Height;
		public void Draw(SpriteBatch spriteBatch, Vector2 offset) {
			Vector2 pos = offset + Position;
			pos.X = (float)Math.Round(pos.X);
			pos.Y = (float)Math.Round(pos.Y) - Z;
			spriteBatch.Draw(CurrentFrame, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 1f);
			spriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, DrawOrder.ToString(), offset + Position, Color.Blue);
			//spriteBatch.Draw(GameSession.Current.Pixel, offset + Position, Color.Blue);
			//spriteBatch.Draw(GameSession.Current.Pixel, offset + Position - new Vector2(0, Z), Color.Blue);
		}
		public abstract void Update(GameTime gameTime);
		public static Vector2 TileSize = new Vector2(128, 64);
		protected Stage Stage;
		protected Vector2 TranslateCoordinates(Vector2 coordinates) {
			return new Vector2((coordinates.X * TileSize.X / 2) + (coordinates.Y * TileSize.X / 2), (coordinates.Y * TileSize.Y / 2) - (coordinates.X * TileSize.Y / 2));
		}
		public float DrawOrder {
			get {
				return ((int)Position.Y - (IsFlat ? (int)StageObject.TileSize.Y / 2 : -Z)) + Priority;
			}
		}
	}

	class Terrain : StageObject {
		public override Texture2D CurrentFrame {
			get {
				return IsBox ? Stage.Textures["tileCube"] : Stage.Textures["tileTest"];
			}
		}
		public override Vector2 Origin {
			get {
				return new Vector2(CurrentFrame.Width / 2, CurrentFrame.Height - TileSize.Y / 2);
			}
		}
		public bool IsBox = false;

		public override void Update(GameTime gameTime) {
		}

		public Terrain(Stage stage, Vector2 position, bool isBox) {
			Stage = stage;
			//Position = position * TileSize - (TileSize / 2);
			if (!isBox) IsFlat = true;
			TilePosition = position;
			Position = TranslateCoordinates(position) - (TileSize / 2);
			IsBox = isBox;
			Priority = IsFlat ? 0.1f : 0;
			Height = IsFlat ? 0 : 64;
		}
	}

	class Actor : StageObject {
		public override Texture2D CurrentFrame {
			get {
				return Stage.Textures["man"];
			}
		}
		public override Vector2 Origin {
			get {
				return new Vector2(CurrentFrame.Width / 2, CurrentFrame.Height);
			}
		}
		public Vector2 IntendedPosition;

		public override void Update(GameTime gameTime) {
			if (IntendedPosition != Position) Position = Vector2.Lerp(Position, IntendedPosition, 0.1f);
		}

		public Actor(Stage stage, Vector2 position) {
			Stage = stage;
			Position = position;
			IntendedPosition = Position;
			Priority = 0.2f;
			Z = 0;
		}
	}
}
