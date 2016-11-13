using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GunBuyItemController: MonoBehaviour
{
	public Image Image;
	public Text HPText;
	public Text AmmoText;
	public Text Name;
	public Text Price;
	public Button Buy;

	public Gun gun { get; set; }

	public GunBuyController gunBuyController  { get; set; }

	void Start() {
		
	}

	public void Render() {
		Name.text = gun.name;
		HPText.text = "Damage : " + gun.hitPoint;
		AmmoText.text = "Ammo : " + gun.ammo;
		Price.text = "Buy for " + gun.price;

		if (gun.price > GameController.Instance.profile.coins) {
			Buy.interactable = false;
		}

		Buy.onClick.AddListener (() => OnBuySelect ());
	}

	public void OnBuySelect() {
		gunBuyController.Buy (gun);
	}
}
