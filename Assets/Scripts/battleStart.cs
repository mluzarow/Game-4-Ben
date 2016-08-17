using UnityEngine;
using System.Collections;

public class battleStart : MonoBehaviour {
	public Texture2D screenBG;
	public Texture2D VSbinder;
	Rect screenBGsize;
	Rect VSbinderSize;

	void Start() {
		screenBGsize = new Rect (Screen.width, Screen.height, Screen.width, Screen.height);
		VSbinderSize = new Rect (Screen.width, Screen.height, Screen.width, Screen.height);
	}

	void OnGUI() {

	}
}
