using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class DetonatorPhysics : MonoBehaviour {

	public Detonation detonation { get; set; }

	// Use this for initialization
	void Start () {
		detonation = GameController.Instance.mission.secondaryGun.detonation;
		Invoke("Boom", detonation.delay);
    }

	void Boom() {
		// This foreach iterates through all the colliders inside the overlap sphere and sendMessages the bombExplode method

		Collider collider = gameObject.GetComponent<Collider> ();
		Vector3 _explosionPosition = transform.position; //- Vector3.Normalize(MyDetonator().direction);
		Collider[] _colliders = Physics.OverlapSphere(_explosionPosition, detonation.radius);

		foreach (var bombInstance in _colliders) {
			bombInstance.SendMessage("BombExplode", collider, SendMessageOptions.DontRequireReceiver);
		} 

		Invoke ("Destroy", 0.0f);
	}


}
