using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using LootSystem;

namespace LootShop {
	public static class RectangleHelper {
		public static Rectangle FromVectors(Vector2 a, Vector2 b) {
			return new Rectangle((int)a.X, (int)a.Y, (int)b.X - (int)a.X, (int)b.Y - (int)a.Y);
		}
	}

	public static class CampaignHelper {
		public static Campaign Load(string fileName) {
			using (Stream stream = File.OpenRead(fileName)) {
				XmlSerializer serializer = new XmlSerializer(typeof(Campaign));
				return (Campaign)serializer.Deserialize(stream);
			}
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
		public static void Save(this Campaign campaign, string fileName) {
			File.Delete(fileName);
			using (Stream stream = File.OpenWrite(fileName)) {
				XmlSerializer serializer = new XmlSerializer(typeof(Campaign));
				serializer.Serialize(stream, campaign);
			}
		}
		public static Vector2 Round(this Vector2 vector) {
			return new Vector2((float)Math.Round(vector.X), (float)Math.Round(vector.Y));
		}
		public static Vector3 Round(this Vector3 vector) {
			return new Vector3((float)Math.Round(vector.X), (float)Math.Round(vector.Y), (float)Math.Round(vector.Z));
		}
		public static Vector3 ToVector3(this Vector2 vector) {
			return new Vector3(vector.X, 0, vector.Y);
		}
		public static Vector2 ToVector2(this Vector3 vector) {
			return new Vector2(vector.X, vector.Z);
		}
		public static float Wrap(this float value, float lower, float upper) {
			float distance = upper - lower;
			float times = (float)System.Math.Floor((value - lower) / distance);

			return value - (times * distance);
		}
		public static float LerpAngle(this float from, float to, float step) {
			// Ensure that 0 <= angle < 2pi for both "from" and "to" 
			while (from < 0)
				from += MathHelper.TwoPi;
			while (from >= MathHelper.TwoPi)
				from -= MathHelper.TwoPi;

			while (to < 0)
				to += MathHelper.TwoPi;
			while (to >= MathHelper.TwoPi)
				to -= MathHelper.TwoPi;

			if (System.Math.Abs(from - to) < MathHelper.Pi) {
				// The simple case - a straight lerp will do. 
				return MathHelper.Lerp(from, to, step);
			}

			// If we get here we have the more complex case. 
			// First, increment the lesser value to be greater. 
			if (from < to)
				from += MathHelper.TwoPi;
			else
				to += MathHelper.TwoPi;

			float retVal = MathHelper.Lerp(from, to, step);

			// Now ensure the return value is between 0 and 2pi 
			if (retVal >= MathHelper.TwoPi)
				retVal -= MathHelper.TwoPi;
			return retVal;
		} 
	}
}
