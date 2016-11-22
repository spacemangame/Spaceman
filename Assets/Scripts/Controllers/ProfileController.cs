using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ProfileController : MonoBehaviour {
    public Text spaceshipHP;
	public Text gunHP;
	public Text medals;
    public Text coins;	

	public InputField coinInput;
	public InputField medalInput;
	public GameObject inputsCanvas;
	public GameObject spaceship;


	void Start() {
        spaceshipHP.text = GameController.Instance.profile.spaceship.hp.ToString();
        gunHP.text = GameController.Instance.profile.spaceship.primaryGun.hitPoint.ToString();
        medals.text = GameController.Instance.profile.medals.ToString();
        coins.text = GameController.Instance.profile.coins.ToString();
    }	


	public void showInputs() {
		coinInput.text = GameController.Instance.profile.coins.ToString();
		medalInput.text = GameController.Instance.profile.medals.ToString();
		inputsCanvas.SetActive (true);
		spaceship.SetActive (false);
	}

	public void submitInputs() {
		int newCoin = int.Parse(coinInput.text);
		int newMedal = int.Parse (medalInput.text);

		GameController.Instance.profile.medals = newMedal;
		GameController.Instance.profile.coins = newCoin;
		UserProfile.Save ();


		medals.text = newMedal.ToString();
		coins.text = newCoin.ToString();

		inputsCanvas.SetActive (false);
		spaceship.SetActive (true);
	}

	public void reset() {
		GameController.Instance.profile = DataGenerator.PopulateUserProfile ();
		UserProfile.Save ();

		SceneManager.LoadScene ("Main Menu");
	}
}
