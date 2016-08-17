using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Allows creation of hash set

public class platformController : raycastController {

	public Vector2 move;
	public LayerMask passengerMask;

	public Vector2[] waypoints;
	Vector2[] worldWaypoints;

	public float platformSpeed;
	int waypointIndex;
	float percentBetweenWaypoints;

	List<PassengerMovement> passengerMovement;
	//Reducing component calls
	Dictionary<Transform, playerController> passengerDisctionary = new Dictionary<Transform, playerController>();


	public override void Start () {
		base.Start ();

		worldWaypoints = new Vector2[waypoints.Length];

		for (int i = 0; i < waypoints.Length; i++) {
			worldWaypoints[i] = waypoints[i] + (Vector2)transform.position;
		}
	}

	void Update () {
		updateRaycasts ();

		Vector2 velocity = calcPlatformMovement();
		calcPassengerMovement (velocity);

		movePassanger (true);
		transform.Translate (velocity);
		movePassanger (false);
	}

	Vector2 calcPlatformMovement() {
		int nextWaypointIndex = waypointIndex + 1;
		float distanceBetweenWaypoints = Vector2.Distance (worldWaypoints [waypointIndex], worldWaypoints [nextWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * platformSpeed / distanceBetweenWaypoints;
	
		Vector2 newPosition = Vector2.Lerp (worldWaypoints [waypointIndex], worldWaypoints [nextWaypointIndex], percentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0f;
			waypointIndex++;

			//Reached the end of the array
			if (waypointIndex >= worldWaypoints.Length - 1) {
				waypointIndex = 0;
				System.Array.Reverse (worldWaypoints);
			}
		}

		return (newPosition - (Vector2)transform.position); 
	}

	void movePassanger(bool beforeMovePlatform) {
		foreach (PassengerMovement passenger in passengerMovement) {
			//Check if passenger does not yet exist in dictionary (only get 1 call per passenger)
			if (!passengerDisctionary.ContainsKey(passenger.transform)) {
				//Add passenger to dictionary
				passengerDisctionary.Add (passenger.transform, passenger.transform.GetComponent<playerController>());
			}

			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				passengerDisctionary[passenger.transform].Move (passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	void calcPassengerMovement(Vector2 velocity) {
		//Store all passengers moved on this frame
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		passengerMovement = new List<PassengerMovement> ();

		float directionX = Mathf.Sign (velocity.x);
		float directionY = Mathf.Sign (velocity.y);

		//vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + inset;

			for (int i = 0; i < verticalRays; i++) {
				//If directionY is negative (moving down), raycast from the bottom left
				//If directionY is positive (moving up), raycast from the top left
				Vector2 rayOrigin = (directionY == -1) ? raycasts.bottomLeft : raycasts.topLeft;
				//Add vertical ray spacing to raycast position
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				//Found a passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);
						//Give x velocity of platform to passanger if platform is moving up
						float pushX = (directionY == 1) ? velocity.x : 0f;
						//Pushing = platform's velocity - (detection gap - ray inset) in y dir
						float pushY = velocity.y - (hit.distance - inset) * directionY;

						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector2(pushX, pushY), directionY == 1, true));
						//hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
		//Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + inset;

			for (int i = 0; i < horizontalRays; i++) {
				//If directionX is negative (moving left), raycast from the bottom left
				//If directionX is positive (moving right), raycast from the bottom right
				Vector2 rayOrigin = (directionX == -1) ? raycasts.bottomLeft : raycasts.bottomRight;
				//Add horizontal ray spacing to raycast position
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				//Found passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);

						//Pushing = platform's velocity - (detection gap - ray inset) in x dir
						float pushX = velocity.x - (hit.distance - inset) * directionX;
						float pushY = 0.001f; //Tiny force to allow player to jump

						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector2(pushX, pushY), false, true));
						//hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
		//If passeger is on horizontally or downward moving platform
		if ((directionY == -1) || (velocity.y == 0 && velocity.x != 0)) {
			float rayLength = inset * 2;
			
			for (int i = 0; i < verticalRays; i++) {
				//Raycast from the top left
				Vector2 rayOrigin = raycasts.topLeft;
				//Add vertical ray spacing to raycast position
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
				
				//Found a passenger
				if (hit) {
					//If passenger is not in passengers list
					if (!movedPassengers.Contains(hit.transform)) {
						//Add passanger to passengers list
						movedPassengers.Add (hit.transform);
						//Give x velocity of platform to passanger if platform is moving up
						float pushX = velocity.x;
						//Pushing = platform's velocity in y dir
						float pushY = velocity.y;

						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));
						//hit.transform.Translate(new Vector2(pushX, pushY));
					}
				}
			}
		}
	}
	//Draw waypoint gizmos
	void OnDrawGizmos() {
		//Only draw if there are any waypoints
		if (waypoints != null) {
			Gizmos.color = Color.red; //Gizmo color
			float size = 0.3f; //Gizmo size

			//Go through each waypoint and convert local to global position
			for (int i = 0; i < waypoints.Length; i++) {
				Vector2 worldWaypointsPos = Application.isPlaying ? worldWaypoints[i] : waypoints[i] + (Vector2) transform.position;

				Gizmos.DrawLine (worldWaypointsPos - Vector2.up * size, worldWaypointsPos + Vector2.up * size);
				Gizmos.DrawLine (worldWaypointsPos - Vector2.right * size, worldWaypointsPos + Vector2.right * size);
			}
		}
	}

	struct PassengerMovement {
		public Transform transform;
		public Vector2 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector2 _velocity, bool _standingOnPlatform, 
		                         bool _moveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}
}
