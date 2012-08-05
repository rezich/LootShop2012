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
		public Vector3 LastPosition;
		public bool IsFlat = false;
		public abstract Texture2D CurrentFrame { get; }
		public abstract Vector3 Origin { get; }
		public abstract Color Color { get; }
		public int Height = 0;
		public float Angle = 0;
		public float IntendedAngle = 0;
		public void Draw(SpriteBatch spriteBatch, Vector2 offset) {
			spriteBatch.Draw(CurrentFrame, Position.Round().ToVector2() - offset.Round(), null, Color.White, Angle, Origin.ToVector2(), 1f, SpriteEffects.None, 1f);
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

		public override Color Color {
			get { return Color.White; }
		}

		public override void Update(GameTime gameTime) {
		}

		public Terrain(Stage stage, Vector3 position, TerrainType type) {
			Stage = stage;
			Stage.Objects.Add(this);
			Position = position * StageObject.TileSize.ToVector3();
			Type = type;
			if (Type == TerrainType.Grass) Height = 0;
			else Height = 128;
		}
	}

	class Actor : StageObject {
		public float MoveSpeed = 8;
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

		protected Color color = Color.White;
		public override Color Color {
			get { return color; }
		}

		public Vector3 IntendedPosition;

		public void MoveTowardsIntended() {
			if (Vector3.Distance(Position, IntendedPosition) > 4) {
				Vector3 velocity = Vector3.Normalize(IntendedPosition - Position);
				MoveInDirection(velocity, MoveSpeed);
				TurnTowardsIntended();
			}
		}

		public void TurnTowardsIntended() {
			IntendedAngle = (float)Math.Atan2(Position.Z - LastPosition.Z, Position.X - LastPosition.X);
		}

		public void MoveInDirection(Vector3 direction, float speed) {
			Position += direction * speed;
		}

		public override void Update(GameTime gameTime) {
			MoveTowardsIntended();
			if (Angle != IntendedAngle) Angle = Angle.LerpAngle(IntendedAngle, 0.3f);
			LastPosition = Position;
		}

		public Actor(Stage stage, Vector3 position) {
			Stage = stage;
			Stage.Objects.Add(this);
			Position = position;
			IntendedPosition = position;
			Height = 32;
			//IntendedPosition = Position;
		}
	}
}
