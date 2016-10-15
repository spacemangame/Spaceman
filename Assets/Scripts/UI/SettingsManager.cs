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
			
			if (playerController != null) {
				playerController.useAccelerometer = false;
				playerController.joystick.gameObject.SetActive (true);
			} else {
				checkpointMove.joystick.gameObject.SetActive (true);
			}
			Accelerometer.isOn = false;
		} else {
			if (playerController != null) {
				playerController.joystick.gameObject.SetActive (false);
			} else {
				checkpointMove.joystick.gameObject.SetActive (false);
			}
			Accelerometer.isOn = true;
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

		if (Accelerometer.isOn) {
			
			if (playerController != null) {
				playerController.useAccelerometer = true;
				playerController.joystick.gameObject.SetActive (false);
			} else {
				checkpointMove.joystick.gameObject.SetActive (false);
			}

			profile.isAccelerometerEnabled = true;
		} else {
			
			if (playerController != null) {
				playerController.useAccelerometer = false;
				playerController.joystick.gameObject.SetActive (true);
			} else {
				checkpointMove.joystick.gameObject.SetActive (true);
			}

			profile.isAccelerometerEnabled = false;
		}

		UserProfile.Save ();
	}



}
