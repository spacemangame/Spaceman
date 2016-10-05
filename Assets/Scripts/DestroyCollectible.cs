using UnityEngine;
using System.Collections;

public class DestroyCollectible : MonoBehaviour {

	private GameController gameController;
	public int pointValue;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Inside destroy collectible me: " + gameObject.name + ", other: " + other.name);
		if(other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("bolt"))
			return;
//		gameController.AddPoints (pointValue);
		Destroy(gameObject);
	}
}
