using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	public static class RectangleHelper {
		public static Rectangle FromVectors(Vector2 a, Vector2 b) {
			return new Rectangle((int)a.X, (int)a.Y, (int)b.X - (int)a.X, (int)b.Y - (int)a.Y);
		}
	}

	public static class Extensions {
		public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color frontColor) {
			DrawStringOutlined(spriteBatch, font, text, position, frontColor, Color.Black, 0f, Vector2.Zero, 1f);
		}
		public static void DrawStringOutlined(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color frontColor, Color backColor, float rotation, Vector2 origin, float scale) {
			Vector2 origin2 = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);

			backColor.A = frontColor.A;

			spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, 1 * scale) + origin2 * scale, backColor, rotation, origin2 + origin, scale, SpriteEffects.None, 1f);

			spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, -1 * scale) + origin2 * scale, backColor, rotation, origin2 + origin, scale, SpriteEffects.None, 1f);
			spriteBatch.DrawString(font, text, position + new Vector2(-1 * scale, 1 * scale) + origin2 * scale, backColor, rotation, origin2 + origin, scale, SpriteEffects.None, 1f);

			spriteBatch.DrawString(font, text, position + new Vector2(1 * scale, -1 * scale) + origin2 * scale, backColor, rotation, origin2 + origin, scale, SpriteEffects.None, 1f);
			spriteBatch.DrawString(font, text, position + origin2 * scale, frontColor, rotation, origin + origin2, scale, SpriteEffects.None, 1f);

		}
	}
}
