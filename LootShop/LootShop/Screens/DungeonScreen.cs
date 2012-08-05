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
		//Actor player;
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
			stage.LoadContent();
			song = content.Load<Song>(@"Music\Thrilling");
			MediaPlayer.Play(song);
			ScreenManager.Game.ResetElapsedTime();
		}
		public override void UnloadContent() {
			content.Unload();
		}

		public override void Initialize() {
			stage.Objects.Add(new Terrain(stage, new Vector2(0, 0).ToVector3(), TerrainType.Grass));
			stage.Objects.Add(new Actor(stage, new Vector2(0, 0).ToVector3()));
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			stage.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {

			ScreenManager.BeginSpriteBatch();

			stage.Draw(ScreenManager.SpriteBatch);

			if (InputState.InputMethod == InputMethods.KeyboardMouse) ScreenManager.SpriteBatch.Draw(GameSession.Current.Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
		}
	}
}
