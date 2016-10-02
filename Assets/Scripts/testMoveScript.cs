using UnityEngine;
using System.Collections;

public class testMoveScript : MonoBehaviour {

	public float maneuverability = 0.9f;
	public float speed = 20.0f;
	public float tilt = 4;
	// Use this for initialization
	void Start () {
		Debug.Log ("Adding testMover to " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
		float bias = 0.7f;

		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
		Camera.main.transform.LookAt (transform.position + transform.forward * 30.0f);

//		transform.position += transform.forward * Time.deltaTime * 10.0f;
		if (Input.GetAxis("Vertical") > 0){
			transform.Translate(Input.GetAxis("Horizontal") * maneuverability, 0.0f, 5.0f * speed * Time.deltaTime);
		}else
			transform.Translate(Input.GetAxis("Horizontal") * maneuverability, 0.0f, speed * Time.deltaTime);
		transform.position = new Vector3 (transform.position.x, 10.0f, transform.position.z);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, Input.GetAxis("Horizontal") * -tilt);

	}
}
