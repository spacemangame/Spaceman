using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour, IPointerDownHandler {

	public GameObject pauseMenu;
	public PlayerController playerController;
	public CheckPointPlayerMove checkpointMove;

	public MissionController missionController;

	public Toggle Accelerometer;
	public Toggle Sound;
	public Toggle VrModeButton;

	public UserProfile profile { get; set; }

	void Start() {
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
		profile = GameController.Instance.profile;

		if (!profile.isSoundEnabled) {
			AudioListener.volume = 0;
			Sound.isOn = false;
		}

		if (VrModeButton == null) {

			if (!profile.isAccelerometerEnabled) {
				playerController.useAccelerometer = false;
				playerController.joystick.gameObject.SetActive (true);
				Accelerometer.isOn = false;
			} else {
				playerController.joystick.gameObject.SetActive (false);
				Accelerometer.isOn = true;
			}
		} else {

			checkpointMove.initialised = false;
			if (profile.isVREnabled) {
				VrModeButton.isOn = true;
			} else {
				VrModeButton.isOn = false;
			}

			Invoke ("setIntialised", 0.1f);
		}
			
	}

	public void setIntialised() {
		checkpointMove.initialised = true;
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

		if (missionController != null) {
			missionController.showAllControls ();
		} else {
			checkpointMove.ShowAllControls ();
		}

	}

	private void showPauseMenu() {

		if (missionController != null) {
			missionController.hideAllControls ();
		} else {
			checkpointMove.HideAllControls ();
		}

		pauseMenu.SetActive (true);
	}

	public void ToggleSound(bool value) {
		if (Sound.isOn) {
			AudioListener.volume = 1;
			profile.isSoundEnabled = true;
		} else {
			AudioListener.volume = 0;
			profile.isSoundEnabled = false;
		}
		UserProfile.Save ();
	}

	public void ToggleAccelerometer(bool value) {
		if (playerController != null) {
			playerController.useAccelerometer = true;
			playerController.joystick.gameObject.SetActive (false);
			profile.isAccelerometerEnabled = true;
		} else {
			playerController.useAccelerometer = false;
			playerController.joystick.gameObject.SetActive (true);
			profile.isAccelerometerEnabled = false;
		}

		UserProfile.Save ();
	}

}
