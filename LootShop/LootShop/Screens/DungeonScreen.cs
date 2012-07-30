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
		Actor player;
		Stage stage;

		public DungeonScreen() {
			stage = new Stage();
			player = new Actor(stage, new Vector2(25, 100));
			stage.Objects.Add(player);
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					stage.Objects.Add(new Terrain(stage, new Vector2(j, i), false));
				}
			}
			stage.Objects.Remove(stage.Objects.Find(x => x.TilePosition.X == 1 && x.TilePosition.Y == 1));
			stage.Objects.Add(new Terrain(stage, new Vector2(1, 1), true));
			/*stage.Objects.Add(new Terrain(stage, new Vector2(0, 0), false));
			stage.Objects.Add(new Terrain(stage, new Vector2(1, 0), false));
			stage.Objects.Add(new Terrain(stage, new Vector2(0, 1), false));
			stage.Objects.Add(new Terrain(stage, new Vector2(3, 3), false));
			stage.Objects.Add(new Terrain(stage, new Vector2(4, 4), false));
			stage.Objects.Add(new Terrain(stage, new Vector2(3, 4), false));*/
		}

		public override void LoadContent() {
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");
			stage.Textures.Add("tileTest", content.Load<Texture2D>(@"Tiles\tileTest"));
			stage.Textures.Add("tileCube", content.Load<Texture2D>(@"Tiles\tileCube"));
			stage.Textures.Add("man", content.Load<Texture2D>(@"Tiles\man"));
			stage.ViewOffset = new Vector2(400, 400);
			ScreenManager.Game.ResetElapsedTime();
		}
		public override void UnloadContent() {
			content.Unload();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			stage.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			ScreenManager.BeginSpriteBatch();
			ScreenManager.SpriteBatch.Draw(GameSession.Current.Pixel, Resolution.Rectangle, Color.Gray);

			stage.Draw(ScreenManager.SpriteBatch);

			for (int i = 0; i < stage.Objects.Count; i++) {
				ScreenManager.SpriteBatch.DrawStringOutlined(GameSession.Current.UIFontSmall, stage.Objects[i].GetType().ToString(), new Vector2(0, i * GameSession.Current.UIFontSmall.LineSpacing), Color.White);
			}

			if (InputState.InputMethod == InputMethods.KeyboardMouse) ScreenManager.SpriteBatch.Draw(GameSession.Current.Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			if (input.IsInput(Inputs.MenuUp, ControllingPlayer)) player.IntendedPosition.Y -= 8;
			if (input.IsInput(Inputs.MenuDown, ControllingPlayer)) player.IntendedPosition.Y += 8;
			if (input.IsInput(Inputs.MenuLeft, ControllingPlayer)) player.IntendedPosition.X -= 8;
			if (input.IsInput(Inputs.MenuRight, ControllingPlayer)) player.IntendedPosition.X += 8;

			player.IntendedPosition += input.LeftThumbstick(ControllingPlayer) * 4;

			if (input.CurrentMouseState.LeftButton == ButtonState.Pressed) player.IntendedPosition = new Vector2(input.CurrentMouseState.X, input.CurrentMouseState.Y) - stage.ViewOffset;
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
			if (isBox) IsFlat = true;
			TilePosition = position;
			Position = TranslateCoordinates(position) - (TileSize / 2);
			IsBox = isBox;
			Priority = IsFlat ? 1 : 0;
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
			Priority = 2;
		}
	}

	abstract class StageObject {
		public Vector2 Position;
		public Vector2 TilePosition;
		public bool IsFlat = false;
		public abstract Texture2D CurrentFrame { get; }
		public abstract Vector2 Origin { get; }
		public int Priority;
		public void Draw(SpriteBatch spriteBatch, Vector2 offset) {
			Vector2 pos = offset + Position;
			pos.X = (float)Math.Round(pos.X);
			pos.Y = (float)Math.Round(pos.Y);
			spriteBatch.Draw(CurrentFrame, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 1f);
			spriteBatch.Draw(GameSession.Current.Pixel, offset + Position, Color.Red);
		}
		public abstract void Update(GameTime gameTime);
		public static Vector2 TileSize = new Vector2(128, 64);
		protected Stage Stage;
		protected Vector2 TranslateCoordinates(Vector2 coordinates) {
			return new Vector2((coordinates.X * TileSize.X / 2) + (coordinates.Y * TileSize.X / 2), (coordinates.Y * TileSize.Y / 2) - (coordinates.X * TileSize.Y / 2));
		}
	}

	class Stage {
		public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
		public List<StageObject> Objects = new List<StageObject>();
		public Vector2 ViewOffset = Vector2.Zero;
		public void SortObjects() {
			Objects.Sort((a, b) => (a.Position.Y + a.Priority - (a.IsFlat ? StageObject.TileSize.Y / 2 : 0)).CompareTo(b.Position.Y + b.Priority - (b.IsFlat ? StageObject.TileSize.Y / 2 : 0)));
		}
		public void Draw(SpriteBatch spriteBatch) {
			foreach (StageObject o in Objects) {
				o.Draw(spriteBatch, ViewOffset);
			}
		}
		public void Update(GameTime gameTime) {
			foreach (StageObject o in Objects) {
				o.Update(gameTime);
			}
			SortObjects();
		}
	}
}
