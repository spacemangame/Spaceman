using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoostButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	private bool touched;
	private int pointerID;
	private float boostRefillTime = 3.0f;

	public bool boost { set; get;}

	void Awake () {
		touched = false;
	}

	public void OnPointerDown(PointerEventData ped) {
		if (!touched) {
			touched = true;
			pointerID = ped.pointerId;
			boost = true;
			Invoke ("disableBoost", 3.0f); //boost effect will last for 3s
			gameObject.SetActive(false); //hide boost button
			Invoke("enableBoostButton", 6.0f);
		}
	}

	private void enableBoostButton(){
		gameObject.SetActive (true); //show boost button
	}
	private void disableBoost(){
		boost = false;
		touched = false;
	}

	public void OnPointerUp(PointerEventData ped) {
//		if (ped.pointerId == pointerID) {
//			touched = false;
//			boost = false;
//		}
	}
}