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

	void Start()
	{
		Instance = this;
		Render ();
	}

	public UserProfile profile { get; set; }


	IEnumerator Show() {
		yield return new WaitForSeconds(0.01f);
		Render ();
	}

	public void Render() {
		if (GameController.Instance != null && GameController.Instance.profile != null) {
			profile = GameController.Instance.profile;

			GameObject spaceship = (GameObject) Resources.Load(GameController.Instance.profile.spaceship.prefab, typeof(GameObject));
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

