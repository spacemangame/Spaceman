using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoostButton : MonoBehaviour, IPointerDownHandler {

	private bool touched;
	private float reloadTime = 6.0f;
	private float nextTime;
	private float startTime;

	public bool boost { set; get;}
	public Text timerText;
	public Image boostButton;


	void Start() {
		timerText.gameObject.SetActive (false);
	}

	void Awake () {
		touched = false;
	}

	void LateUpdate() {
		if (touched && Time.time > nextTime) {
			nextTime = Time.time + 0.1f;
			UpdateText ();
		}
	}

	public void pressBoost() {
		if (!touched) {
			touched = true;
			boost = true;
			Invoke ("disableBoost", 3.0f); //boost effect will last for 3s
			Invoke("enableBoostButton", reloadTime);
			startTime = Time.time;
			boostButton.gameObject.SetActive(false);
			timerText.gameObject.SetActive (true);
		}
	}

	public void OnPointerDown(PointerEventData ped) {
		pressBoost ();
	}

	private void enableBoostButton(){
		boostButton.gameObject.SetActive(true);
		timerText.gameObject.SetActive (false);
		touched = false;
	}

	private void disableBoost(){
		boost = false;
	}

	private void UpdateText() {
		float time = Mathf.Clamp(reloadTime - (nextTime - startTime), 0, reloadTime);
		timerText.text = time.ToString("0.0");
	}
		
}