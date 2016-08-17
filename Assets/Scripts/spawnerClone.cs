using UnityEngine;
using System.Collections;

public class spawnerClone : MonoBehaviour {
	///<summary>The speed of the game object.</summary>
	public Vector2 velocity = new Vector2 (0f, 0f);
	///<summary>The distance the game object can travel before it is destroyed.</summary>
	//public Vector2 limits = new Vector2 (100f, 100f);

    public void setVelocity (Vector2 vec) {
        velocity = vec;
    }

	// Update is called once per frame
	void Update () {
		transform.Translate (velocity * Time.deltaTime);

		//if (Mathf.Abs(transform.position.x) > limits.x || Mathf.Abs (transform.position.y) > limits.y) {
		//	Destroy(gameObject);
		//}
	}
}
