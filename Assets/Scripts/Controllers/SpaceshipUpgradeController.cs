using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpaceshipUpgradeController : MonoBehaviour {	
	public Text spaceshipHP;
    public Text gunHP;
    public Text minMedals;
	public Text price;
	public Image SpaceshipImage;

	public Button upgradeButton;
	private Spaceship nextUpgradeSpaceship;

	void Start() {
		Spaceship currentSpaceship = GameController.Instance.profile.spaceship;
		int spaceshipCount = GameController.Instance.shop.spaceships.Count;
		for (int i = 0; i < spaceshipCount; i++) {
			nextUpgradeSpaceship = GameController.Instance.shop.spaceships [i];
			if (currentSpaceship.id == nextUpgradeSpaceship.id) {
				Spaceship toAssign = GameController.Instance.shop.spaceships [i + 1];
				if (toAssign != null) {
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

		if ((GameController.Instance.profile.coins < nextUpgradeSpaceship.price) || GameController.Instance.profile.medals < nextUpgradeSpaceship.minMedals) {
			//upgradeButton.GetComponent<Button> ().interactable = false;
			upgradeButton.gameObject.SetActive(false);
		}
	}	

	public void UpgradeSpaceship() {
			GameController.Instance.profile.coins -= nextUpgradeSpaceship.price;
			GameController.Instance.profile.spaceship = nextUpgradeSpaceship;
			UserProfile.Save();
	}
}
