using UnityEngine;
using System.Collections;

public class playerActions : MonoBehaviour {
	public GameObject healObject;
	public GameObject player;

	public void heal() {
		Instantiate (healObject, player.transform.position, Quaternion.identity);
	}

	public void dash() {

	}
}
