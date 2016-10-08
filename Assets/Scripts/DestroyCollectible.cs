using UnityEngine;
using System.Collections;

public class DestroyCollectible : MonoBehaviour {

	private MissionController missionController;
	public int pointValue;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("MissionController");
		if (gameControllerObject != null) {
            missionController = gameControllerObject.GetComponent<MissionController>();
		}
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Inside destroy collectible me: " + gameObject.name + ", other: " + other.name);
		if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("bolt"))
			return;

		Collectible collectible = Helper.getCollectibleFromGameObject (gameObject);
		missionController.AddPoints(collectible.value);

		Destroy(gameObject);
	}
}
