using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class MissionPanelController : MonoBehaviour {
	public Text kidDeliveryCoins;
	public Text kidDeliveryMedals;
	public Text drugDeliveryCoins;
	public Text drugDeliveryMedals;
	public Text kidPickupCoins;
	public Text kidPickupMedals;

	public GameObject gunSelect;
	public GameObject missionSelect;

	public Mission mission { get; set; }

	void Start() {

		int targetItemCountKD = GameController.Instance.missions [0].targetItemCount;
		int itemValueKD = GameController.Instance.missions [0].item.value;
		kidDeliveryCoins.text = (targetItemCountKD * itemValueKD).ToString ();
		kidDeliveryMedals.text = GameController.Instance.missions [0].maxMedalEarned.ToString ();

		int targetItemCountKP = GameController.Instance.missions [1].targetItemCount;
		int itemValueKP = GameController.Instance.missions [1].item.value;
		kidPickupCoins.text = (targetItemCountKP * itemValueKP).ToString ();
		kidPickupMedals.text = GameController.Instance.missions [1].maxMedalEarned.ToString ();

		int targetItemCountDD = GameController.Instance.missions [2].targetItemCount;
		int itemValueDD = GameController.Instance.missions [2].item.value;
		drugDeliveryCoins.text = (targetItemCountDD * itemValueDD).ToString ();
		drugDeliveryMedals.text = GameController.Instance.missions [2].maxMedalEarned.ToString ();
	}	

	public void ShowGunSelection() {


		/* TODO: Uncomment this when gun upgrade is implemented
		 if (GameController.Instance.profile.guns.Count == 0) {
			GameController.Instance.StartMission ();
			return;
		} 
		*/

		missionSelect.SetActive (false);
		gunSelect.SetActive (true);
	}

	public void HideGunSelectionMenu() {
		gunSelect.SetActive (false);
		missionSelect.SetActive (true);
	}

	public void OnBack() {
		HideGunSelectionMenu ();
		GameController.Instance.mission = null;
	}

	public void SetupMission(int missionType) {
		mission = GameController.Instance.missions.ElementAt (missionType - 1);

		GameController.Instance.mission = mission;

		// If scene is not drug mission then show modal to select guns
		if (GameController.Instance.GetMissionScene () != "Drug") {
			ShowGunSelection ();
		} else {
			GameController.Instance.StartMission ();
		}

	}
}
