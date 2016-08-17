using UnityEngine;
using System.Collections;

namespace GamePadInput {
	public struct AxisCode {
		#region Public Fields
		/// <summary>
		/// This X-axis axis code.
		/// </summary>
		public string x;
		/// <summary>
		/// The Y-axis axis code.
		/// </summary>
		public string y;
		#endregion Public Fields

		#region Constructors
		public AxisCode (string x, string y) {
			this.x = x;
			this.y = y;
		}
		#endregion Constructors
	}

	/// <summary>
	/// Gamepad	
	/// </summary>
	public static class GamePad {
		#region Enums
		private enum Button {A, B, X, Y, LeftShoulder, RightShoulder, Select, Start, LeftStickIn, RightStickIn}
		private enum Axis {LeftStick, RightStick, Dpad}
		private enum Trigger {LeftTrigger, RightTrigger}
		#endregion Enums

		#region Public Variables
		public static GamePadKeyCodes gamepadKeyCodes = new GamePadKeyCodes(0);
		#endregion Public Variables

		#region Structs
		///<summary>
		/// Struct holding all gamepad input keycodes.
		///</summary>
		public struct GamePadKeyCodes {
			public KeyCode Btn_A;
			public KeyCode Btn_B;
			public KeyCode Btn_X;
			public KeyCode Btn_Y;
			public KeyCode Btn_ShoulderLeft;
			public KeyCode Btn_ShoulderRight;
			public KeyCode Btn_Select;
			public KeyCode Btn_Start;
			public KeyCode Btn_StickLeftClick;
			public KeyCode Btn_StickRightClick;
			public AxisCode Axis_Dpad;
			public AxisCode Axis_StickLeft;
			public AxisCode Axis_StickRight;
			public string Trigger_Left;
			public string Trigger_Right;

			public GamePadKeyCodes(int i) {
				Btn_A = GetKeyCode (Button.A);
				Btn_B = GetKeyCode (Button.B);
				Btn_X = GetKeyCode (Button.X);
				Btn_Y = GetKeyCode (Button.Y);
				Btn_ShoulderLeft    = GetKeyCode (Button.LeftShoulder);
				Btn_ShoulderRight   = GetKeyCode (Button.RightShoulder);
				Btn_Select          = GetKeyCode (Button.Select);
				Btn_Start           = GetKeyCode (Button.Start);
				Btn_StickLeftClick  = GetKeyCode (Button.LeftStickIn);
				Btn_StickRightClick = GetKeyCode (Button.RightStickIn);
				Axis_Dpad 		= GetAxisCode (Axis.Dpad);
				Axis_StickLeft 	= GetAxisCode (Axis.LeftStick);
				Axis_StickRight = GetAxisCode (Axis.RightStick);
				Trigger_Left  = GetTriggerCode (Trigger.LeftTrigger);
				Trigger_Right = GetTriggerCode (Trigger.RightTrigger);
			}
		}

		public struct GamePadState {
			public bool Btn_A;
			public bool Btn_B;
			public bool Btn_X;
			public bool Btn_Y;
			public bool Btn_ShoulderLeft;
			public bool Btn_ShoulderRight;
			public bool Btn_Select;
			public bool Btn_Start;
			public bool Btn_StickLeftClick;
			public bool Btn_StickRightClick;
			public Vector2 Axis_Dpad;
			public Vector2 Axis_StickLeft;
			public Vector2 Axis_StickRight;
			public float Trigger_Left;
			public float Trigger_Right;

			public GamePadState (int i) {
				Btn_A = false;
				Btn_B = false;
				Btn_X = false;
				Btn_Y = false;
				Btn_ShoulderLeft = false;
				Btn_ShoulderRight = false;
				Btn_Select = false;
				Btn_Start = false;
				Btn_StickLeftClick = false;
				Btn_StickRightClick = false;
				Axis_Dpad = Vector2.zero;
				Axis_StickLeft = Vector2.zero;
				Axis_StickRight = Vector2.zero;
				Trigger_Left = 0f;
				Trigger_Right = 0f;
			}
		}
		#endregion Structs

