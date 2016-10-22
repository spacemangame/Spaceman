using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {

	public Vector2 startWait; 
	public float tilt;
	private float targetManeuver;
	public float dodge; 
	public float smoothing;
	public Vector2 manenuverTime;
	public Vector2 maneuverWait;
	private Rigidbody rb;
	private float currentSpeed;
	public Boundary boundary;
	private Obstacle obstacle;
	private bool moveTowardPlayer;

	private GameObject player;

	void Start () {
		player =  GameObject.FindGameObjectWithTag("Player");

		rb = GetComponent<Rigidbody> ();

		obstacle = GetComponent<GameObjectObstacle> ().obstacle;
		if (obstacle != null && obstacle.isAI) {
			moveTowardPlayer = true;
		} else {
			moveTowardPlayer = false;
			StartCoroutine (Evade ());
		}
			
		currentSpeed = rb.velocity.z;
	}

	IEnumerator Evade(){
		yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
		while (true) {
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign(transform.position.x);
			yield return new WaitForSeconds (Random.Range(manenuverTime.x, manenuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}


	void FixedUpdate () {
		if (moveTowardPlayer && player != null) {
			rb.position = Vector3.MoveTowards(rb.position, player.transform.position, obstacle.velocity * Time.deltaTime);
		} else {
			float newMeneuverX = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
			float newMeneuverY = Mathf.MoveTowards (rb.velocity.y, targetManeuver, Time.deltaTime * smoothing);
			rb.velocity = new Vector3 (newMeneuverX, newMeneuverY, currentSpeed);
		}

		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			rb.position.z
		);
		rb.rotation = Quaternion.Euler (rb.velocity.y * tilt, 0.0f, rb.velocity.x * -tilt);
	}
}
