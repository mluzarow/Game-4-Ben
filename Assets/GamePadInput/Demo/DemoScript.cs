//Author: Richard Pieterse
//Date: 16 May 2013
//Email: Merrik44@live.com

using UnityEngine;
using System.Collections;
using GamePadInput;

public class DemoScript : MonoBehaviour
{


    /*void Examples()
    {
        GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.One);
        GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);
        GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One);

        GamepadState state = GamePad.GetState(GamePad.Index.One);

        print("A: " + state.A);
    }*/

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 20, Screen.width, Screen.height));

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();

        DrawLabels();
		
        DrawState();
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();

    }


    void DrawState()
    {
        GUILayout.Space(45);

        GUILayout.BeginVertical();
        
		GamePad.GamePadState state = GamePad.GetState();

        // buttons
        GUILayout.Label("Gamepad");
        GUILayout.Label("" + state.Btn_A);
		GUILayout.Label("" + state.Btn_B);
		GUILayout.Label("" + state.Btn_X);
		GUILayout.Label("" + state.Btn_Y);
		GUILayout.Label("" + state.Btn_Start);
		GUILayout.Label("" + state.Btn_Select);
		GUILayout.Label("" + state.Btn_ShoulderLeft);
		GUILayout.Label("" + state.Btn_ShoulderRight);
		GUILayout.Label("" + state.Btn_StickLeftClick);
        GUILayout.Label("" + state.Btn_StickRightClick);

        GUILayout.Label("");

        // triggers
        GUILayout.Label("" + System.Math.Round(state.Trigger_Left, 2));
        GUILayout.Label("" +  System.Math.Round(state.Trigger_Right, 2));

        GUILayout.Label("");

        // Axes
        GUILayout.Label("" + state.Axis_StickLeft);
        GUILayout.Label("" + state.Axis_StickRight);
        GUILayout.Label("" + state.Axis_Dpad);
        

        //GUILayout.EndArea();
        GUILayout.EndVertical();

    }

    void DrawLabels()
    {
        //GUILayout.BeginArea(new Rect(30, 0, width - 30, Screen.height));

        GUILayout.BeginVertical();
        // buttons
        GUILayout.Label(" ", GUILayout.Width(80));
        GUILayout.Label("A");
        GUILayout.Label("B");
        GUILayout.Label("X");
        GUILayout.Label("Y");
        GUILayout.Label("Start");
        GUILayout.Label("Back");
        GUILayout.Label("Left Shoulder");
        GUILayout.Label("Right Shoulder");
        GUILayout.Label("LeftStick");
        GUILayout.Label("RightStick");

        GUILayout.Label("");

        GUILayout.Label("LeftTrigger");
        GUILayout.Label("RightTrigger");

        GUILayout.Label("");

        GUILayout.Label("LeftStickAxis");
        GUILayout.Label("rightStickAxis");
        GUILayout.Label("dPadAxis");

        GUILayout.EndVertical();

    }
}