		#region Key Finds
		/// <summary>
		/// Gets the KeyCode of the submitted gamepad face button.
		/// </summary>
		private static KeyCode GetKeyCode(Button btn) {
			if (btn == Button.A) {
				return (KeyCode.JoystickButton0);
			} else if (btn == Button.B) {
				return (KeyCode.JoystickButton1);
			} else if (btn == Button.X) {
				return (KeyCode.JoystickButton2);
			} else if (btn == Button.Y) {
				return (KeyCode.JoystickButton3);
			} else if (btn == Button.LeftShoulder) {
				return (KeyCode.JoystickButton4);
			} else if (btn == Button.RightShoulder) {
				return (KeyCode.JoystickButton5);
			} else if (btn == Button.Select) {
				return (KeyCode.JoystickButton6);
			} else if (btn == Button.Start) {
				return (KeyCode.JoystickButton7);
			} else if (btn == Button.LeftStickIn) {
				return (KeyCode.JoystickButton8);
			} else if (btn == Button.RightStickIn) {
				return (KeyCode.JoystickButton9);
			} else {
				return (KeyCode.None);
			}
		}

		private static AxisCode GetAxisCode (Axis axis) {
			AxisCode axisData;

			if (axis == Axis.Dpad) {
				axisData.x = "DPad_XAxis_1";
				axisData.y = "DPad_YAxis_1";
			} else if (axis == Axis.LeftStick) {
				axisData.x = "L_XAxis_1";
				axisData.y = "L_YAxis_1";
			} else if (axis == Axis.RightStick) {
				axisData.x = "R_XAxis_1";
				axisData.y = "R_YAxis_1";
			} else {
				axisData.x = "";
				axisData.y = "";
			}

			return (axisData);
		}

		private static string GetTriggerCode (Trigger trigger) {
			string code = "";

			if (trigger == Trigger.LeftTrigger) {
				code = "TriggerL_1";
			} else if (trigger == Trigger.RightTrigger) {
				code = "TriggerR_1";
			}

			return (code);
		}
		#endregion Key Finds

		public static GamePadState GetState () {
			GamePadState state = new GamePadState ();

			state.Btn_A = Input.GetKey (gamepadKeyCodes.Btn_A);
			state.Btn_B = Input.GetKey (gamepadKeyCodes.Btn_B);
			state.Btn_X = Input.GetKey (gamepadKeyCodes.Btn_X);
			state.Btn_Y = Input.GetKey (gamepadKeyCodes.Btn_Y);
			state.Btn_ShoulderLeft = Input.GetKey (gamepadKeyCodes.Btn_ShoulderLeft);
			state.Btn_ShoulderRight = Input.GetKey (gamepadKeyCodes.Btn_ShoulderRight);
			state.Btn_Start = Input.GetKey (gamepadKeyCodes.Btn_Start);
			state.Btn_Select = Input.GetKey (gamepadKeyCodes.Btn_Select);
			state.Axis_StickLeft.x = Input.GetAxis (gamepadKeyCodes.Axis_StickLeft.x);
			state.Axis_StickLeft.y = Input.GetAxis (gamepadKeyCodes.Axis_StickLeft.y);
			state.Axis_StickRight.x = Input.GetAxis (gamepadKeyCodes.Axis_StickRight.x);
			state.Axis_StickRight.y = Input.GetAxis (gamepadKeyCodes.Axis_StickRight.y);
			state.Axis_Dpad.x = Input.GetAxis (gamepadKeyCodes.Axis_Dpad.x);
			state.Axis_Dpad.y = Input.GetAxis (gamepadKeyCodes.Axis_Dpad.y);
			state.Trigger_Left = Input.GetAxis (gamepadKeyCodes.Trigger_Left);
			state.Trigger_Right = Input.GetAxis (gamepadKeyCodes.Trigger_Right);

			return (state);
		}
		/*
		public static Vector2 GetAxis (AxisCode axis) {
			Vector2 axisVals = new Vector2();

			axisVals.x = Input.GetAxis (axis.x);
			axisVals.y = -Input.GetAxis (axis.y);

			return (axisVals);
		}

		public static Vector2 GetAxis (AxisCode axis, bool raw) {
			Vector2 axisVals = new Vector2();

			if (raw) {
				axisVals.x = Input.GetAxisRaw (axis.x);
				axisVals.y = -Input.GetAxisRaw (axis.y);
			} else if (!raw) {
				axisVals.x = Input.GetAxis (axis.x);
				axisVals.y = -Input.GetAxis (axis.y);
			}

			return (axisVals);
		}*/
	}
}