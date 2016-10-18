using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;


public class GunBuyController: MonoBehaviour
{

	public GameObject ContentPanel;
	public GameObject GunBuyListItemPrefab;

	public List<Gun> GunsToBuy { get; set; }

	void Start() {

	    List<Gun> ShopGuns = GameController.Instance.shop.guns;
		List<Gun> PlayerGuns = GameController.Instance.profile.guns;

		GunsToBuy = ShopGuns.Where(el2 => !PlayerGuns.Any(el1 => el1.id == el2.id)).ToList();
		Debug.Log (GunsToBuy.Count);

		GameObject gunPrefabItem = (GameObject) Resources.Load("GunBuyListItem", typeof(GameObject));

		foreach(Gun gun in GunsToBuy) {
			
			Debug.Log ("Inside foreach");
			GameObject gunItem = Instantiate(gunPrefabItem) as GameObject;
			GunBuyItemController controller = gunItem.GetComponent<GunBuyItemController>();

			Sprite image = Resources.Load<Sprite> ("Images/" + gun.texture);
			controller.Image.sprite = image;

			controller.gun = gun;
			controller.gunBuyController = this;

			gunItem.transform.SetParent(ContentPanel.transform);
			gunItem.transform.localScale = Vector3.one;
		}
	}

	public void Buy(Gun gun) {
		GameController.Instance.profile.coins -= gun.price;
		GameController.Instance.profile.guns.Add (gun);
		UserProfile.Save();
	}
		
}


