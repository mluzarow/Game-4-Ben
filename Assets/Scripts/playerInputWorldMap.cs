using UnityEngine;
using System.Collections;

public class playerInputWorldMap : MonoBehaviour {
	WorldNode currentNode;
	MoveFlag flags;
	float dist2dest = 0f;
	const float moveSpeed = 4f;
	public Texture2D bg;
	public Texture2D player;
	Rect playerSize;

	void Start () {
		WorldNode W01_L01 = new WorldNode ("Level One", "platformScene", 0f, 0f, 0f, 1.9f, null, null, null, null);
		WorldNode W01_L02 = new WorldNode ("Level Two", "menuMain", 0f, 0f, 1.9f, 2f, null, null, null, null);
		WorldNode W01_L03 = new WorldNode ("Level Three", "menuMain", 0f, 0f, 2f, 0f, null, null, null, null);
		W01_L01.nodeRight = W01_L02;
		W01_L02.nodeLeft = W01_L01;
		W01_L02.nodeRight = W01_L03;
		W01_L03.nodeLeft = W01_L02;
		currentNode = W01_L01;
		flags.reset ();

		playerSize = new Rect (30f, 10f, 30f, 30f);

	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);
		GUI.DrawTexture (playerSize, player);
	}

	// Update is called once per frame
	void Update () {
		//Get raw direction - Keyboard returns 1, 0, or -1 each axis
		Vector2 input = new Vector2 ((Input.GetKeyDown (KeyCode.A) ? -1 : 0) 
		                             + (Input.GetKeyDown (KeyCode.D) ? 1 : 0),
		                             (Input.GetKeyDown (KeyCode.W) ? 1 : 0) 
		                             + (Input.GetKeyDown (KeyCode.S) ? -1 : 0));
		//On eneter key
		if (Input.GetKey (KeyCode.Return)) {
			Application.LoadLevel(currentNode.nodeLevel);
		}
		//Player can input movements
		if (flags.moveFlag) {
			//Player is going left; check if node exists on left
			if ((input.x < 0) && (currentNode.nodeLeft != null)) {
				if (currentNode.nodeLeft != null) {
					flags.moveFlagLeft = true;
					flags.moveFlag = false;

					dist2dest = currentNode.distanceLeft;
					currentNode = currentNode.nodeLeft;
				}
			//Player is going right; check if node exists on right
			} else if ((input.x > 0) && (currentNode.nodeRight != null)) {
				if (currentNode.distanceRight != 0) {
					flags.moveFlagRight = true;
					flags.moveFlag = false;

					dist2dest = currentNode.distanceRight;
					currentNode = currentNode.nodeRight;
				}
			//Player is going down; check if node exists below
			} else if ((input.y < 0) && (currentNode.nodeDown != null)) {
				if (currentNode.distanceDown != 0) {
					flags.moveFlagDown = true;
					flags.moveFlag = false;

					dist2dest = currentNode.distanceDown;
					currentNode = currentNode.nodeDown;
				}
			//Player is going up; check if node exists above
			} else if ((input.y > 0) && (currentNode.nodeUp != null)) {
				if (currentNode.distanceUp != 0) {
					flags.moveFlagUp = true;
					flags.moveFlag = false;
					
					dist2dest = currentNode.distanceUp;
					currentNode = currentNode.nodeUp;
				}
			}
		//Player is now moving and inputs are locked
		} else {
			//Move player to left node
			if (flags.moveFlagLeft) {
				dist2dest -= moveSpeed * Time.deltaTime;

				if (dist2dest > 0) {
					transform.Translate(-moveSpeed * Time.deltaTime, 0f, 0f);
				} else {
					flags.reset ();
				}
			//Move player to right node
			} else if (flags.moveFlagRight) {
				dist2dest -= moveSpeed * Time.deltaTime;
				
				if (dist2dest > 0) {
					transform.Translate(moveSpeed * Time.deltaTime, 0f, 0f);
				} else {
					flags.reset ();
				}
			//Move player to bottom node
			} else if (flags.moveFlagDown) {
				dist2dest -= moveSpeed * Time.deltaTime;
				
				if (dist2dest > 0) {
					transform.Translate(0f, -moveSpeed * Time.deltaTime, 0f);
				} else {
					flags.reset ();
				}
			//Move player to top node
			} else if (flags.moveFlagUp) {
				dist2dest -= moveSpeed * Time.deltaTime;
				
				if (dist2dest > 0) {
					transform.Translate(0f, moveSpeed * Time.deltaTime, 0f);
				} else {
					flags.reset ();
				}
			}
		}
	}

	struct MoveFlag {
		public bool moveFlag;
		public bool moveFlagLeft;
		public bool moveFlagRight;
		public bool moveFlagUp;
		public bool moveFlagDown;

		public void reset() {
			moveFlag = true;
			moveFlagLeft = false;
			moveFlagRight = false;
			moveFlagUp = false;
			moveFlagDown = false;
		}
	}

	class WorldNode {
		public string nodeName;
		public string nodeLevel;

		public float distanceUp;
		public float distanceDown;
		public float distanceLeft;
		public float distanceRight;
		
		public WorldNode nodeUp;
		public WorldNode nodeDown;
		public WorldNode nodeLeft;
		public WorldNode nodeRight;

		public WorldNode(string name, string level, float dUp, float dDown, float dLeft, float dRight, 
		                 WorldNode nUp, WorldNode nDown, WorldNode nLeft, WorldNode nRight) {
			nodeName = name;
			nodeLevel = level;
			distanceUp = dUp;
			distanceDown = dDown;
			distanceLeft = dLeft;
			distanceRight = dRight;
			nodeUp = nUp;
			nodeDown = nDown;
			nodeLeft = nLeft;
			nodeRight = nRight;
		}
	}
}
