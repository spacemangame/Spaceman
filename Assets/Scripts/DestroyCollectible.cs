using UnityEngine;
using System.Collections;

public class DestroyCollectible : MonoBehaviour {

	private MissionController missionController;
	private Mission mission;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("MissionController");
		if (gameControllerObject != null) {
            missionController = gameControllerObject.GetComponent<MissionController>();
		}
		mission = GameController.Instance.mission;
	}

	void OnTriggerEnter(Collider other) {
		if(!other.CompareTag("Player"))
			return;

		Collectible collectible = Helper.getCollectibleFromGameObject (gameObject);
		missionController.AddPoints(collectible.value);

		// if the collectible is item, addItem
		if (mission.item.GetType ().Name == collectible.GetType ().Name) {
			missionController.AddItem ();
		}

		Destroy(gameObject);
	}
}
