using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public enum SwipeDirection {
	None = 0,
	Left = 1,
	Right = 2,
	Up = 4,
	Down = 8,
	LeftDown = 9,
	LeftUp = 5,
	RightDown = 10,
	RightUp = 6
}


public class FireButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler {

	private bool touched;
	private int pointerID;

	public bool canFire { set; get;}

	private MissionController missionController;

	private Vector2 origin;
	private Vector2 direction;

	void Awake () {
		touched = false;
		direction = Vector2.zero;
	}

	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag("MissionController");
		if (gameControllerObject != null) {
			missionController = gameControllerObject.GetComponent<MissionController>();
		}
	}

	public void OnPointerDown(PointerEventData ped) {
		if (!touched) {
			touched = true;
			pointerID = ped.pointerId;
			pointerID = ped.pointerId;
			origin = ped.position;
		}
	}

	public void OnPointerUp(PointerEventData ped) {
		if (ped.pointerId == pointerID) {
			touched = false;

			if (direction.x != 0.0f) {
				canFire = false;
				if(direction.x > 0.0f){
					missionController.ChangeActiveGun (1);
				} else{
					missionController.ChangeActiveGun (-1);
				}
			} else {
				canFire = true;
			}
			direction = Vector3.zero;
		}
	}

	public void OnDrag (PointerEventData ped) {
		if (ped.pointerId == pointerID) {
			Vector2 currentPosition = ped.position;
			Vector2 directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
		}
	}

	void LateUpdate() {
		canFire = false;
	}
		
}
