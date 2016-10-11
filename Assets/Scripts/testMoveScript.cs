using UnityEngine;
using System.Collections;

public class testMoveScript : MonoBehaviour {

	public float maneuverability = 1.0f; //how fast a player can maneuver
	public float speed = 20.0f; //speed of player
	public float tilt = 4.0f; //max tilt factor
	public float checkpointReward; //seconds to add on checkpoint touchdown
	public GameObject explosion; //explosion effect to play on player colllision

	//joystick and booster button 
	public VirtualJoystick joystick;
	public BoostButton boostButton;

	private Rigidbody rb;
	private float boost = 1.0f;
	private CountDownTimer cTimer;


	void Start () {
		rb = GetComponent<Rigidbody> ();
		GameObject g = GameObject.Find ("TimerText");
		cTimer = g.GetComponent<CountDownTimer> ();
	}
	//function that gets called when player object collides with terrain  
	void OnCollisionEnter(Collision col){
		destroyOnTimer ();
	}

	//function that gets called on checkpoint touchdown
	void OnTriggerEnter(Collider other){
		cTimer.updateTimer (checkpointReward);
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
		
	public void destroyOnTimer(){
		Destroy (gameObject);
		if(explosion != null)
			Instantiate (explosion, transform.position, transform.rotation);
		cTimer.stopTimer = true;
	}

	void FixedUpdate(){
		Vector3 moveCamTo = transform.position - transform.forward * 4.0f + Vector3.up * 3.0f;
//		float bias = 0.7f;
//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
		Camera.main.transform.position = moveCamTo;
		Camera.main.transform.LookAt (transform.position + transform.forward * 10.0f);

		Vector3 movement;
		boost = 1.0f;
		if (Input.GetButton ("Jump") || boostButton.boost) {
			boost = 2.0f;
		}

		if (joystick.InputDirection != Vector3.zero) {
			movement = new Vector3 (joystick.InputDirection.x * maneuverability, joystick.InputDirection.z * maneuverability, speed * boost);
		} else {
			if (SystemInfo.deviceType == DeviceType.Desktop) {
				movement = new Vector3 ( Input.GetAxis("Horizontal") * maneuverability, Input.GetAxis("Vertical") * maneuverability, speed * boost);
			} else {
				movement = new Vector3 ( Input.acceleration.x * maneuverability, Input.acceleration.z * maneuverability, speed * boost);
//				movement = new Vector3 (Input.acceleration.x, 0.0f, Input.acceleration.y) * accelerometerSensitivity;
			}
		}

		rb.velocity = movement;
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}




}
