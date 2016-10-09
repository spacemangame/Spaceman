using UnityEngine;
using System.Collections;

public class DestroyCollectible : MonoBehaviour {

	private MissionController missionController;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("MissionController");
		if (gameControllerObject != null) {
            missionController = gameControllerObject.GetComponent<MissionController>();
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("bolt"))
			return;
//		if (!other.CompareTag("Player")) {
//			return;
//		}

		Collectible collectible = Helper.getCollectibleFromGameObject (gameObject);
		missionController.AddPoints(collectible.value);

		Destroy(gameObject);
	}
}
