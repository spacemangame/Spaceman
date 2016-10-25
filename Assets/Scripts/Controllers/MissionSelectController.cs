using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class MissionSelectController: MonoBehaviour
{
	public GameObject ContentPanel;
	public GameObject MissionPanelPrefab;

	public List<Mission> Missions { get; set; }

	public GameObject missionPanel;

	public MissionPanelController missionPanelController { get; set; }

	void Start() {

		missionPanelController = missionPanel.GetComponent<MissionPanelController>();


		Missions = GameController.Instance.missions = DataGenerator.GenerateMissions ();

		// 1. Iterate through the data, 
		//	  instantiate prefab, 
		//	  set the data, 
		//	  add it to panel
		MissionPanelPrefab = (GameObject) Resources.Load("MissionPanel", typeof(GameObject));

		foreach(Mission mission in Missions) {

			GameObject missionItem = Instantiate(MissionPanelPrefab) as GameObject;
			MissionSelectItemController controller = missionItem.GetComponent<MissionSelectItemController>();

			controller.mission = mission;
			controller.missionPanelController = missionPanelController;

			missionItem.transform.SetParent(ContentPanel.transform);
			missionItem.transform.localScale = Vector3.one;
		}
	}
}

