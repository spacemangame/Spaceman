using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectItemController: MonoBehaviour
{

	public Text MaxCoins;
	public Text MaxMedals;
	public Text SelectButtonText;
	public Button SelectButton;

	public Mission mission { get; set;}

	public MissionPanelController missionPanelController { get;set;}

	void Start() {
		
		int targetItemCountKD = mission.targetItemCount;
		int itemValueKD = mission.item.value;
		MaxCoins.text = (targetItemCountKD * itemValueKD).ToString ();
		MaxMedals.text = mission.maxMedalEarned.ToString ();

		SelectButtonText.text = mission.missionName;
		SelectButton.onClick.AddListener(() => OnMissionSelect( ));
	}

	public void OnMissionSelect() {
		missionPanelController.SetupMission (mission.id);
	}
}

