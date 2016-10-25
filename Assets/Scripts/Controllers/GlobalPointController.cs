using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class GlobalPointController: MonoBehaviour
{

	public Text CoinText;
	public Text MedalText;

	// Make global
	public static GlobalPointController Instance {
		get;
		set;
	}

	void Start()
	{
		Render ();
	}

	public UserProfile profile { get; set; }


	IEnumerator Show() {
		yield return new WaitForSeconds(0.05f);
		Render ();
	}

	public void Render() {
		if (GameController.Instance != null && GameController.Instance.profile != null) {
			profile = GameController.Instance.profile;
			CoinText.text = profile.coins.ToString ();
			MedalText.text = profile.medals.ToString ();
		} else {
			StartCoroutine (Show ());
		}
	}
}

