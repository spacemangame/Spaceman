using UnityEngine;
using System.Collections;

public class testMoveScript : MonoBehaviour {

	public float maneuverability = 0.9f;
	public float speed = 2.0f;
	public float tilt = 4;
	// Use this for initialization
	void Start () {
		Debug.Log ("Adding testMover to " + gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
		//logic to smooth out camera chase. Retains bais% of previous position making camera transition smooth
//		float bias = 0.96f; 
//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

		Camera.main.transform.position = moveCamTo;

		Camera.main.transform.LookAt (transform.position + transform.forward * 30.0f);

//		transform.position += transform.forward * Time.deltaTime * 10.0f;

//		transform.Translate(Input.GetAxis("Horizontal") * maneuverability, 0.0f, Input.GetAxis("Vertical") );

		if (Input.GetAxis("Vertical") > 0){
			transform.Translate(Input.GetAxis("Horizontal") * maneuverability, 0.0f, 2.0f * speed);
		}else
			transform.Translate(Input.GetAxis("Horizontal") * maneuverability, 0.0f, speed );

		transform.position = new Vector3 (transform.position.x, 10.0f, transform.position.z);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, Input.GetAxis("Horizontal") * -tilt);

		float terrainHeightWhereWeAre = Terrain.activeTerrain.SampleHeight (transform.position);
		if (transform.position.y < terrainHeightWhereWeAre) {
			Destroy(gameObject);
		}

	}
}
