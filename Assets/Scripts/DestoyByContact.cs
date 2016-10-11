using UnityEngine;
using System.Collections;

public class DestoyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	private MissionController missionController;

	void Start(){
        GameObject gameControllerObject = GameObject.FindWithTag("MissionController");
		if (gameControllerObject != null) {
            missionController = gameControllerObject.GetComponent<MissionController>();
		}
	}

	/**
	* Type of collissions:
	*	1. Player
	* 		1.1 with enemybolt -> destroy enemybolt & decrease player hp
	*		1.2 with alienship -> destroy alienship & decrease player hp & decrease item count
	*		1.3 with asteroid -> destroy asteroid & decrease player hp
	*	2. Obstacle
	*		2.1 with bolt -> decrease obstacle hp
	*
	**/
	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Boundary"))
			return;

		if (other.tag == "Player") {
			int hpValue;
			if (gameObject.tag == "enemybolt") { // 1.1
				hpValue = GameController.Instance.mission.enemyGunHP;
			} else {
				Obstacle obstacle = Helper.getObstacleFromGameObject (gameObject);
				hpValue = obstacle.currentHp; // 1.2 & 1.3

				if (obstacle is Alien) { // 1.2
					missionController.DecreaseItem();
				}
			}
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);

			missionController.DecreaseHP (hpValue);
			ChangeColor (other.gameObject, (float)missionController.getHP(), (float)GameController.Instance.profile.spaceship.hp);

			if (missionController.getHP () <= 0 || (missionController.mission.type == Constant.Transport && missionController.getItemCount() <= 0)) {
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				missionController.GameOver ();
				Destroy (other.gameObject);
				return;
			} 

		}

		if (other.tag == "bolt") { // 2.1
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (other.gameObject);

			Obstacle obstacle = Helper.getObstacleFromGameObject (gameObject);
			int newHP = obstacle.currentHp - missionController.activeGun.hitPoint;
			ChangeColor (gameObject, (float)obstacle.currentHp, (float)newHP);
			obstacle.currentHp = newHP;
			if (obstacle.currentHp <= 0) {
				Destroy (gameObject);
			}
		}

	}

	private void ChangeColor(GameObject gm, float currentHp, float maxHp) {
		float newColor = currentHp / maxHp;
		gm.GetComponentInChildren<Renderer> ().material.color = new Color(newColor + (float)100/255, newColor, newColor);
	}
}