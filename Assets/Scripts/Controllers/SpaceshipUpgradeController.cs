using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpaceshipUpgradeController : MonoBehaviour {	
	public Text spaceshipHP;
    public Text gunHP;
    public Text minMedals;
	public Text price;

	void Start() {

        spaceshipHP.text =  GameController.Instance.shop.spaceships[1].hp.ToString();
        gunHP.text = GameController.Instance.shop.spaceships[1].primaryGun.hitPoint.ToString();
        minMedals.text = GameController.Instance.shop.spaceships[1].minMedals.ToString();
        price.text = GameController.Instance.shop.spaceships[1].price.ToString();
	}	
}
