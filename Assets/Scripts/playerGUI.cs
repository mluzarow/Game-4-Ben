using UnityEngine;
using System.Collections;

public class playerGUI : MonoBehaviour {
	public GUIStyle label1;
	float deltaTime = 0.0f;

	Rect platformerTimerSize = new Rect (Screen.width * 0.1f, Screen.height * 0.1f, 
	                                Screen.width * 0.200f, Screen.height * 0.1f);
	float timer;
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		timer += Time.deltaTime;
	}

	void OnGUI() {
		float fps = 1.0f / deltaTime;
		GUI.Label (platformerTimerSize, string.Format("{0:0.00}", timer) + "\n" + string.Format("{0:0.} FPS", fps));
	}
}
