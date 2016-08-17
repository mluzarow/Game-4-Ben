using UnityEngine;
using System.Collections;

public class AIDecisions : MonoBehaviour {
	public enum AIcommand : sbyte {
		Left = 1, Right = 2, Up = 3, Down = 4,
		Jump = 5, 
		Action = 6
	} 

	public Stack commanStack;

	// Use this for initialization
	void Start () {
		commanStack = new Stack ();
	}

	private Stack testroutine () {
		Stack tempStack = new Stack ();


		return (tempStack);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
