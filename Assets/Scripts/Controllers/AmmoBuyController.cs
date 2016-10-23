using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;


public class AmmoBuyController: MonoBehaviour
{

	public GameObject ContentPanel;
	public GameObject GunBuyListItemPrefab;

	public List<Gun> Guns { get; set; }

	void Start() {

		Guns = GameController.Instance.profile.guns;

		GameObject gunPrefabItem = (GameObject) Resources.Load("GunBuyListItem", typeof(GameObject));

		foreach(Gun gun in Guns) {

			GameObject gunItem = Instantiate(gunPrefabItem) as GameObject;
			AmmoBuyItemController controller = gunItem.GetComponent<AmmoBuyItemController>();

			Sprite image = Resources.Load<Sprite> ("Images/" + gun.texture);
			controller.Image.sprite = image;

			controller.gun = gun;
			controller.ammoBuyController = this;

			gunItem.transform.SetParent(ContentPanel.transform);
			gunItem.transform.localScale = Vector3.one;
		}
	}

	public void Buy(Gun gun) {
		GameController.Instance.profile.coins -= (5* gun.ammoPrice);
		List<Gun> UserGuns = GameController.Instance.profile.guns;
		foreach(Gun userGun in UserGuns) {
			if(userGun == gun) {
				gun.ammo += 5;
				break;
		}
		UserProfile.Save();
	}

}
}