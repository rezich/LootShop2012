﻿using System;
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
		public StageObject FollowingObject = null;
		public float CameraSpeed = 0.2f;

		public Stage() {
		}

		public void LoadContent() {
		}

		public Vector3 MouseCoordinates {
			get {
				return new Vector3(Mouse.GetState().X, 0, Mouse.GetState().Y) + ViewOffset.ToVector3();
			}
		}

		public void SortObjects() {
			Objects.Sort((a, b) => (a.DrawOrder).CompareTo(b.DrawOrder));
		}

		public void Draw(SpriteBatch spriteBatch) {
			Vector3 cameraPosition = Vector3.Zero;
			
			foreach (StageObject o in Objects) {
				o.Draw(spriteBatch, ViewOffset);
			}
		}

		public void Update(GameTime gameTime) {
			if (FollowingObject != null) IntendedViewOffset = FollowingObject.Position.ToVector2() - new Vector2(Resolution.Right, Resolution.Bottom) / 2;
			ViewOffset = Vector2.Lerp(ViewOffset, IntendedViewOffset, CameraSpeed);
			foreach (StageObject o in Objects) {
				o.Update(gameTime);
			}
			SortObjects();
		}
	}
}
