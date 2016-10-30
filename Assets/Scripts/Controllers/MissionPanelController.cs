using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class MissionPanelController : MonoBehaviour {
	
	public GameObject gunSelect;
	public GameObject missionSelect;

	public Mission mission { get; set; }

	void Start() {
		
	}	

	public void ShowGunSelection() {
		
		if (GameController.Instance.profile.guns.Count == 0) {
			GameController.StartMission ();
			return;
		}

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

	public void SetupMission(int id) {
		mission = GameController.Instance.missions.Find(x => x.id == id);

		GameController.Instance.mission = mission;

		// If scene is not drug mission then show modal to select guns
		if (!mission.scene.StartsWith("drug", StringComparison.InvariantCultureIgnoreCase)) {
			ShowGunSelection ();
		} else {
			GameController.StartMission ();
		}

	}
}
