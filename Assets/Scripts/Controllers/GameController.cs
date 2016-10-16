using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	// Make global
	public static GameController Instance {
		get;
		set;
	}

	public bool isFirstRun { get; set;}

	public UserProfile profile { get; set; }
	public Shop shop { get; set; }
	public Mission mission { get; set; }
	public List<Mission> missions { get; set;}

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		if (GameController.Instance == null) Instance = this;
	}

	void Start() {
		
		if (!GameController.Instance.isFirstRun) {
			GameController.Instance.isFirstRun = true;
			DataGenerator.GenerateShop ();
			UserProfile.Load ();
		}

	}


	public void SelectMission(string screen) {
		GameController.Instance.missions = DataGenerator.GenerateMissions();
		SceneManager.LoadScene(screen);
	}

    public void ShowProfileScreen(string screen)
    {
        SceneManager.LoadScene(screen);
    }

    public void ReturnToScreen(string screen)
    {
        SceneManager.LoadScene(screen);
    }

    public void RestartMission() {
		GameController.Instance.missions = DataGenerator.GenerateMissions();
		int missionIndex = GameController.Instance.mission.id - 1;

		mission = GameController.Instance.missions.ElementAt (missionIndex);
		GameController.Instance.mission = mission;

		StartMission ();
	}

	public String GetMissionScene() {
		if (mission.missionName.StartsWith ("drug", StringComparison.InvariantCultureIgnoreCase)) {
			return "Drug";
		} else {
			return "Main";
		}	
	}

	public void StartMission() {
		SceneManager.LoadScene (GetMissionScene ());
	}
		
}