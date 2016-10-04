using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameController : MonoBehaviour {

	// Make global
	public static GameController Instance {
		get;
		set;
	}

	public int HP { get; set; }

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
		Instance = this;
	}

	void Start(){
		GameController.Instance.HP = 100;
	}

}