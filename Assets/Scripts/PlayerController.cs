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

	private float nextFire;
	private Rigidbody rb;
	private Quaternion calibrationQuaternion;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		CalibrateAccelerometer (); //TODO should be outside of here outside, in options perhaps
	}

	void Update(){
		if (fireButton.canFire && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn1.position, shotSpawn1.rotation);
			Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
			GetComponent<AudioSource> ().Play ();
		}
	}

	void FixedUpdate(){

		// update the position based on movement.
		Vector3 movement;
		if (SystemInfo.deviceType == DeviceType.Desktop) {
			// desktop
			movement = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0.0f);
		} else {
			// accelerometer
			movement = new Vector3 (Input.acceleration.x, -Input.acceleration.y ,0.0f) * accelerometerSensitivity;
		}

		// joystick
		if (joystick.InputDirection != Vector3.zero) {
			movement = joystick.InputDirection;
		}

		rb.velocity = movement * speed;

		// make sure player doesn't go out of the boundary.
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			0.0f
		);
		Debug.Log (rb.position);

		// rotate the player to show the movement
		rb.rotation = Quaternion.Euler (rb.velocity.y * -tilt, 0.0f, rb.velocity.x * -tilt);
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
