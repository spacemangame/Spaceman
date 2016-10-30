using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

	public Text timerText;
	public float countDownTime = 10.0f;
	private float timer;
	public bool stopTimer = false;
	private bool isBlinking = true;

	// Use this for initialization
	void Start () {
		timer = countDownTime;
//		StartCoroutine(BlinkText());
//		StartCoroutine(StopBlinking());
	}
	
	// Update is called once per frame
	void Update () {
		if (stopTimer)
			return;
		
		timer -= Time.deltaTime;
		string minutes = ((int)timer / 60).ToString();
		string seconds = ((int)timer % 60).ToString ();



		if (timer <= 5) {
			timerText.color = Color.red;
//			isBlinking = true;
		} else {
			timerText.color = Color.white;
//			isBlinking = false;
		}

		
		timerText.text = minutes + " : " + seconds;

	
		if (timer <= 0) { //logic to end the game
			stopTimer = true;
			GameObject.Find ("Player").SendMessage ("destroyOnTimer");
		}
	}

	public void updateTimer(float addTime){
		timer += addTime;
	}

	//function to blink the text 
	public IEnumerator BlinkText(){
		Debug.Log (isBlinking);
		while(isBlinking){
			Debug.Log (isBlinking);
			timerText.enabled = false;
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds(.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			timerText.enabled = true;
			yield return new WaitForSeconds(.5f); 
		}
	}


	public IEnumerator StopBlinking(){
		if(timer >= 5)
			isBlinking = false;
		yield return new WaitForSeconds(.5f);
		timerText.enabled = true;
	}
}
