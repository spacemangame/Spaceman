using UnityEngine;
using System.Collections;

public class DestoyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int hpValue;
	private GameController gameController;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
	}

	void OnTriggerEnter(Collider other){
		//        Debug.Log ("Inside destroy by contact me: " + gameObject.name + ", other: " + other.name);
		if(other.CompareTag("Boundary"))
			return;

		if (other.tag == "Player") {
			if (gameController.getHP () == 10) {
				gameController.AddScore (scoreValue);
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				gameController.DecreaseHP (hpValue);
				gameController.GameOver ();
				Destroy (other.gameObject);
				return;
			} else {
				gameController.DecreaseHP (hpValue);
				Instantiate (explosion, transform.position, transform.rotation);
				Destroy (gameObject);
			}
		}

		if (other.tag == "bolt") {
			Instantiate (explosion, transform.position, transform.rotation);
			gameController.AddScore (scoreValue);
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}