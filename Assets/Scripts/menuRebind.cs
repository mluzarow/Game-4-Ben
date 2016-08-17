using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GamePadInput;

//For reading/writing file
using System.IO;

public class menuRebind : MonoBehaviour {
	Rect rebindLeftSize;
	Rect rebindRightSize;
	Rect rebindUpSize;
	Rect rebindDownSize;
	Rect rebindJumpSize;

	Rect rebindBackSize;

	Rect labelLeftSize;
	Rect labelRightSize;
	Rect labelUpSize;
	Rect labelDownSize;
	Rect labelJumpSize;

	Rect rebindingLabelSize;

	bool lookingForKey = false;
	string keyToChange = "";

	void Start() {
		//Set up GUI
		drawRects ();
	}

	void OnGUI() {
		GUI.Label (labelLeftSize, "LEFT");
		GUI.Label (labelRightSize, "RIGHT");
		GUI.Label (labelUpSize, "UP");
		GUI.Label (labelDownSize, "DOWN");
		GUI.Label (labelJumpSize, "JUMP");

		//Rebind using the key that the user presses
		if (lookingForKey) {
			GUI.Label (rebindingLabelSize, "PRESS A BUTTON TO USE FOR INPUT: " + keyToChange);

			Event e = Event.current;



			if (e.isKey) {
				menuMain.keys[keyToChange] = e.keyCode;
				lookingForKey = false;

				List<string> keysToNull = new List<string>();

				//Check to see if the keyCode is used anywhere else
				foreach (KeyValuePair<string, KeyCode> entry in menuMain.keys) {
					if (entry.Value == e.keyCode && entry.Key != keyToChange) {
						keysToNull.Add (entry.Key);
					}
				}

				//None each matching key's value
				foreach (string entry in keysToNull) {
					menuMain.keys[entry] = KeyCode.None;
				}

				//Save key bind change to file
				savePrefs("playerPrefs.ini");
			}
		//Show menu of current binds
		} else {
			if (GUI.Button (rebindLeftSize, menuMain.keys["LEFT"].ToString())) {
				lookingForKey = true;
				keyToChange = "LEFT";
			}
			if (GUI.Button (rebindRightSize, menuMain.keys["RIGHT"].ToString())) {
				lookingForKey = true;
				keyToChange = "RIGHT";
			}
			if (GUI.Button (rebindUpSize, menuMain.keys["UP"].ToString())) {
				lookingForKey = true;
				keyToChange = "UP";
			}
			if (GUI.Button (rebindDownSize, menuMain.keys["DOWN"].ToString())) {
				lookingForKey = true;
				keyToChange = "DOWN";
			}
			if (GUI.Button (rebindJumpSize, menuMain.keys["JUMP"].ToString())) {
				lookingForKey = true;
				keyToChange = "JUMP";
			}
			if (GUI.Button (rebindBackSize, "Back to menu")) {
				Application.LoadLevel("menuMain");
			}
		}
	}

	//Write key binds to playerPrefs.ini
	bool savePrefs(string filename) {
		try {
			StreamWriter f = new StreamWriter(Application.dataPath + "/Files/" + filename);

			using (f) {
				foreach (KeyValuePair<string, KeyCode> entry in menuMain.keys) {
					f.WriteLine (entry.Key + ":" + entry.Value.ToString());
				}
			}
		} catch (IOException e) {
			print (e);
			print (true);
		}
		return(false);
	}

	//Redraw all Rects on resolution change
	void drawRects() {
		//left top width height
		rebindLeftSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.242f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindRightSize = new Rect (Screen.width * 0.438f, Screen.height * 0.300f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindUpSize =    new Rect (Screen.width * 0.438f, Screen.height * 0.358f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindDownSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.416f, Screen.width * 0.200f, Screen.height * 0.052f);
		rebindJumpSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.472f, Screen.width * 0.200f, Screen.height * 0.052f);

		rebindBackSize =  new Rect (Screen.width * 0.438f, Screen.height * 0.526f, Screen.width * 0.200f, Screen.height * 0.052f);


		labelLeftSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.242f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelRightSize = new Rect (Screen.width * 0.338f, Screen.height * 0.300f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelUpSize =    new Rect (Screen.width * 0.338f, Screen.height * 0.358f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelDownSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.416f, Screen.width * 0.200f, Screen.height * 0.052f);
		labelJumpSize =  new Rect (Screen.width * 0.338f, Screen.height * 0.472f, Screen.width * 0.200f, Screen.height * 0.052f);

		rebindingLabelSize = new Rect (Screen.width * 0.500f, Screen.height * 0.500f, Screen.width * 0.500f, Screen.height * 0.500f);
	}
}
