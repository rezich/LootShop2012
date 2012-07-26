//Based on Shawn Hargreaves implementation at:
//http://blogs.msdn.com/b/shawnhar/archive/2007/06/08/displaying-the-framerate.aspx

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LootShop {
	public class FrameRateCounter : DrawableGameComponent {
		ContentManager content;
		SpriteBatch spriteBatch;
		SpriteFont spriteFont;
		String[] numbers;

		int frameRate = 0;
		int frameCounter = 0;
		TimeSpan elapsedTime = TimeSpan.Zero;


		public FrameRateCounter(Game game)
			: base(game) {
			content = game.Content;
			numbers = new String[10];
			for (int j = 0; j < 10; j++) {
				numbers[j] = j.ToString();
			}
		}


		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spriteFont = content.Load<SpriteFont>("UIFontSmall");
		}

		public override void Update(GameTime gameTime) {
			elapsedTime += gameTime.ElapsedGameTime;

			if (elapsedTime > TimeSpan.FromSeconds(1)) {
				elapsedTime -= TimeSpan.FromSeconds(1);
				frameRate = frameCounter;
				frameCounter = 0;
			}
		}

		public override void Draw(GameTime gameTime) {
			frameCounter++;

			//Cap the framerate at 9999 fps
			if (frameRate > 9999) {
				frameRate = 9999;
			}

			//Thousands digit 
			int fps1 = frameRate / 1000;
			//Hundreds digit
			int fps2 = (frameRate - fps1 * 1000) / 100;
			//Tens digit
			int fps3 = (frameRate - fps1 * 1000 - fps2 * 100) / 10;
			//Ones digit
			int fps4 = frameRate - fps1 * 1000 - fps2 * 100 - fps3 * 10;

			spriteBatch.Begin();

			spriteBatch.DrawString(spriteFont, numbers[fps1], new Vector2(33, 33), Color.Black);
			spriteBatch.DrawString(spriteFont, numbers[fps1], new Vector2(32, 32), Color.White);

			spriteBatch.DrawString(spriteFont, numbers[fps2], new Vector2(33 + spriteFont.MeasureString(numbers[fps1]).X, 33), Color.Black);
			spriteBatch.DrawString(spriteFont, numbers[fps2], new Vector2(32 + spriteFont.MeasureString(numbers[fps1]).X, 32), Color.White);

			spriteBatch.DrawString(spriteFont, numbers[fps3], new Vector2(33 + spriteFont.MeasureString(numbers[fps1]).X + spriteFont.MeasureString(numbers[fps2]).X, 33), Color.Black);
			spriteBatch.DrawString(spriteFont, numbers[fps3], new Vector2(32 + spriteFont.MeasureString(numbers[fps1]).X + spriteFont.MeasureString(numbers[fps2]).X, 32), Color.White);

			spriteBatch.DrawString(spriteFont, numbers[fps4], new Vector2(33 + spriteFont.MeasureString(numbers[fps1]).X + spriteFont.MeasureString(numbers[fps2]).X + spriteFont.MeasureString(numbers[fps3]).X, 33), Color.Black);
			spriteBatch.DrawString(spriteFont, numbers[fps4], new Vector2(32 + spriteFont.MeasureString(numbers[fps1]).X + spriteFont.MeasureString(numbers[fps2]).X + spriteFont.MeasureString(numbers[fps3]).X, 32), Color.White);

			spriteBatch.End();
		}
	}
}