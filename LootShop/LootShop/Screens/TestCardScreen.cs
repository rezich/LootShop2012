using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	class TestCardScreen : GameScreen {
		public override void Draw(Microsoft.Xna.Framework.GameTime gameTime) {
			ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Resolution.getTransformationMatrix());
			ScreenManager.SpriteBatch.Draw(GameSession.Current.TestBackground, Vector2.Zero, Color.White);
			ScreenManager.SpriteBatch.End();
		}
	}
}
