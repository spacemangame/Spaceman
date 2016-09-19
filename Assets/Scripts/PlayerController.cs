using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
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

	private float nextFire;
	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	void Update(){
		if (Input.GetButton ("Jump") && Time.time > nextFire) {
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
			movement = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		} else {
			movement = new Vector3 (Input.acceleration.x, 0.0f, Input.acceleration.y) * accelerometerSensitivity;
		}
		rb.velocity = movement * speed;

		// make sure player doesn't go out of the boundary.
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		// rotate the player to show the movement
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
