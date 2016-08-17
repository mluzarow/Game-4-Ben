using UnityEngine;
using System.Collections;

public class playerController : raycastController {
	public collisionInfo collisions;
	DeathInfo deathInfo;

	/// <summary>
	/// The collision mask for colliding with the ground layer.</summary>
	public LayerMask collisionMask;
	///<summary>The kill mask for colliding with the danger layer.</summary>
	public LayerMask killMask;
	public LayerMask waypoint1Mask;
	public LayerMask waypoint2Mask;
	public LayerMask waypoint3Mask;
    public LayerMask endLevelMask;
    public string nextLevel;

    public Vector2 respawnPoint1;
    public Vector2 respawnPoint2;
    public Vector2 respawnPoint3;

	public override void Start() {//Must be override to use base start method
		base.Start ();

		collisions.directionFacing = 1;
		deathInfo.recentWaypoint = new Vector2 (0f, 0f);
	}

	public void Move(Vector2 velocity, bool standingOnPlatform) {
		updateRaycasts ();
		collisions.reset ();

		if (velocity.x != 0) {
			collisions.directionFacing = (int)Mathf.Sign(velocity.x);
		}

		//velocity in the x plane is NOT 0; check for collisions
		horizontalCollisions (ref velocity);
		
		//velocity in the y plane is NOT 0; check for collisions
		if (velocity.y != 0) {
			verticalCollisions (ref velocity);
		}

		//Move player with updated velocity value
		transform.Translate (velocity);

		if (standingOnPlatform) {
			collisions.down = true;
		}
	}

	//velocity in the x plane is NOT 0; check for collisions
	void horizontalCollisions(ref Vector2 velocity) {
		float directionX = collisions.directionFacing;
		float rayLength = Mathf.Abs (velocity.x) + inset;

		//Extend rays to detect walls while not moving towards them
		if (Mathf.Abs (velocity.x) < inset) {
			rayLength = 2 * inset;
		}

		for (int i = 0; i < horizontalRays; i++) {
			//If directionX is negative (moving left), raycast from the bottom left
			//If directionX is positive (moving right), raycast from the bottom right
			Vector2 rayOrigin = (directionX == -1)?raycasts.bottomLeft:raycasts.bottomRight;
			//Add horizontal ray spacing to raycast position
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D kill = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, killMask);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			RaycastHit2D waypoint1 = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, waypoint1Mask);
			RaycastHit2D waypoint2 = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, waypoint2Mask);
			RaycastHit2D waypoint3 = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, waypoint3Mask);
            RaycastHit2D endLevel = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, endLevelMask);

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (kill) {
				collisions.reset ();
				respawn (ref velocity, deathInfo.recentWaypoint);
			} else if (hit) {
				if (hit.distance == 0) {
					continue;
				}
			
				velocity.x = (hit.distance - inset) * directionX;
				rayLength = hit.distance;

				//Set collision flag for collision left or right
				collisions.left = (directionX == -1);
				collisions.right = (directionX == 1);
			}
			if (waypoint1) {
                deathInfo.recentWaypoint = respawnPoint1;
			} else if (waypoint2) {
				deathInfo.recentWaypoint = respawnPoint2;
			} else if (waypoint3) {
				deathInfo.recentWaypoint = respawnPoint3;
			} else if (endLevel) {
                Application.LoadLevel(nextLevel);
            }
		}
	}

	void respawn(ref Vector2 velocity, Vector2 location) {
		transform.position = location;
		velocity = new Vector2 (0f, 0f);
	}

	void verticalCollisions(ref Vector2 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + inset;

		for (int i = 0; i < verticalRays; i++) {
			Vector2 rayOrigin = (directionY == -1)?raycasts.bottomLeft:raycasts.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			RaycastHit2D kill = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, killMask);
			RaycastHit2D waypoint1 = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, waypoint1Mask);
			RaycastHit2D waypoint2 = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, waypoint2Mask);
			RaycastHit2D waypoint3 = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, waypoint3Mask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (kill) {
				collisions.reset ();
				respawn (ref velocity, deathInfo.recentWaypoint);
			} else if (hit) {
				velocity.y = (hit.distance - inset) * directionY;
				rayLength = hit.distance;

				//Set collision flag for collision up or down
				collisions.up = (directionY == 1);
				collisions.down = (directionY == -1);
			}
			if (waypoint1) {
				deathInfo.recentWaypoint = respawnPoint1;
			} else if (waypoint2) {
				deathInfo.recentWaypoint = respawnPoint2;
			} else if (waypoint3) {
				deathInfo.recentWaypoint = respawnPoint3;
			}
		}
	}

	struct DeathInfo {
		public Vector2 recentWaypoint;
	}

	//struct for telling which direction you are colliding in
	public struct collisionInfo {
		public bool up;
		public bool down;
		public bool left;
		public bool right;
		public int directionFacing;
		public bool kill;

		public void reset() {
			up = false;
			down = false;
			left = false;
			right = false;
			kill = false;
		}
	}
}