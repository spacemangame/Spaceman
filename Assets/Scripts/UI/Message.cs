using UnityEngine.UI;
using System.Collections;
using UnityEngine;


public static class Message {
	public static int delay = 1;

	public static IEnumerator show(Text text, string message) {
		text.text = message;
		text.gameObject.SetActive (true);
		yield return new WaitForSeconds (delay);
		text.gameObject.SetActive (false);
	}
}