using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	private PlayerController playerController;

    public void NewGameBtn(string mainGameScreen) {
		GameController.Instance.StartMission (mainGameScreen);

		//playerController.CalibrateAccelerometer (); // calibrate the accelerometer
    }
}
