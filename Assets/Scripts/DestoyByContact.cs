using UnityEngine;
using System.Collections;

public class DestoyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int hpValue;
	private MissionController missionController;

	void Start(){
        GameObject gameControllerObject = GameObject.FindWithTag("MissionController");
		if (gameControllerObject != null) {
            missionController = gameControllerObject.GetComponent<MissionController>();
		}
	}

	void OnTriggerEnter(Collider other){
		//        Debug.Log ("Inside destroy by contact me: " + gameObject.name + ", other: " + other.name);
		if(other.CompareTag("Boundary"))
			return;

		if (other.tag == "Player") {

			if (gameObject.tag == "enemybolt") {
				hpValue = GameController.Instance.mission.enemyGunHP;
			} else {
				Obstacle obstacle = Helper.getObstacleFromGameObject (gameObject);

				hpValue = obstacle.currentHp;
			}
			missionController.DecreaseHP (hpValue);
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);

			if (missionController.getHP () <= 0) {
				missionController.AddScore (scoreValue);
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				missionController.GameOver ();
				Destroy (other.gameObject);
				return;
			} 
		}

		if (other.tag == "bolt") {
			Instantiate (explosion, transform.position, transform.rotation);
			missionController.AddScore (scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}

	}
}