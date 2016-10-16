using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;


public class GunBuyController: MonoBehaviour
{

	public GameObject GunPanel;
	public GameObject GunBuyListItemPrefab;

	public List<Gun> GunsToBuy { get; set; }
	public List<GameObject> GunGameObjects { get; set; }

	void Start() {

		List<Gun> ShopGuns = GameController.Instance.shop.guns;
		List<Gun> PlayerGuns = GameController.Instance.profile.guns;

		GunsToBuy = ShopGuns.Except(PlayerGuns).ToList();
		Debug.Log (GunsToBuy.Count);

		GameObject gunPrefabItem = (GameObject) Resources.Load("GunBuyListItem", typeof(GameObject));

		GunGameObjects = new List<GameObject> ();

		foreach(Gun gun in GunsToBuy){

			GameObject gunItem = Instantiate(gunPrefabItem) as GameObject;
			GunBuyItemController controller = gunItem.GetComponent<GunBuyItemController>();

			GunGameObjects.Add (gunItem);

			//controller.Image.sprite = gun.image;
			controller.gun = gun;
			controller.gunBuyController = this;

			gunItem.transform.SetParent(GunPanel.transform);
			gunItem.transform.localScale = Vector3.one;
		}
	}

	public void Buy() {


		//UserProfile.Save ();
	}
		
}


