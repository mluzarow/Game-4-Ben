using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {
	public GameObject bullet;
	public float x = 0f;
	public float y = 0f;
	Vector2 distanceApart;
	bool fire = false;

	public GameObject player;

	void Start() {
		//StartCoroutine (wait (5f));
	}

	void Update() {
		distanceApart.x = Mathf.Abs (player.transform.position.x) - Mathf.Abs (this.transform.position.x);
		distanceApart.y = Mathf.Abs (player.transform.position.y) - Mathf.Abs (this.transform.position.y);

		if (distanceApart.x <= 20f || distanceApart.y <= 20f) {
			fire = true;
		}
	}

	public void spawnBullet() {
		Instantiate (bullet, new Vector3 (0f, 0f, 0f), Quaternion.identity
		             /*Quaternion.Euler(new Vector3(0f, 0f, Mathf.Cos (distanceApart.x / Mathf.Sqrt(Mathf.Pow (distanceApart.x, 2f) + Mathf.Pow (distanceApart.y, 2f)))))*/);

		//wait 5 seconds
		//StartCoroutine (wait ());
	}



	void OnDrawGizmos() {
		Gizmos.color = Color.blue; //Gizmo color
		float size = 0.3f; //Gizmo size
		
		//Draw gizmos for the start and end positions of the door teleports
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.up * size, (Vector2)transform.position + Vector2.up * size);
		Gizmos.DrawLine ((Vector2)transform.position - Vector2.right * size, (Vector2)transform.position + Vector2.right * size);
	}
}
