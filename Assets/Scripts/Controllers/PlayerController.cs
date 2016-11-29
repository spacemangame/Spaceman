using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Boundary{
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {
	public int speed;
	public int accelerometerSensitivity;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn1 { get; set; }
	public Transform shotSpawn2 { get; set; }
	public Transform shotSpawnBomb { get; set; }
	public GameObject secPrefab { get; set; } 

	public VirtualJoystick joystick;
	public FireButton primaryFireButton;
	public FireButton secondaryFireButton;

	public bool useAccelerometer {set; get;}

	private float nextFirePrimary;
	private float nextFireSecondary;
	private Rigidbody rb;
	private Quaternion calibrationQuaternion;

	private bool useInput;
	private bool shoot;

	private Vector3 dir;
	private Vector3 _InputDir;

	private MissionController mc;
	private Mission mission;
	Coroutine destabilise { get; set; }

	public GameObject spaceship { get; set; }

	protected Queue<Vector3> filterDataQueue = new Queue<Vector3>();
	public int filterLength = 3;

	void Start() {
		useInput = true;
		rb = GetComponent<Rigidbody> ();
		CalibrateAccelerometer (); //TODO should be outside of here outside, in options perhaps
		useAccelerometer = true;

		mission = GameController.Instance.mission;

		GameObject gameControllerObject = GameObject.FindWithTag("MissionController");
		if (gameControllerObject != null) {
			mc = gameControllerObject.GetComponent<MissionController>();
		}

		spaceship = (GameObject) Resources.Load(GameController.Instance.profile.spaceship.prefab, typeof(GameObject));
		spaceship =  (GameObject) Instantiate (spaceship);


		foreach (Transform child in spaceship.transform)
		{
			if (child.tag == "shotspawn1") {
				shotSpawn1 = child;
			} else if (child.tag == "shotspawn2") {
				shotSpawn2 = child;
			} else if (child.tag == "shotspawnbomb") {
				shotSpawnBomb = child;
			} else if (child.tag == "secprefab") {
				secPrefab = child.gameObject;
			}
		}

		spaceship.transform.SetParent(gameObject.transform);
		//spaceship.transform.localScale = Vector3.one;

		if (mission.secondaryGun != null && mission.secondaryGun.bolt == Constant.gunBomb) {
			secPrefab.SetActive (true);
		}

		destabilise = StartCoroutine(DestabilisePlayer ());
		tilt = 5.0f;

		for(int i=0; i<filterLength; i++)
			filterDataQueue.Enqueue(Input.acceleration);
	}

	void OnDestroy() {
		try {
			StopCoroutine (destabilise);
		} catch(System.Exception e){}
	}

	void Update(){
		checkBoundary ();
	}

	void LateUpdate() {
		// shoot primary
		if (mc.HasBullet(true) && primaryFireButton.canFire && Time.time > nextFirePrimary) {
			nextFirePrimary = Time.time + mc.primaryGun.reloadTime;
			shootBullet (true);
			mc.DecreaseBullet (true);
		}

		// shoot secondary
		if (mission.secondaryGun != null && mc.HasBullet(false) && secondaryFireButton.canFire && Time.time > nextFireSecondary) {
			nextFireSecondary = Time.time + mc.secondaryGun.reloadTime;
			shootBullet (false);
			mc.DecreaseBullet (false);
		}
	}

	IEnumerator DestabilisePlayer() {

		if (mission.type == Constant.Transport && mission.stabilitliy > 0) {

			yield return new WaitForSeconds (5);

			while (true) {
				
				if (Random.value <= mission.stabilitliy) {
					useInput = false;
					rb.AddForce (new Vector3 (
						Random.Range (-Constant.instabilityFactor, Constant.instabilityFactor), 
						Random.Range (-Constant.instabilityFactor, Constant.instabilityFactor),
						0),
						ForceMode.VelocityChange);
					checkBoundary ();
				}
				yield return new WaitForSecondsRealtime (0.2f);
				useInput = true;
			}
		}
	}

	void FixedUpdate(){
		if (!useInput) {
			return;
		}

		// update the position based on movement.
		Vector3 movement = Vector3.zero;
		if (SystemInfo.deviceType == DeviceType.Desktop) {
			// desktop
			movement = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0.0f);
		} else if (useAccelerometer) {
			
			// accelerometer
			_InputDir = getAccelerometer(Input.acceleration);
			movement = new Vector3 (_InputDir.x, _InputDir.z * 1.25f, 0.0f) * accelerometerSensitivity;

//			Debug.Log ("Z:" + _InputDir.z);
		}

		// joystick
		if (joystick.InputDirection != Vector3.zero) {
			movement = joystick.InputDirection;
		}

		if (movement == Vector3.zero) {
			return; // no input;
		}

		rb.velocity = movement * speed;

		checkBoundary ();

		// rotate the player to show the movement
		rb.rotation = Quaternion.Euler (rb.velocity.y * -tilt, 0.0f, rb.velocity.x * -tilt);
	}

	private void checkBoundary() {
		// make sure player doesn't go out of the boundary.
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			0.0f
		);
	}

	// calibrates the Input.acceleration
	public void CalibrateAccelerometer () {
//		Vector3 accelerationSnapshot = Input.acceleration;
//		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
//		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);

		dir = Vector3.zero;
		dir = Input.acceleration;
		if (dir.sqrMagnitude > 1)
			dir.Normalize();


		Debug.Log ("Calibarate Z:" + dir.z);
	}
		

	//Method to get the calibrated input 
	Vector3 getAccelerometer(Vector3 accelerator){
//		Vector3 accel = Input.acceleration;
		Vector3 accel = LowPassAccelerometer();
		accel.x = accel.x - dir.x;
		accel.y = accel.y - dir.y;
		accel.z = accel.z - dir.z;
		return accel;
	}


	// Get the 'calibrated' value from the Input
	Vector3 FixAcceleration (Vector3 acceleration) {
		return calibrationQuaternion * acceleration;
	}

	private void shootBullet(bool isPrimary) {
		string boltResource = isPrimary ? mc.primaryGun.bolt : mc.secondaryGun.bolt;
		GameObject bolt = Resources.Load<GameObject> (boltResource);

		if (isPrimary || mc.secondaryGun.bolt != Constant.gunBomb) {
			Instantiate (bolt, shotSpawn1.position, shotSpawn1.rotation);
			Instantiate (bolt, shotSpawn2.position, shotSpawn2.rotation);
		} else {
			Instantiate (bolt, shotSpawnBomb.position, shotSpawnBomb.rotation);
		}
		GetComponent<AudioSource> ().Play ();
	}

	public Vector3 LowPassAccelerometer() {
		if(filterLength <= 0)
			return Input.acceleration;
		filterDataQueue.Enqueue(Input.acceleration);
		filterDataQueue.Dequeue();

		Vector3 vFiltered= Vector3.zero;
		foreach(Vector3 v in filterDataQueue)
			vFiltered += v;
		vFiltered /= filterLength;
		return vFiltered;
	}
}
