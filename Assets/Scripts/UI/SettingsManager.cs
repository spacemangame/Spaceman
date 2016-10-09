using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour, IPointerDownHandler {

	public GameObject pauseMenu;
	public PlayerController playerController;

	void Start(){
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
	}

	public void OnPointerDown(PointerEventData ped) {
		Time.timeScale = 0;
		showPauseMenu ();
	}

	public void Resume() {
		Time.timeScale = 1;
		HidePauseMenu ();
	}

	public void Exit() {
		SceneManager.LoadScene("Main Menu");
	}

	public void Calibrate() {
		playerController.CalibrateAccelerometer ();
	}

	private void HidePauseMenu() {
		pauseMenu.SetActive (false);
	}

	private void showPauseMenu() {
		pauseMenu.SetActive (true);
	}

	public void ToggleSound(bool value) {
		AudioListener.volume = value ? 1 : 0;
	}

	public void ToggleAccelerometer(bool value) {
		playerController.useAccelerometer = value;
	}



}
