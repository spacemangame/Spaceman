using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FireButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	private bool touched;
	private int pointerID;

	public bool canFire { set; get;}

	private float reloadTime = 0.0f;
	private float nextTime;
	private float startTime;

	public Text timerText;
	public Text bulletCountText;
	public Image fireButton;

	public bool isSecondary;
	public bool waitForReload { get; set; }

	public Mission mission { get; set; }

	void Start() {
		mission = GameController.Instance.mission;
		waitForReload = false;
		if (isSecondary == true) {
			if ((mission.secondaryGun != null) && (mission.secondaryGun.reloadTime > Constant.reloadTime)) {
				reloadTime = mission.secondaryGun.reloadTime;
				waitForReload = true;
			}
		}
		timerText.gameObject.SetActive (false);
	}

	void Awake () {
		touched = false;
	}

	public void OnPointerUp(PointerEventData ped) {
		if (waitForReload == false && ped.pointerId == pointerID) {
			touched = false;
			canFire = false;
		}
	}

	void LateUpdate() {
		if (waitForReload && touched && Time.time > nextTime) {
			nextTime = Time.time + 0.1f;
			UpdateText ();
		}
	}

	public void OnPointerDown(PointerEventData ped) {
		if (!touched) {
			touched = true;
			pointerID = ped.pointerId;
			canFire = true;

			if (waitForReload) {
				Invoke ("enableFireButton", reloadTime);
				startTime = Time.time;
				fireButton.gameObject.SetActive (false);
				bulletCountText.gameObject.SetActive (false);
				timerText.gameObject.SetActive (true);
			}
		}
	}

	private void enableFireButton() {
		fireButton.gameObject.SetActive(true);
		bulletCountText.gameObject.SetActive (true);
		timerText.gameObject.SetActive (false);
		touched = false;
		canFire = false;
	}


	private void UpdateText() {
		float time = Mathf.Clamp(reloadTime - (nextTime - startTime), 0, reloadTime);
		timerText.text = time.ToString("0.0");
	}
}