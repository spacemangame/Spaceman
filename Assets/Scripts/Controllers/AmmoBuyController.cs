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
		
	}

	public void ClearContent() {
		foreach (Transform child in ContentPanel.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}


	public void Render() {

		ClearContent ();

		Guns = GameController.Instance.profile.guns;

		GameObject AmmoPrefabItem = (GameObject) Resources.Load("AmmoBuyListItem", typeof(GameObject));

		foreach(Gun gun in Guns) {

			GameObject ammoItem = Instantiate(AmmoPrefabItem) as GameObject;

			AmmoBuyItemController controller = ammoItem.GetComponent<AmmoBuyItemController>();

			Sprite image = Resources.Load<Sprite> ("Images/" + gun.texture);
			controller.Image.sprite = image;

			controller.gun = gun;
			controller.ammoBuyController = this;

			controller.Render ();

			ammoItem.transform.SetParent(ContentPanel.transform);
			ammoItem.transform.localScale = Vector3.one;

			Vector3 position = new Vector3 (ammoItem.transform.localPosition.x, ammoItem.transform.localPosition.y, 0.0f);
			ammoItem.transform.localPosition = position;
		}
	}

	public void Buy(Gun gun) {
		GameController.Instance.profile.coins -= (5* gun.ammoPrice);
		List<Gun> UserGuns = GameController.Instance.profile.guns;
		foreach (Gun userGun in UserGuns) {
			if (userGun == gun) {
				gun.ammo += 5;
				break;
			}
		}
		UserProfile.Save();
		GlobalPointController.Instance.Render ();

		MessageController.Instance.Render ("Ammo for  Gun " + gun.name + " upgraded by 5 !");
	}
}