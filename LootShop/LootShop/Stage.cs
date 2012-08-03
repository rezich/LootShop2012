using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	class Stage {
		public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
		public List<StageObject> Objects = new List<StageObject>();
		public Vector2 ViewOffset = Vector2.Zero;
		public Vector2 IntendedViewOffset = Vector2.Zero;
		public void SortObjects() {
			Objects.Sort((a, b) => (a.DrawOrder).CompareTo(b.DrawOrder));
		}
		public void Draw(SpriteBatch spriteBatch) {
			foreach (StageObject o in Objects) {
				o.Draw(spriteBatch, ViewOffset);
			}
		}
		public void Update(GameTime gameTime) {
			ViewOffset = Vector2.Lerp(ViewOffset, IntendedViewOffset, 0.1f);
			foreach (StageObject o in Objects) {
				o.Update(gameTime);
			}
			SortObjects();
		}
	}
}
