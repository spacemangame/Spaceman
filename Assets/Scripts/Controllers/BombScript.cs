using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;



public class BombScript : MonoBehaviour
{
	private float bornTime;
	private float bombTime = 1.0f;

	public GameObject explosion;

	// INIT
	void Start () {
		bornTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		bombMechanics();
	}    

	void bombMechanics() {

		// This if statement is basically the bombs timer and is the second value is set in the bombTime variable 
		if((Time.time - bornTime) >= bombTime){        
			TriggerExplode ();
		}
	}

	void TriggerExplode() {
		
		//This Destroy Call destroys the bomb you dropped once the time runs out.
		Destroy(this.gameObject);

		Instantiate (explosion, transform.position, transform.rotation);

	}
}

