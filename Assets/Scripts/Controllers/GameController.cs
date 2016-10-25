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
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			DestroyImmediate(gameObject);
		}
	}

	void Start() {
		if (GameController.Instance.profile == null) {
			DataGenerator.GenerateShop ();
			UserProfile.Load ();
		}
	}

	public static void SelectMission(string screen) {
		GameController.Instance.missions = DataGenerator.GenerateMissions();
		SceneManager.LoadScene(screen);
	}

	public static void StartMission() {
		SceneManager.LoadScene (GameController.Instance.mission.scene);
	}

	public void StartBonusMission() {
		UserProfile profile = GameController.Instance.profile;
		Mission mission = GameController.Instance.mission;
		int bonusMissionCount = profile.bonusMission;
		int medals = profile.medals;
		int level = (int)Math.Floor (medals / 9.0f);
		if ((bonusMissionCount < level) & (mission.medalEarned == 3)) {
			profile.bonusMission++;
			UserProfile.Save ();
			Mission BonusMission = DataGenerator.GetBonusMission ();
			GameController.Instance.mission = BonusMission;
			SceneManager.LoadScene ("BonusMission");
			return;
		}
	}


}