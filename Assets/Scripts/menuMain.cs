using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class menuMain : MonoBehaviour {
	public Texture2D background;
	public GUIStyle button1Style;
	public GUIStyle button2Style;
	public GUIStyle button3Style;
	public GUIStyle optionsVsyncLabelStyle;

	bool showOptions = false;
	bool resolutionChanged = false;
	bool vsyncState = false;
	Rect playButtonSize;
	Rect optionButtonSize;
	Rect exitButtonSize;

	Rect optionsVsyncLabel;
	Rect optionsVysncOnSize;
	Rect optionsVysncOffSize;
	Rect optionsResolutionSize1;
	Rect optionsResolutionSize2;
	Rect optionsResolutionSize3;
	Rect optionsResolutionSize4;
	Rect optionsResolutionSize5;

	Rect optionsRebindSize;
	
	//Dictionary holding all key bindings
	public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
	//List holding all input string keys
	List<string> inputs = new List<string> ();

	void Start() {
		//Populates list of input keys
		populateInputList ();
		//Checks dictionary to make sure input keys are listed and defaults them to none
		checkDictionary ();

		//Load input bindings from player prefs
		loadPrefs ("playerPrefs.ini");

		Screen.SetResolution (1024, 768, false);
		resolutionChanged = true;
	}

	//Populates list of input keys
	void populateInputList () {
		inputs.Add ("LEFT");
		inputs.Add ("RIGHT");
		inputs.Add ("UP");
		inputs.Add ("DOWN");
		inputs.Add ("JUMP");
		inputs.Add ("ACTION");
	}

	//Checks dictionary to make sure input keys are listed and defaults them to none
	void checkDictionary() {
		foreach (string input in inputs) {
			if (!keys.ContainsKey(input)) {
				keys.Add (input, KeyCode.None);
			}
		}
	}

	void OnGUI() {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);

		if (resolutionChanged) {
			drawRects();
		}

		if (GUI.Button (playButtonSize, "", button1Style)) {
			LoadLevel();
		}
		if (GUI.Button (optionButtonSize, "", button2Style)) {
			showOptions = true;
		}
		if (GUI.Button (exitButtonSize, "", button3Style)) {
			Application.Quit();
		}	
		if (showOptions) {

			if (vsyncState) {
				GUI.Label (optionsVsyncLabel, "Turn V-sync on or off\nV-sync is now ON", optionsVsyncLabelStyle);
			} else {
				GUI.Label (optionsVsyncLabel, "Turn V-sync on or off\nV-sync is now OFF", optionsVsyncLabelStyle);
			}


			//Increase quality preset
			/*if (GUI.Button (new Rect(810,100,300,100), "Increase Quality")) {
				QualitySettings.IncreaseLevel();
			}
			//Decrease quality preset
			if (GUI.Button (new Rect(810,210,300,100), "Decrease Quality")) {
				QualitySettings.DecreaseLevel();
			}*/
			//Enable V-Sync
			if (GUI.Button (optionsVysncOnSize, "ON")) {
				QualitySettings.vSyncCount = 1;
				vsyncState = true;
			}
			//Disable V-Sync
			if (GUI.Button (optionsVysncOffSize, "OFF")) {
				QualitySettings.vSyncCount = 0;
				vsyncState = false;
			}
			if (GUI.Button (optionsResolutionSize1, "640x480")) {
				Screen.SetResolution (640, 480, false);
				resolutionChanged = true;
			}
			if (GUI.Button (optionsResolutionSize2, "800x600")) {
				Screen.SetResolution (800, 600, false);
				resolutionChanged = true;
			}
			if (GUI.Button (optionsResolutionSize3, "1024x768")) {
				Screen.SetResolution (1024, 768, false);
				resolutionChanged = true;
			}
			if (GUI.Button (optionsResolutionSize4, "1680x1050")) {
				Screen.SetResolution (1680, 1050, false);
				resolutionChanged = true;
			}
			if (GUI.Button (optionsResolutionSize5, "1920x1080")) {
				Screen.SetResolution (1920, 1080, false);
				resolutionChanged = true;
			}
			if (GUI.Button (optionsRebindSize, "Rebind Keys")) {
				Application.LoadLevel ("rebindScene");
			}

		}

	}

	//Read key binds from playerPrefs.ini
	bool loadPrefs(string filename) {
		//Begin load block
		try {
			//Data will hold single key value pair "string:keycode"
			string data;
			//Key:Value pair split into [0] key [1] value
			string[] currentKey = new string[2];
			
			//Start filestream
			StreamReader f = new StreamReader(Application.dataPath + "/Files/" + filename);
			
			//Begin reading
			using(f) {
				//Check for a line; is a line, read it and check for another
				do {
					//Read a line
					data = f.ReadLine();
					
					//If line had data
					if (data != null) {
						//Split data line into key string and value keycode
						currentKey = data.Split(':');

						//Parse letter as KeyCode and put in dictionary
						keys [currentKey [0]] = (KeyCode) System.Enum.Parse(typeof(KeyCode), currentKey [1]);
					}
				} while (data != null);
				
				//Close the file stream
				f.Close ();
				//Return 0
				return(false);
			}
		} catch (IOException e) {
			print(e);
			return (true);
		}
	}

	void LoadLevel() {
		Application.LoadLevel ("stage1StartScene");
	}

	//Redraw all Rects on resolution change
	void drawRects() {
		playButtonSize = new Rect (Screen.width * 0.738f, Screen.height * 0.242f, 
		                           Screen.width * 0.200f, Screen.height * 0.052f);
		optionButtonSize = new Rect (Screen.width * 0.738f, Screen.height * 0.3f, 
		                             Screen.width * 0.200f, Screen.height * 0.052f);
		exitButtonSize = new Rect (Screen.width * 0.738f, Screen.height * 0.358f, 
		                           Screen.width * 0.200f, Screen.height * 0.052f);
		
		optionsVsyncLabel = new Rect (Screen.width * 0.738f, Screen.height * 0.416f, 
		                              Screen.width * 0.200f, Screen.height * 0.052f);
		optionsVysncOnSize = new Rect (Screen.width * 0.738f, Screen.height * 0.472f, 
		                               Screen.width * 0.090f, Screen.height * 0.052f);
		optionsVysncOffSize = new Rect (Screen.width * 0.848f, Screen.height * 0.472f, 
		                                Screen.width * 0.090f, Screen.height * 0.052f);
		optionsResolutionSize1 = new Rect (Screen.width * 0.738f, Screen.height * 0.528f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
		optionsResolutionSize2 = new Rect (Screen.width * 0.738f, Screen.height * 0.584f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
		optionsResolutionSize3 = new Rect (Screen.width * 0.738f, Screen.height * 0.640f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
		optionsResolutionSize4 = new Rect (Screen.width * 0.738f, Screen.height * 0.696f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
		optionsResolutionSize5 = new Rect (Screen.width * 0.738f, Screen.height * 0.752f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
		optionsRebindSize = new Rect (Screen.width * 0.738f, Screen.height * 0.808f, 
		                                   Screen.width * 0.200f, Screen.height * 0.052f);
	}
}
