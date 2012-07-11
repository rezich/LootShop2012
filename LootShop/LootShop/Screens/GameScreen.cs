using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	public abstract class GameScreen {
		public virtual void Initialize() { }
		public virtual void LoadContent() { }
		public virtual void UnloadContent() { }
		public virtual void HandleInput(InputState input) { }
		public virtual void Draw(GameTime gameTime) { }
		public virtual void Update(GameTime gameTime) { }

		public PlayerIndex? ControllingPlayer {
			get { return controllingPlayer; }
			internal set { controllingPlayer = value; }
		}
		PlayerIndex? controllingPlayer;

		public ScreenManager ScreenManager {
			get { return screenManager; }
			internal set { screenManager = value; }
		}
		ScreenManager screenManager;
	}
}
