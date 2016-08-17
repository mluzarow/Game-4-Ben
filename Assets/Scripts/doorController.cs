using UnityEngine;
using System.Collections;

public class doorController : raycastController {
	public Vector2 endLocation;
	public LayerMask playerMask;
	public GameObject player;
	
	bool foundPlayer;

	public override void Start() {
		//Handle raycast initialization
		base.Start ();
		//Bind raycasts to object
		horizontalRays = 2;
		verticalRays = 2;

		updateRaycasts ();

		//Make sure the found player flag starts at false
		foundPlayer = false;
	}

	void Update () {
		//Found the player in the correct position
		if (checkDoor ()) {
			//Up was pressed by player
			if (playerInput.input.y == 1) {
				//Activate door
				useDoor();
			}
		}
	}

	//Activate door
	void useDoor() {
		//Move player to endLocation
		player.transform.position = endLocation;
	}

	//Checks if the player is positioned correctly to use the door
	bool checkDoor() {
		float rayLength = 2 * inset;

		//For every one of the 8 rays, generate and process ray
		for (int i = 0; i < horizontalRays; i++) {
			//Start one ray set on the bottom left, another on the bottom right
			Vector2 rayLeft = raycasts.bottomLeft;
			Vector2 rayRight = raycasts.bottomRight;

			//Raycasts form start positon, up by ray spacing each iteration for new ray
			rayLeft += Vector2.up * (horizontalRaySpacing * i);
			rayRight += Vector2.up * (horizontalRaySpacing * i);

			//Check if the rays hit player
			RaycastHit2D hitLeft = Physics2D.Raycast (rayLeft, Vector2.right * -1, rayLength, playerMask);
			RaycastHit2D hitRight = Physics2D.Raycast (rayRight, Vector2.right * 1, rayLength, playerMask);
		
			//Debug.DrawRay (rayLeft, Vector2.right * -1 * rayLength, Color.red);
			//Debug.DrawRay (rayRight, Vector2.right * 1 * rayLength, Color.red);

			//Player is detecting on both sides of the door object
			if (hitLeft && hitRight) {
				foundPlayer = true;
				//Break ray generator loop with foundPlayer = true
				return(foundPlayer);
			//Player is not near the door or on only one side
			}  else {
				foundPlayer = false;
			}
		}
		return(foundPlayer);
	}

	//Draw waypoint gizmos
	void OnDrawGizmos() {
		//Only draw if there are any waypoints
		if (endLocation != null) {
			Gizmos.color = Color.green; //Gizmo color
			float size = 0.3f; //Gizmo size
			
			//Draw gizmos for the start and end positions of the door teleports
			Gizmos.DrawLine ((Vector2)transform.position - Vector2.up * size, (Vector2)transform.position + Vector2.up * size);
			Gizmos.DrawLine ((Vector2)transform.position - Vector2.right * size, (Vector2)transform.position + Vector2.right * size);
			Gizmos.DrawLine (endLocation - Vector2.up * size, endLocation + Vector2.up * size);
			Gizmos.DrawLine (endLocation - Vector2.right * size, endLocation + Vector2.right * size);
		}
	}
}
