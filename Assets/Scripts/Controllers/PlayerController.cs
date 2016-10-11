using UnityEngine;
using System.Collections;

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
	public Transform shotSpawn1;
	public Transform shotSpawn2;
	public float fireRate;
	public VirtualJoystick joystick;
	public FireButton fireButton;

	public bool useAccelerometer {set; get;}

	private float nextFire;
	private Rigidbody rb;
	private Quaternion calibrationQuaternion;

	private bool useInput;

	private Mission mission { get; set; }
	Coroutine destabilise { get; set; }

	void Start() {
		useInput = true;
		rb = GetComponent<Rigidbody> ();
		CalibrateAccelerometer (); //TODO should be outside of here outside, in options perhaps
		useAccelerometer = true;
		mission = GameController.Instance.mission;
		destabilise = StartCoroutine(DestabilisePlayer ());
	}

	void OnDestroy() {
		StopCoroutine (destabilise);
	}

	void Update(){
		checkBoundary ();

		if (fireButton.canFire && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn1.position, shotSpawn1.rotation);
			Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
			GetComponent<AudioSource> ().Play ();
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
			movement = new Vector3 (Input.acceleration.x, -Input.acceleration.y ,0.0f) * accelerometerSensitivity;
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
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	// Get the 'calibrated' value from the Input
	Vector3 FixAcceleration (Vector3 acceleration) {
		return calibrationQuaternion * acceleration;
	}
}
