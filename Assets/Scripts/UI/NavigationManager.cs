using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class NavigationManager: MonoBehaviour
{

	public void ShowProfileScreen(string screen)
	{
		SceneManager.LoadScene(screen);
	}

	public void ReturnToScreen(string screen)
	{
		SceneManager.LoadScene(screen);
	}

	public void SelectMission() {
		GameController.SelectMission ("Missions");
	}

	public void RestartMission() {
		GameController.Instance.missions = DataGenerator.GenerateMissions();

		GameController.Instance.mission = GameController.Instance.missions.Find(x => x.id == GameController.Instance.mission.id);
		GameController.Instance.mission.secondaryGun = GameController.Instance.profile.secondaryGun;

		GameController.StartMission ();
	}

}

