using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LootShop {
	public class Camera {
		private Vector3 position;
		private Vector3 lookAt;
		private Matrix viewMatrix;
		private Matrix projectionMatrix;
		private float aspectRatio;

		public Camera(Viewport viewport) {
			this.aspectRatio = ((float)viewport.Width) / ((float)viewport.Height);
			this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
										MathHelper.ToRadians(40.0f),
										this.aspectRatio,
										1.0f,
										10000.0f);
		}

		public Vector3 Position {
			get { return this.position; }
			set { this.position = value; }
		}
		public Vector3 LookAt {
			get { return this.lookAt; }
			set { this.lookAt = value; }
		}
		public Matrix ViewMatrix {
			get { return this.viewMatrix; }
		}
		public Matrix ProjectionMatrix {
			get { return this.projectionMatrix; }
		}
		public void Update() {
			this.viewMatrix =
				Matrix.CreateLookAt(this.position, this.lookAt, Vector3.Up);
		}
	}
}
