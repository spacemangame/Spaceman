using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpaceshipUpgradeController : MonoBehaviour {	
	public Text spaceshipHP;
    public Text gunHP;
    public Text minMedals;
	public Text price;
	public Text disableReason;
	public Image SpaceshipImage;

	public Button upgradeButton;
	private Spaceship nextUpgradeSpaceship;

	void Start() {
		Render ();
	}	

	public void Render() {
		Spaceship currentSpaceship = GameController.Instance.profile.spaceship;
		int spaceshipCount = GameController.Instance.shop.spaceships.Count;

		for (int i = 0; i < spaceshipCount; i++) {
			nextUpgradeSpaceship = GameController.Instance.shop.spaceships [i];
			if (currentSpaceship.id == nextUpgradeSpaceship.id) {
				if (i+1 < spaceshipCount ) {
					nextUpgradeSpaceship = GameController.Instance.shop.spaceships [i + 1];
					break;
				} else {
					upgradeButton.GetComponentInChildren<Text>().text = "No more updates";
					upgradeButton.GetComponent<Button>().interactable = false;
				}
			}
		}

		spaceshipHP.text =  nextUpgradeSpaceship.hp.ToString();
		gunHP.text = nextUpgradeSpaceship.primaryGun.hitPoint.ToString();
		minMedals.text = nextUpgradeSpaceship.minMedals.ToString();
		price.text = nextUpgradeSpaceship.price.ToString();


		Sprite image = Resources.Load<Sprite> ("Images/" + nextUpgradeSpaceship.texture);
		SpaceshipImage.sprite = image;

		if (GameController.Instance.profile.medals < nextUpgradeSpaceship.minMedals) {
			disableReason.gameObject.SetActive (true);
			disableReason.text = Strings.needMoreMedal;
			upgradeButton.interactable = false;

		} else if (GameController.Instance.profile.coins < nextUpgradeSpaceship.price) {
			disableReason.gameObject.SetActive (true);
			disableReason.text = Strings.needMoreCoin;
			upgradeButton.interactable = false;

		} else {
			disableReason.gameObject.SetActive (false);
		}
	}

	public void UpgradeSpaceship() {
		GameController.Instance.profile.coins -= nextUpgradeSpaceship.price;
		GameController.Instance.profile.spaceship = nextUpgradeSpaceship;
		UserProfile.Save();
		GlobalPointController.Instance.Render ();
		Render ();
	}
}
