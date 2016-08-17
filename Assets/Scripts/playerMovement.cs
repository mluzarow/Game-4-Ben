using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {
	float baseSpeed = 2f;
	float distToGround;
	//bool isGrounded = false;
	float jumpTotalForce = 0f;
	Vector2 playerPosition;
	Vector2 groundPosition;
	bool isJumping = false;
	const float jumpMaxTime = 2f;
	const float jumpMaxForce = 6f;
	const float jumpMinForce = 2f;

	void Start() {
	}

	void Update() {
		playerPosition = new Vector2 (transform.position.x, transform.position.y);
		groundPosition = new Vector2 (transform.position.x, transform.position.y - 0.1f);
		if (isGrounded()) {
			if (Input.GetKeyDown(KeyCode.W)) {
				jumpTotalForce = 0f;
				jump(jumpMinForce, jumpTotalForce);
				isJumping = true;
			}
		}
		if (isJumping) {
			if (Input.GetKey (KeyCode.W)) {
				jumpTotalForce += Time.deltaTime * 20f;
				jump (Time.deltaTime * 20f, jumpTotalForce);
			}
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			isJumping = false;
		}

	}

	void FixedUpdate () {
		if (Input.GetKey (KeyCode.D)) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(baseSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (Input.GetKey (KeyCode.A)) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(-baseSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}
		//if (Input.GetKey (KeyCode.W) && isGrounded()) {
		//	rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, baseSpeed * 4f);
		//}
	}

	bool isGrounded() {
		bool check = Physics2D.Linecast (playerPosition, groundPosition, 
		                                 1 << LayerMask.NameToLayer ("ground"));
		playerPosition.x -= .05f;
		bool checkL = Physics2D.Linecast (playerPosition, groundPosition, 
		                                 1 << LayerMask.NameToLayer("ground"));
		if (check || checkL) {
			Debug.Log ("GROUNDED");
			Debug.DrawLine(playerPosition, groundPosition , Color.green, 20f, false);
		} else {
			Debug.Log ("NOT GROUNDED");
			Debug.DrawLine(playerPosition, groundPosition , Color.blue, 20f, false);
		}

		return (check);
	}

	void jump(float force, float total) {

		if (total > jumpMaxForce) {
			force = 0f;
		}
		Vector2 jumpVec = new Vector2 (0f, force); 

		GetComponent<Rigidbody2D>().AddForce (jumpVec, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		/*if (collider.gameObject.tag == "ground") {
			isGrounded = true;
			//Debug.Log ("Player is grounded.");
		} else */if (collider.gameObject.tag == "spike") {
			transform.position = new Vector2(1f, 1f);
		}
	}
	/*void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "ground") {
			isGrounded = false;
			//Debug.Log ("Player is NOT grounded.");
		}
	}*/
}
