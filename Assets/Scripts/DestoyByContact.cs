using UnityEngine;
using System.Collections;

public class DestoyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
	}

	void OnTriggerEnter(Collider other){
//		Debug.Log ("Inside destroy by contact me: " + gameObject.name + ", other: " + other.name);
		if(other.CompareTag("Boundary") || other.CompareTag("Enemy"))
			return;
		
		if(explosion != null)
			Instantiate (explosion, transform.position, transform.rotation);

        if (other.tag == "Player") {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
			
		
		gameController.AddScore (scoreValue);

		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
