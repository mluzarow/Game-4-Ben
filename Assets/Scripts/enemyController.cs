using UnityEngine;
using System.Collections;

public class enemyController : raycastController {
	public LayerMask collisionMask;
	public collisionInfo collisions;

	// Use this for initialization
	public override void Start () {
		base.Start ();

		collisions.directionFacing = 1;
	}

	/// <summary>
	/// Move the specified velocity.
	/// </summary>
	public void move (Vector2 velocity) {
		updateRaycasts ();
		collisions.reset ();

		//If moving on x-axis, update position facing
		if (velocity.x != 0) {
			collisions.directionFacing = (int)Mathf.Sign(velocity.x);

			//velocity in the x plane is NOT 0; check for collisions
			horizontalCollisions (ref velocity);
		}

		//velocity in the y plane is NOT 0; check for collisions
		if (velocity.y != 0) {
			verticalCollisions (ref velocity);
		}
		
		//Move player with updated velocity value
		transform.Translate (velocity);
	}

	/// <summary>
	/// Detects whether or not each of the horizontal rays, projected from the game object in the direction that the game object has last moved in, hit anything.  
	/// If there is a hit, a new velocity is calculated and collision flags set.
	/// </summary>
	private void horizontalCollisions(ref Vector2 velocity) {
		float directionX = collisions.directionFacing;
		float rayLength = Mathf.Abs (velocity.x) + inset;

		for (int i = 0; i < horizontalRays; i++) {
			//If directionX is negative (moving left), raycast from the bottom left
			//If directionX is positive (moving right), raycast from the bottom right
			Vector2 rayOrigin = (directionX == -1)?raycasts.bottomLeft:raycasts.bottomRight;
			//Add horizontal ray spacing to raycast position
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				if (hit.distance == 0) {
					continue;
				}
				
				velocity.x = (hit.distance - inset) * directionX;
				rayLength = hit.distance;
				
				//Set collision flag for collision left or right
				collisions.left = (directionX == -1);
				collisions.right = (directionX == 1);
			}

		}
	}

	/// <summary>
	/// Detects whether or not each of the vertical rays, projected from the game object in the direction that the game object has last moved in, hit anything.  
	/// If there is a hit, a new velocity is calculated and collision flags set.
	/// </summary>
	private void verticalCollisions(ref Vector2 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + inset;
		
		for (int i = 0; i < verticalRays; i++) {
			//If directionY is negative (moving left), raycast from the bottom left
			//If directionY is positive (moving right), raycast from the top left
			Vector2 rayOrigin = (directionY == -1)?raycasts.bottomLeft:raycasts.topLeft;
			//Add vertical ray spacing to raycast position
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);
			
			if (hit) {
				velocity.y = (hit.distance - inset) * directionY;
				rayLength = hit.distance;
				
				//Set collision flag for collision up or down
				collisions.up = (directionY == 1);
				collisions.down = (directionY == -1);
			}
			
		}
	}

	///<summary>
	/// Stores data on which direction the game object is currectly colliding in and in which direction it is facing.
	///</summary>
	public struct collisionInfo {
		/// <summary>
		/// Collision flag for collision detected immediately above the game object.
		/// </summary>
		public bool up;
		/// <summary>
		/// Collision flag for collision detected immediately below the game object.
		/// </summary>
		public bool down;
		/// <summary>
		/// Collision flag for collision detected immediately left of the game object.
		/// </summary>
		public bool left;
		/// <summary>
		/// Collision flag for collision detected immediately right of the game object.
		/// </summary>
		public bool right;
		public int directionFacing;
		public bool kill;

		/// <summary>
		/// Resets collision in all directions to false. The direction facing will not be altered.
		/// </summary>
		public void reset() {
			up = false;
			down = false;
			left = false;
			right = false;
			kill = false;
		}
	}
}