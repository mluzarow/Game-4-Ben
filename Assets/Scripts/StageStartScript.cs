using UnityEngine;
using System.Collections;

public class StageStartScript : MonoBehaviour {
    public Texture2D background;
    public string nextLevel;

    void OnGUI () {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown (KeyCode.Return)) {
            Application.LoadLevel (nextLevel);
        }
	}
}
