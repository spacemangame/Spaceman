using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
	
    public void NewGameBtn(string mainGameScreen) {
		GameController.SelectMission ("");

		//playerController.CalibrateAccelerometer (); // calibrate the accelerometer
    }
		
	public void startMission(string missionType) {
		//GameController.StartMission (missionType);
	}
}
