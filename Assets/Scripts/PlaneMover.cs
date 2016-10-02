using UnityEngine;
using System.Collections;

public class PlaneMover : MonoBehaviour {

	public float speed = 10.0f;
	public float accelerometerSensitivity = 2;
//	public float planelift = 1.965f;
//	public float rollrate = 0.5f;
//	public float elevatorRate = 2;
	private Rigidbody rb;

	void Start () {
		Debug.Log ("Attaching script planerMover to: "+ gameObject.name);
		rb = GetComponent<Rigidbody> ();
	}
	

	void Update () {
		Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
		float bias = 0.96f;

		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
		Camera.main.transform.LookAt (transform.position + transform.forward * 30.0f);

		transform.position += transform.forward * Time.deltaTime * speed;
//		transform.Translate(0, 0, speed * Time.deltaTime);

		speed -= transform.forward.y * 2.0f;

		if (speed < 30)
			speed = 30;
		else if (speed > 50)
			speed = 50;
//		transform.Rotate(-Input.GetAxis("Vertical"), 0.0f , -Input.GetAxis("Horizontal"));	
		Vector3 rotation;
		if (SystemInfo.deviceType == DeviceType.Desktop) {
			rotation = new Vector3 (-Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
		} else {
			rotation = new Vector3 (Input.acceleration.z * accelerometerSensitivity, 0.0f, -Input.acceleration.x * accelerometerSensitivity) ;
		}

		transform.Rotate (rotation);

		float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight (transform.position);

//		if (transform.position.y < terrainHeightWhereWeAre) {
//			transform.position = new Vector3 (transform.position.x, terrainHeightWhereWeAre , transform.position.z);
//		}
	}

//	void FixedUpdate(){
//		rb.AddRelativeForce (Vector3.up * speed * planelift);
//		rb.AddRelativeTorque (0, 0, -Input.GetAxis("Horizontal") * rollrate);
//		rb.AddRelativeForce (0, 0, speed);
//		rb.AddRelativeTorque (-Input.GetAxis("Vertical") * elevatorRate, 0, 0);
//	}
}
