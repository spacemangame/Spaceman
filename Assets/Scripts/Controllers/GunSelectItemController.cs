using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GunSelectItemController: MonoBehaviour
{
	public Image Image;
	public Text HPText;
	public Text AmmoText;
	public Text Name;

	public Gun gun { get; set; }

	public Mission mission {get;set;}

	public GunSelectController gunSelectController  { get; set; }

	void Start() {

		Name.text = gun.name;
		HPText.text = "Damage : " + gun.hitPoint;
		AmmoText.text = "Ammo : " + gun.ammo;

		mission = GameController.Instance.mission;
		Button selctBtn = GetComponent<Button> ();
		selctBtn.onClick.AddListener(() => OnGunSelect( ));
	}

	public void OnGunSelect() {
		gunSelectController.SetSelectedGun (gun);
	}
}
