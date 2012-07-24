using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LootShop {
	public enum ScreenState {
		TransitionOn,
		Active,
		TransitionOff,
		Hidden,
	}

	public abstract class GameScreen {
		public ScreenState ScreenState {
			get { return screenState; }
			protected set { screenState = value; }
		}
		ScreenState screenState = ScreenState.TransitionOn;

		public virtual void Initialize() { }
		public virtual void LoadContent() { }
		public virtual void UnloadContent() { }
		public virtual void HandleInput(InputState input) { }
		public virtual void Draw(GameTime gameTime) { }
		public virtual void Update(GameTime gameTime) {
			if (isExiting) {
				// If the screen is going away to die, it should transition off.
				screenState = ScreenState.TransitionOff;

				if (!UpdateTransition(gameTime, transitionOffTime, 1)) {
					// When the transition finishes, remove the screen.
					ScreenManager.RemoveScreen(this);
				}
			}
			else {
				// Otherwise the screen should transition on and become active.
				if (UpdateTransition(gameTime, transitionOnTime, -1)) {
					// Still busy transitioning.
					screenState = ScreenState.TransitionOn;
				}
				else {
					// Transition finished!
					screenState = ScreenState.Active;
				}
			}
		}

		bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction) {
			float transitionDelta;

			if (time == TimeSpan.Zero)
				transitionDelta = 1;
			else
				transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
										  time.TotalMilliseconds);

			transitionPosition += transitionDelta * direction;

			if (((direction < 0) && (transitionPosition <= 0)) ||
				((direction > 0) && (transitionPosition >= 1))) {
				transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
				return false;
			}

			// Otherwise we are still busy transitioning.
			return true;
		}

		public void ExitScreen() {
			if (TransitionOffTime == TimeSpan.Zero) {
				// If the screen has a zero transition time, remove it immediately.
				ScreenManager.RemoveScreen(this);
			}
			else {
				// Otherwise flag that it should transition off and then exit.
				isExiting = true;
			}
		}

		public bool IsExiting {
			get { return isExiting; }
			protected internal set { isExiting = value; }
		}

		bool isExiting = false;

		public TimeSpan TransitionOnTime {
			get { return transitionOnTime; }
			protected set { transitionOnTime = value; }
		}

		TimeSpan transitionOnTime = TimeSpan.Zero;

		public TimeSpan TransitionOffTime {
			get { return transitionOffTime; }
			protected set { transitionOffTime = value; }
		}

		TimeSpan transitionOffTime = TimeSpan.Zero;

		public float TransitionPosition {
			get { return transitionPosition; }
			protected set { transitionPosition = value; }
		}

		public float TransitionPositionSquared {
			get { return (float)Math.Pow(transitionPosition, 2); }
		}

		float transitionPosition = 1;

		public float TransitionAlpha {
			get { return 1f - TransitionPosition; }
		}

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

		public bool TopActive {
			get { return ScreenManager.LastNonExiting() == this; }
		}
	}
}
