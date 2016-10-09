using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class FireButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	private bool touched;
	private int pointerID;

	public bool canFire { set; get;}

	void Awake () {
		touched = false;
	}

	public void OnPointerDown(PointerEventData ped) {
		if (!touched) {
			touched = true;
			pointerID = ped.pointerId;
			canFire = true;
		}
	}

	public void OnPointerUp(PointerEventData ped) {
		if (ped.pointerId == pointerID) {
			touched = false;
			canFire = false;
		}
	}
}
