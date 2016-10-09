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
		if (!other.CompareTag("Player")) {
			return;
		}

		Collectible collectible = Helper.getCollectibleFromGameObject (gameObject);
		missionController.AddPoints(collectible.value);

		Destroy(gameObject);
	}
}
