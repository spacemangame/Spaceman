using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour {
	public float maxTranslate = 1.5f;  // Amount to move left and right from the start point
	public float speed = 2.0f;
	public bool hMove = true;
	public bool vMove = false;

	private Vector3 startPos;

	void Start() {
		startPos = transform.position;
	}
	void Update() {
		Vector3 v = startPos;
		float delta = maxTranslate * Mathf.Sin (Time.time * speed);
		if (hMove)
			v.x += delta;
		else
			v.y += delta;
		transform.position = v;
	}
}
