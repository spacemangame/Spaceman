using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Collections.Generic;

public class MessageController: MonoBehaviour
{
	public Text MessageText;

	// Make global
	public static MessageController Instance {
		get;
		set;
	}

	void Start() {
		Instance = this;
		this.gameObject.SetActive (false);
	}
		
	public void Render(string message, int delay = 1) {
		this.gameObject.SetActive (true);
		MessageText.text = message;
		StartCoroutine (ClearMessage ());
	}

	public static int delay = 1;

	IEnumerator ClearMessage() {
		yield return new WaitForSeconds (delay);
		this.gameObject.SetActive (false);
	}
}

