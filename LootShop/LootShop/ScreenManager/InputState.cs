using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LootShop {
	public enum MouseButtons {
		Left,
		Right,
		Middle,
		X1,
		X2
	}
	public enum InputMethods {
		Gamepad,
		KeyboardMouse
	}
	public class InputState {
		public const int MaxInputs = 4;

		public readonly KeyboardState[] CurrentKeyboardStates;
		public readonly GamePadState[] CurrentGamePadStates;
		public MouseState CurrentMouseState;

		public readonly KeyboardState[] LastKeyboardStates;
		public readonly GamePadState[] LastGamePadStates;
		public MouseState LastMouseState;

		public static InputMethods InputMethod;

		public readonly bool[] GamePadWasConnected;

		public InputState() {
			CurrentKeyboardStates = new KeyboardState[MaxInputs];
			CurrentGamePadStates = new GamePadState[MaxInputs];
			
			LastKeyboardStates = new KeyboardState[MaxInputs];
			LastGamePadStates = new GamePadState[MaxInputs];

			CurrentMouseState = new MouseState();
			LastMouseState = new MouseState();

			GamePadWasConnected = new bool[MaxInputs];
		}

		public void Update() {
			LastMouseState = CurrentMouseState;
			CurrentMouseState = Mouse.GetState();
			for (int i = 0; i < MaxInputs; i++) {
				LastKeyboardStates[i] = CurrentKeyboardStates[i];
				LastGamePadStates[i] = CurrentGamePadStates[i];

				CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
				CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

				if (CurrentGamePadStates[i].IsConnected) {
					GamePadWasConnected[i] = true;
				}
			}
			if (IsKeyboardInteracted() || IsMouseInteracted()) InputMethod = InputMethods.KeyboardMouse;
			if (IsGamepadInteracted()) InputMethod = InputMethods.Gamepad;
		}

		public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
									out PlayerIndex playerIndex) {
			if (controllingPlayer.HasValue) {
				playerIndex = controllingPlayer.Value;

				int i = (int)playerIndex;

				return (CurrentKeyboardStates[i].IsKeyDown(key) &&
						LastKeyboardStates[i].IsKeyUp(key));
			}
			else {
				return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
						IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
						IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
						IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
			}
		}

		public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
													 out PlayerIndex playerIndex) {
			if (controllingPlayer.HasValue) {
				playerIndex = controllingPlayer.Value;

				int i = (int)playerIndex;

				return (CurrentGamePadStates[i].IsButtonDown(button) &&
						LastGamePadStates[i].IsButtonUp(button));
			}
			else {
				return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
						IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
						IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
						IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
			}
		}

		public bool IsNewMousePress(MouseButtons button) {
			switch (button) {
				case MouseButtons.Left: return CurrentMouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton != ButtonState.Pressed;
				case MouseButtons.Middle: return CurrentMouseState.MiddleButton == ButtonState.Pressed && LastMouseState.MiddleButton != ButtonState.Pressed;
				case MouseButtons.Right: return CurrentMouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton != ButtonState.Pressed;
				case MouseButtons.X1: return CurrentMouseState.XButton1 == ButtonState.Pressed && LastMouseState.XButton1 != ButtonState.Pressed;
				case MouseButtons.X2: return CurrentMouseState.XButton2 == ButtonState.Pressed && LastMouseState.XButton2 != ButtonState.Pressed;
				default: return false;
			}
		}

		public bool IsMouseMoved() {
			return CurrentMouseState.X != LastMouseState.X || CurrentMouseState.Y != CurrentMouseState.Y;
		}

		public bool IsMouseInteracted() {
			return CurrentMouseState != LastMouseState;
		}

		public bool IsKeyboardInteracted() {
			for (int i = 0; i < MaxInputs; i++) {
				if (CurrentKeyboardStates[i] != LastKeyboardStates[i]) return true;
			}
			return false;
		}

		public bool IsGamepadInteracted() {
			for (int i = 0; i < MaxInputs; i++) {
				if (CurrentGamePadStates[i] != LastGamePadStates[i]) return true;
			}
			return false;
			//return !CurrentKeyboardStates.Equals(LastKeyboardStates);
		}

		public bool IsMenuSelect(PlayerIndex? controllingPlayer,
						 out PlayerIndex playerIndex) {
			return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
				   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
				   IsNewKeyPress(Keys.X, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
		}
		public bool IsMenuCancel(PlayerIndex? controllingPlayer,
								 out PlayerIndex playerIndex) {
			return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
				   IsNewKeyPress(Keys.Z, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
		}
		public bool IsMenuUp(PlayerIndex? controllingPlayer) {
			PlayerIndex playerIndex;

			return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
		}
		public bool IsMenuDown(PlayerIndex? controllingPlayer) {
			PlayerIndex playerIndex;

			return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
		}

		public bool IsPressStart(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex) {
			return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
				   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
				   IsNewKeyPress(Keys.X, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex);
		}
		public bool IsPause(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex) {
			return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
				   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
		}
	}
}
