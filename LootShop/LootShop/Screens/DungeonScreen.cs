using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LootSystem;

namespace LootShop {
	class DungeonScreen : GameScreen {
		ContentManager content;
		Actor player;
		Stage stage;
		Song song;

		public DungeonScreen() {
			stage = new Stage();
		}

		public override void LoadContent() {
			if (content == null) content = new ContentManager(ScreenManager.Game.Services, "Content");
			stage.Textures.Add("grass", content.Load<Texture2D>(@"Textures\Test\grass"));
			stage.Textures.Add("wall", content.Load<Texture2D>(@"Textures\Test\wall"));
			stage.Textures.Add("creature", content.Load<Texture2D>(@"Textures\Test\creature"));
			stage.Textures.Add("table", content.Load<Texture2D>(@"Textures\Test\table"));
			stage.Textures.Add("item", content.Load<Texture2D>(@"Textures\Test\item"));
			stage.Textures.Add("bloodSplat", content.Load<Texture2D>(@"Textures\Test\bloodSplat"));
			stage.LoadContent();
			song = content.Load<Song>(@"Music\Thrilling");
			MediaPlayer.Play(song);
			ScreenManager.Game.ResetElapsedTime();
		}

		public override void UnloadContent() {
			content.Unload();
		}

		public override void Initialize() {
			for (int i = 0; i < 10; i++) {
				for (int j = 0; j < 10; j++) {
					new Terrain.Floor(stage, new Vector2(j, i).ToVector3());
				}
			}
			Hero hero = new Hero();
			new Terrain.Wall(stage, new Vector2(0, 1).ToVector3());
			player = new Actor(stage, new Vector2(500, 500).ToVector3(), hero);
			new Prop.Table(stage, new Vector2(3, 3).ToVector3());
			new Prop.Table(stage, new Vector2(3, 4).ToVector3());
			new Prop.Item(stage, new Vector2(3, 3).ToVector3());
			new Actor(stage, new Vector2(400, 400).ToVector3(), Creature.Create(CreatureType.Zombie));
			stage.FollowingObject = player;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			if (!TopActive) return;
			stage.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {

			ScreenManager.BeginSpriteBatch();

			stage.Draw(ScreenManager.SpriteBatch);

			ScreenManager.SpriteBatch.DrawLine(new Vector2(100, 100), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 1, Color.White);
			ScreenManager.SpriteBatch.DrawNgon(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 100, 5, 3, Color.White, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 5));
			ScreenManager.SpriteBatch.DrawCircle(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 300, 2, Color.White, 0);

			if (InputState.InputMethod == InputMethods.KeyboardMouse) ScreenManager.SpriteBatch.Draw(GameSession.Current.Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			if (input.IsInput(Inputs.GamePause, ControllingPlayer)) ScreenManager.AddScreen(new PauseScreen(), ControllingPlayer);
			if (InputState.InputMethod == InputMethods.Gamepad) {
				//player.Position += input.LeftThumbstick(ControllingPlayer).ToVector3() * 8;
				player.MoveInDirection(input.LeftThumbstick(ControllingPlayer).ToVector3(), player.MoveSpeed);
				player.IntendedPosition = player.Position;
				if (player.Position != player.LastPosition) player.TurnTowardsIntended();
			}
			else {
				if (input.IsNewMousePress(MouseButtons.Left)) {
					player.IntendedPosition = stage.MouseCoordinates;
					for (int i = 0; i < stage.Objects.Count; i++) {
						Rectangle bb = stage.Objects[i].BoundingBox;
						Vector2 dest = stage.Objects[i].Position.Round().ToVector2();// - stage.ViewOffset.Round();
						bb.X += (int)dest.X;
						bb.Y += (int)dest.Y;
						if (bb.Contains(new Point((int)stage.MouseCoordinates.X, (int)stage.MouseCoordinates.Z)) && stage.Objects[i] is Actor && stage.Objects[i] != player && Vector3.Distance(player.Position, stage.Objects[i].Position) < 128) ((Actor)stage.Objects[i]).Kill();
					}
					//player.MoveTowardsIntended();
				}
			}
		}
	}
}
