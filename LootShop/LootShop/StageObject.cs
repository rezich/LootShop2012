using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	abstract class StageObject {
		public Vector3 Position;
		public Vector3 LastPosition;
		public bool IsFlat = false;
		public bool Solid = false;
		public abstract Texture2D CurrentFrame { get; }
		public abstract Vector3 Origin { get; }
		public abstract Color Color { get; }
		public int Height = 0;
		public float Angle = 0;
		public float IntendedAngle = 0;
		public void Draw(SpriteBatch spriteBatch, Vector2 offset) {
			Vector2 destination = Position.Round().ToVector2() - offset.Round();
			spriteBatch.Draw(CurrentFrame, destination, null, Color.White, Angle, Origin.ToVector2(), 1f, SpriteEffects.None, 1f);
			spriteBatch.Draw(GameSession.Current.Pixel, new Rectangle((int)destination.X + BoundingBox.X, (int)destination.Y + BoundingBox.Y, BoundingBox.Width, BoundingBox.Height), Color.Lime * 0.3f);
		}
		public abstract void Update(GameTime gameTime);
		public static Vector2 TileSize = new Vector2(64, 64);
		protected Stage Stage;
		public float DrawOrder {
			get {
				float bonus = 0;
				if (this is Actor) bonus = 0.5f;
				if (this is Terrain) bonus = 0.1f;
				return Position.Y + Height + bonus;
			}
		}
		public abstract Rectangle BoundingBox { get; }
	}

	abstract class Terrain : StageObject {
		public override Vector3 Origin {
			get {
				return Vector3.Zero;
			}
		}
		public override Rectangle BoundingBox {
			get { return Rectangle.Empty; }
		}
		public override Color Color {
			get { return Color.White; }
		}

		public override void Update(GameTime gameTime) {
		}

		public class Floor : Terrain {
			public override Texture2D CurrentFrame {
				get {
					return Stage.Textures["grass"];
				}
			}
			public Floor(Stage stage, Vector3 position) {
				Stage = stage;
				Stage.Objects.Add(this);
				Position = position * StageObject.TileSize.ToVector3();
				Height = 0;
				Solid = false;
			}
		}

		public class Wall : Terrain {
			public override Texture2D CurrentFrame {
				get {
					return Stage.Textures["wall"];
				}
			}
			public Wall(Stage stage, Vector3 position) {
				Stage = stage;
				Stage.Objects.Add(this);
				Position = position * StageObject.TileSize.ToVector3();
				Height = 128;
				Solid = true;
			}
		}
	}

	abstract class Prop : StageObject {
		public override Color Color {
			get { return Color.White; }
		}
		public override Vector3 Origin {
			get { return new Vector2(CurrentFrame.Width / 2, CurrentFrame.Height / 2).ToVector3(); }
		}
		public override Rectangle BoundingBox {
			get { return Rectangle.Empty; }
		}

		public override void Update(GameTime gameTime) {
		}

		public class Table : Prop {
			public override Texture2D CurrentFrame {
				get {
					return Stage.Textures["table"];
				}
			}
			public Table(Stage stage, Vector3 position) {
				Stage = stage;
				Stage.Objects.Add(this);
				Position = position * TileSize.ToVector3() - TileSize.ToVector3() / 2;
				Height = 32;
			}
		}

		public class Item : Prop {
			public override Texture2D CurrentFrame {
				get {
					return Stage.Textures["item"];
				}
			}
			public Item(Stage stage, Vector3 position) {
				Stage = stage;
				Stage.Objects.Add(this);
				Position = position * TileSize.ToVector3() - TileSize.ToVector3() / 2;
				Height = 4;
			}
		}
	}

	class Actor : StageObject {
		public Creature Creature;
		public float MoveSpeed = 4;
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
		public override Rectangle BoundingBox {
			get {
				int size = 36;
				return new Rectangle(-size / 2, -size / 2, size, size);
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

		public void Kill() {
			Stage.Objects.Remove(this);
			new Decal(Stage, Position);
		}

		public override void Update(GameTime gameTime) {
			MoveTowardsIntended();
			if (Angle != IntendedAngle) Angle = Angle.LerpAngle(IntendedAngle, 0.3f);
			LastPosition = Position;
		}

		public Actor(Stage stage, Vector3 position, Creature creature) {
			Stage = stage;
			Stage.Objects.Add(this);
			Position = position;
			IntendedPosition = position;
			Creature = creature;
			Height = 48;
		}
	}

	class Decal : StageObject {
		public override Texture2D CurrentFrame {
			get {
				return Stage.Textures["bloodSplat"];
			}
		}
		public override Vector3 Origin {
			get { return new Vector2(CurrentFrame.Width / 2, CurrentFrame.Height / 2).ToVector3(); }
		}
		public override Color Color {
			get { return Color.White; }
		}
		public override Rectangle BoundingBox {
			get { return Rectangle.Empty; }
		}

		public override void Update(GameTime gameTime) {
		}

		public Decal(Stage stage, Vector3 position) {
			Stage = stage;
			stage.Objects.Add(this);
			Position = position;
			Angle = (float)(GameSession.Random.NextDouble() * MathHelper.TwoPi);
			Height = 1;
		}
	}
}
