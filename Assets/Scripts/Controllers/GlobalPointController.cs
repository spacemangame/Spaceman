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

	public bool isFirstRun { get; set;}

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		if (GlobalPointController.Instance == null) Instance = this;
	}

	void Start() {
		if (!GlobalPointController.Instance.isFirstRun) {
			GlobalPointController.Instance.isFirstRun = true;
		}

		StartCoroutine(LateStart());
	}


	IEnumerator LateStart()
	{
		yield return new WaitForSeconds(0.05f);
		Render ();
	}

	public UserProfile profile { get; set; }

	public void Render() {
		profile = GameController.Instance.profile;
		CoinText.text = profile.coins.ToString();
		MedalText.text = profile.medals.ToString();
	}
}

