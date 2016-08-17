using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {
    public string nextLevel;
    	
	public void callNextLevel () {
        Application.LoadLevel (nextLevel);
    }
}
