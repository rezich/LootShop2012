using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LootSystem;

namespace LootShop {
	class PressStartScreen : GameScreen {
		public override void Draw(GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin();
			ScreenManager.SpriteBatch.DrawString(LootShop.CurrentGame.UIFontSmall, "PRESS START", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2), Color.White, 0.0f, LootShop.CurrentGame.UIFontSmall.MeasureString("PRESS START") / 2, 1.0f, SpriteEffects.None, 0.0f);
			ScreenManager.SpriteBatch.End();
		}

		public override void HandleInput(InputState input) {
			PlayerIndex playerIndex;
			if (input.IsPressStart(null, out playerIndex)) ScreenManager.ReplaceScreen(new TestScreen(), playerIndex);
		}
	}
}
