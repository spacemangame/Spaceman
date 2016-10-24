using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour {
	
	void Start(){
		Time.timeScale = 1;
	}
		
	public void Exit() {
		SceneManager.LoadScene("Main Menu");
	}



}
