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
		public Vector3 Position;
		public bool IsFlat = false;
		public abstract Texture2D CurrentFrame { get; }
		public abstract Vector3 Origin { get; }
		public int Height = 0;
		public float Angle = 0;
		public void Draw(SpriteBatch spriteBatch, Vector2 offset) {
			Vector3 pos = Position.Round();
			spriteBatch.Draw(CurrentFrame, pos.ToVector2() - offset, null, Color.White, Angle, Origin.ToVector2(), 1f, SpriteEffects.None, 1f);
		}
		public abstract void Update(GameTime gameTime);
		public static Vector2 TileSize = new Vector2(128, 128);
		protected Stage Stage;
		public int DrawOrder {
			get {
				return (int)Position.Y + Height;
			}
		}
	}

	public enum TerrainType {
		Grass,
		Wall
	}

	class Terrain : StageObject {
		public TerrainType Type;
		public override Texture2D CurrentFrame {
			get {
				return Type == TerrainType.Grass ? Stage.Textures["grass"] : Stage.Textures["wall"];
			}
		}
		public override Vector3 Origin {
			get {
				return Vector3.Zero;
			}
		}

		public override void Update(GameTime gameTime) {
		}

		public Terrain(Stage stage, Vector3 position, TerrainType type) {
			Stage = stage;
			Position = position * StageObject.TileSize.ToVector3();
			Type = type;
			if (Type == TerrainType.Grass) Height = 0;
			else Height = 128;
		}
	}

	class Actor : StageObject {
		public override Texture2D CurrentFrame {
			get {
				return Stage.Textures["creature"];
			}
		}
		public override Vector3 Origin {
			get {
				return new Vector2(CurrentFrame.Width / 2, CurrentFrame.Height / 2).ToVector3();
			}
		}
		//public Vector3 IntendedPosition;

		public override void Update(GameTime gameTime) {
			//if (IntendedPosition != Position) Position = Vector2.Lerp(Position, IntendedPosition, 0.1f);
		}

		public Actor(Stage stage, Vector3 position) {
			Stage = stage;
			Position = position;
			Height = 32;
			//IntendedPosition = Position;
		}
	}
}
