using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        AudioSource audio = GetComponent <AudioSource> ();
        audio.Play ();
        yield return new WaitForSeconds(audio.clip.length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
