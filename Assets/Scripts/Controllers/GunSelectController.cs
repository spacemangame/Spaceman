using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;


public class GunSelectController: MonoBehaviour
{

	public GameObject ContentPanel;
	public GameObject GunListItemPrefab;

	public List<Gun> Guns { get; set; }
	public List<GameObject> GunGameObjects { get; set; }

	public Color SelectedColor;

	public Gun SelectedGun { get; set; }

	void Start() {

		//TODO: Uncomment this when gun upgrade is implemented
		//Guns = GameController.Instance.profile.guns;

		//TODO: Remove this when gun upgrade is implemented
		Guns = GameController.Instance.shop.guns;

		// 1. Iterate through the data, 
		//	  instantiate prefab, 
		//	  set the data, 
		//	  add it to panel
		GameObject gunPrefabItem = (GameObject) Resources.Load("GunSelectListItem", typeof(GameObject));

		GunGameObjects = new List<GameObject> ();

		foreach(Gun gun in Guns){
			
			GameObject gunItem = Instantiate(gunPrefabItem) as GameObject;
			GunSelectItemController controller = gunItem.GetComponent<GunSelectItemController>();

			GunGameObjects.Add (gunItem);

			Sprite image = Resources.Load<Sprite> ("Images/" + gun.texture);
			controller.Image.sprite = image;

			controller.gun = gun;
			controller.gunSelectController = this;

			gunItem.transform.SetParent(ContentPanel.transform);
			gunItem.transform.localScale = Vector3.one;
		}

		Gun secGun;

		if (GameController.Instance.profile.secondaryGun != null) {
			secGun = GameController.Instance.profile.secondaryGun ;
		} else {
			secGun = Guns.ElementAt (0);
		}

		SetSelectedGun (secGun);
	}

	public void OnGunSelect() {
		GameController.Instance.mission.secondaryGun = SelectedGun;

		GameController.Instance.profile.secondaryGun = SelectedGun;
		UserProfile.Save ();

		GameController.Instance.StartMission ();
	}

	public void SetSelectedGun(Gun gun) {
		SelectedGun = gun;

		foreach(GameObject gunItem in GunGameObjects) {
			GunSelectItemController controller = gunItem.GetComponent<GunSelectItemController>();

			Color color;

			if (controller.gun == gun) {
				color = SelectedColor;
			} else {
				color = Color.white;
			}

			gunItem.GetComponent<Image> ().color = color;

		}
	}


}

