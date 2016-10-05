using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

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


	public void StartMission(string gameScreen) {
		//TODO: Generate a mission and populate stuff
		Mission mission = new Mission();
		GameController.Instance.mission = mission;

		SceneManager.LoadScene(gameScreen);
	}

}