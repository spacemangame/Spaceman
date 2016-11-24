using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
//	private int currentHp;
//	private int totalHp;
	public Image currentHpBar; 

	// Make global
	public static ProgressBar Instance {
		get;
		set;
	}

	void Start()
	{
		Instance = this;
		updateHpBar (100,100);

		Debug.Log ("Pregress bar started");
	}

	public void updateHpBar(int currentHp, int totalHp){
		float ratio = currentHp / (float) totalHp;
		if(ratio >= 0)
			currentHpBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		if (ratio <= 0.3) {
			currentHpBar.color = Color.red;
		}
	}

	public void showProgressBar(){
		gameObject.SetActive(true);
	}

	public void hideProgressBar(){
		gameObject.SetActive(false);
	}
}
