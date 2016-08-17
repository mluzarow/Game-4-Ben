using UnityEngine;
using System.Collections;

public class playerCamera : MonoBehaviour {
	public playerController target;
	[HideInInspector]
	public Vector2 focusAreaSize;

	public float verticalOffset;
	public float lookAheadDistX;
	public float lookSmoothTimeX;
	public float smoothTimeY;
	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	FocusArea focusArea;

	IEnumerator wait(float sec) {
		yield return new WaitForSeconds (sec);
	}

	void Start() {
		wait (0.1f);
		focusArea = new FocusArea (target.GetComponent<Collider2D>().bounds, focusAreaSize);
		focusAreaSize = new Vector2 (3, 5);

	}

	void LateUpdate() {
		//Focus area collision
		focusArea.Update (target.GetComponent<Collider2D>().bounds);

		//Camera movement
		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
		//If focus area is being pushed, find the direction
		if (focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
		}
		//Find direction and distance of look target
		targetLookAheadX = lookAheadDirX * lookAheadDistX;
		//Damp moving of camera to target
		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, 
		                                     ref smoothLookVelocityX, lookSmoothTimeX);
		//Calc camera movement
		focusPosition += Vector2.right * currentLookAheadX;
		//Move camera while keeping it -10 units away from the level on the z axis
		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, 0.5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);
	}

	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left;
		float right;
		float top;
		float bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;

			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}

			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			
			top += shiftY;
			bottom += shiftY;

			center = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2 (shiftX, shiftY);
		}
	}
}