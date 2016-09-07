using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {


	public float tumble;

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * tumble;
	}
	

}
