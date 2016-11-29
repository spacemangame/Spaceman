using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FreePlaneMover : MonoBehaviour {
	public float speed = 50.0f;
	private float h;
	private float v;
	public float rotateSpeed = 3.0f;

	public Button btn; 
	private Vector3 dir;

	public Matrix4x4 calibrationMatrix;
	Vector3 wantedDeadZone  = Vector3.zero;
	Vector3 _InputDir;

	public Transform player;

	public GameObject leftCamera;
	public GameObject rightCamera;

	public GameObject mainCamera;

	// Use this for initialization
	void Start () {
		calibrateAccelerometer();
		Button calibBtn = btn.GetComponent<Button>();
		calibBtn.onClick.AddListener(calibrateAccelerometer);
		Invoke ("setCameraAngle", 0.3f);
	}

	void setCameraAngle(){
		GameObject c = transform.Find ("Camera").gameObject;
		Debug.Log ("setting camera");
		foreach (Transform child in c.transform) {
//			child.rotation = Quaternion.Euler (0, 45, 0);

			child.Rotate(new Vector3(0,90,0));
			Debug.Log (child.name + " " + child.rotation.ToString());
		}
	}

	void calibrateAccelerometer()
	{
		dir = Vector3.zero;
		dir = Input.acceleration;
		if (dir.sqrMagnitude > 1)
			dir.Normalize();
	}


	//Method to get the calibrated input 
	Vector3 getAccelerometer(Vector3 accelerator){
		Vector3 accel = Input.acceleration;
		accel.x = accel.x - dir.x;
		accel.y = accel.y - dir.y;
		accel.z = accel.z - dir.z;
		return accel;

	}
		



	// Update is called once per frame
	void Update () {
		

		
//		transform.Translate (1.5, 0, 0);
		transform.Translate (1.5f, 0.0f, 0.0f);

		if (SystemInfo.deviceType == DeviceType.Desktop) {
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");

		} else {
			_InputDir = getAccelerometer(Input.acceleration);
			h = _InputDir.x;
			v = _InputDir.z;
		}	


		transform.Rotate(0, h * speed * Time.deltaTime, 0);
		transform.localEulerAngles = new Vector3 (-h*60, transform.localEulerAngles.y, v*45);

		mainCamera.transform.LookAt (player);

		Debug.Log ("Camera Position " + mainCamera.transform.position);
		Debug.Log ("Player Position " + transform.position);
		
//		zt.text =  calibrationMatrix.ToString();

	}

}

