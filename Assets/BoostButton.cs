using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BoostButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	private bool touched;
	private int pointerID;

	public bool boost { set; get;}

	void Awake () {
		touched = false;
	}

	public void OnPointerDown(PointerEventData ped) {
		if (!touched) {
			touched = true;
			pointerID = ped.pointerId;
			boost = true;
		}
	}

	public void OnPointerUp(PointerEventData ped) {
		if (ped.pointerId == pointerID) {
			touched = false;
			boost = false;
		}
	}
}