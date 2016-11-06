using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class SpaceshipImageController: MonoBehaviour
{
	// Make global
	public static SpaceshipImageController Instance {
		get;
		set;
	}

	public Vector3 scale;
	public Quaternion rotation;

	public bool isUpgradeSpaceship;

	void Start()
	{
		Instance = this;
		if (!isUpgradeSpaceship) {
			Render ();
		}
	}

	public UserProfile profile { get; set; }


	IEnumerator Show() {
		yield return new WaitForSeconds(0.01f);
		Render ();
	}

	public void Render(Spaceship ship = null) {
		try {
			foreach(Transform trans in gameObject.transform) {
				Destroy(trans.gameObject);
			}

		} catch(Exception e) {
		}

		if (GameController.Instance != null && GameController.Instance.profile != null) {
			profile = GameController.Instance.profile;

			ship = ship == null ? GameController.Instance.profile.spaceship : ship;

			GameObject spaceship = (GameObject)Resources.Load (ship.prefab, typeof(GameObject));

			spaceship =  (GameObject) Instantiate (spaceship);
			spaceship.transform.SetParent(gameObject.transform);
			spaceship.transform.localScale = scale;
			spaceship.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
			spaceship.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		} else {
			StartCoroutine (Show ());
		}


	}
}

