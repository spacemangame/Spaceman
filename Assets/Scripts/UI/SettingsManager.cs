using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour, IPointerDownHandler {

	public GameObject pauseMenu;
	public PlayerController playerController;
	public CheckPointPlayerMove checkpointMove;

	public Toggle Accelerometer;
	public Toggle Sound;

	public UserProfile profile { get; set; }

	void Start() {
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
		profile = GameController.Instance.profile;

		if (!profile.isSoundEnabled) {
			AudioListener.volume = 0;
			Sound.isOn = false;
		}

		if (!profile.isAccelerometerEnabled) {
			playerController.useAccelerometer = false;
			Accelerometer.isOn = false;
		}
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
		if (playerController != null)
			playerController.CalibrateAccelerometer ();
		else
			checkpointMove.CalibrateAccelerometer ();
	}

	private void HidePauseMenu() {
		pauseMenu.SetActive (false);
	}

	private void showPauseMenu() {
		pauseMenu.SetActive (true);
	}

	public void ToggleSound(bool value) {
		if (value) {
			AudioListener.volume = 1;
			profile.isSoundEnabled = true;
		} else {
			AudioListener.volume = 0;
			profile.isSoundEnabled = false;
		}
		UserProfile.Save ();
	}

	public void ToggleAccelerometer(bool value) {

		if (value) {
			playerController.useAccelerometer = true;
			profile.isAccelerometerEnabled = true;
		} else {
			playerController.useAccelerometer = true;
			profile.isAccelerometerEnabled = false;
		}

		UserProfile.Save ();
	}



}
