using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ProfileController : MonoBehaviour {
    public Text spaceshipHP;
	public Text gunHP;
	public Text medals;
    public Text coins;	

	void Start() {
        spaceshipHP.text = GameController.Instance.profile.spaceship.hp.ToString();
        gunHP.text = GameController.Instance.profile.spaceship.primaryGun.hitPoint.ToString();
        medals.text = GameController.Instance.profile.medals.ToString();
        coins.text = GameController.Instance.profile.coins.ToString();
    }	
}
