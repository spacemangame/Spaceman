using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckPointPlayerMove : MonoBehaviour {

	public float maneuverability = 1.0f; //how fast a player can maneuver
	public float speed = 20.0f; //speed of player
	public float tilt = 4.0f; //max tilt factor
	public float checkpointReward; //seconds to add on checkpoint touchdown
	public GameObject explosion; //explosion effect to play on player colllision

	//joystick and booster button 
	public VirtualJoystick joystick;
	public BoostButton boostButton;

	private Quaternion calibrationQuaternion;

	private Rigidbody rb;
	private float boost = 1.0f;
	private CountDownTimer cTimer;

	public Image settings;
	public Text timerText;

	public GameObject gameoverMenu;
	public GameObject gamesuccessMenu;


	public Text coinText;
	public Text medalText;
	public Text itemCountText;


	public Mission mission;

	// Call this function when game is completed successfully
	public void OnGameComplete(int noOfCheckpoints, int totalCheckpoints, int itemsCollected, int coinsCollected = 0) {

		gamesuccessMenu.SetActive (true);

		int medalsEarned = ((int) ((double) itemsCollected) / mission.targetItemCount) * mission.maxMedalEarned;
		medalText.text = "Medal(s) Earned : " + medalsEarned;

		string itemName = mission.item.GetType ().Name;
		string itemPickedText = itemsCollected + "/" + mission.targetItemCount + " (" + mission.item.value + " per" + itemName + ")";
		itemCountText.text = itemName + "'s collected : " + itemPickedText;

		int itemCoins = itemsCollected * mission.item.value;
		int coinsEarned = (itemsCollected * mission.item.value) + coinsCollected;
		string coinsEarnedStr =  itemCoins + (((coinsCollected - itemCoins) == 0) ? "": (" + " + (coinsCollected - itemCoins) + " = " + coinsEarned));
		coinText.text = "Coins Earned : " + coinsEarnedStr;

		GameController.Instance.profile.medals += medalsEarned;
		GameController.Instance.profile.coins += coinsEarned;

	}

	// Call this function when game is over, 
	public void OnGameOver() {
		string reason;
		if (mission.currentHp == 0) {
			reason = Strings.wrecked;
		} else {
			reason = System.String.Format(Strings.outOfItem, mission.item.GetType().Name);
		}

		gameoverMenu.SetActive (true);
		Text gameOverReason = gameoverMenu.transform.Find("GameOverReason").GetComponent<Text>();
		gameOverReason.text = reason;
	}

	private void HideAllControls() {
		joystick.gameObject.SetActive (false);
		settings.gameObject.SetActive (false);
		timerText.gameObject.SetActive (false);
		boostButton.gameObject.SetActive (false);
	}

	void Start () {
		mission = GameController.Instance.mission;

		rb = GetComponent<Rigidbody> ();
		CalibrateAccelerometer (); //TODO should be outside of here outside, in options perhaps

		GameObject g = GameObject.Find ("TimerText");
		cTimer = g.GetComponent<CountDownTimer> ();
	}
	//function that gets called when player object collides with terrain  
	void OnCollisionEnter(Collision col){
		destroyOnTimer ();
		OnGameOver ();
	}

	//function that gets called on checkpoint touchdown
	void OnTriggerEnter(Collider other){
		cTimer.updateTimer (checkpointReward);
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}
		
	public void destroyOnTimer(){
		Destroy (gameObject);
		if(explosion != null)
			Instantiate (explosion, transform.position, transform.rotation);
		cTimer.stopTimer = true;
	}

	void FixedUpdate(){
		Vector3 moveCamTo = transform.position - transform.forward * 4.0f + Vector3.up * 3.0f;
//		float bias = 0.7f;
//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
		Camera.main.transform.position = moveCamTo;
		Camera.main.transform.LookAt (transform.position + transform.forward * 10.0f);

		Vector3 movement;
		boost = 1.0f;
		if (Input.GetButton ("Jump") || boostButton.boost) {
			boost = 2.0f;
		}

		if (joystick.InputDirection != Vector3.zero) {
			movement = new Vector3 (joystick.InputDirection.x * maneuverability, joystick.InputDirection.z * maneuverability, speed * boost);
		} else {
			if (SystemInfo.deviceType == DeviceType.Desktop) {
				movement = new Vector3 ( Input.GetAxis("Horizontal") * maneuverability, Input.GetAxis("Vertical") * maneuverability, speed * boost);
			} else {
				movement = new Vector3 ( Input.acceleration.x * maneuverability, Input.acceleration.z * maneuverability, speed * boost);
//				movement = new Vector3 (Input.acceleration.x, 0.0f, Input.acceleration.y) * accelerometerSensitivity;
			}
		}

		rb.velocity = movement;
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	// calibrates the Input.acceleration
	public void CalibrateAccelerometer () {
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}




}
