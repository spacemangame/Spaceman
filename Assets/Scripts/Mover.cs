using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public float speed;

	void Start(){

		if (gameObject.tag != "primaryBolt" && gameObject.tag != "secondaryBolt" && gameObject.tag != "enemybolt") {
			speed = Random.Range (-3, -10);
		}

		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;
	}
}
