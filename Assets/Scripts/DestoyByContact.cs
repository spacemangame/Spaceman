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
		if(other.CompareTag("Boundary"))
			return;

		if (other.tag == "Player") {
			if (missionController.getHP () == 10) {
				missionController.AddScore (scoreValue);
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				missionController.DecreaseHP (hpValue);
				missionController.GameOver ();
				Destroy (other.gameObject);
				return;
			} else {
				missionController.DecreaseHP (hpValue);
				Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			}

			//change the color of player
			ChangeColor (other.gameObject, (float)missionController.getHP(), (float)GameController.Instance.profile.spaceship.hp);
		}

		if (other.tag == "bolt") {
			Instantiate (explosion, transform.position, transform.rotation);
			missionController.AddScore (scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

	private void ChangeColor(GameObject gm, float currentHp, float maxHp) {
		float newColor = currentHp / maxHp;
		gm.GetComponentInChildren<Renderer> ().material.color = new Color(newColor + (float)100/255, newColor, newColor);
	}
}