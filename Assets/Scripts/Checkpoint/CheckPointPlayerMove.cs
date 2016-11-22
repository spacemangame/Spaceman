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
	public float maxAltitude = 100.0f; //limiting player's movement on y-axis
	//joystick and booster button 
	public VirtualJoystick joystick;
	public BoostButton boostButton;
	public int hpHit = 20;

	private Quaternion calibrationQuaternion;

	private Rigidbody rb;
	private float boost = 1.0f;
	private CountDownTimer cTimer;

	public Image settings;
	public Text timerText;
	public Text itemCountText;

	public GameObject gameoverMenu;
	public GameObject gamesuccessMenu;


	public Text coinText;
	//public Text medalText;
	public Image medalImage1;
	public Image medalImage2;
	public Image medalImage3;
	public Text drugCountText;


	public Mission mission {get; set;}
	private int noOfCheckpoints, totalCheckpoints, itemsCollected, coinsCollected;
	private int maxHP;
	private Vector3 dir;
	private Vector3 _InputDir;


	public GameObject spaceship { get; set; }

	// Call this function when game is completed successfully
	public void OnGameComplete(int noOfCheckpoints, int totalCheckpoints, int itemsCollected, int coinsCollected = 0) {


		HideAllControls ();

		cTimer.stopTimer = true;
		gamesuccessMenu.SetActive (true);

		int medalsEarned = (int) System.Math.Ceiling((((double) itemsCollected) / mission.targetItemCount ) * mission.maxMedalEarned);
		//medalText.text = "Medal(s) Earned : " + medalsEarned;
		Debug.Log(medalsEarned);
		int caseSwitch = medalsEarned;
		switch (caseSwitch)
		{
		case 1:
			medalImage1.gameObject.SetActive (true);
			medalImage2.gameObject.SetActive (false);
			medalImage3.gameObject.SetActive (false);
			break;
		case 2:
			medalImage1.gameObject.SetActive (true);
			medalImage2.gameObject.SetActive (true);
			medalImage3.gameObject.SetActive (false);
			break;
		case 3:
			medalImage1.gameObject.SetActive (true);
			medalImage2.gameObject.SetActive (true);
			medalImage3.gameObject.SetActive (true);
			break;
		default:
			medalImage1.gameObject.SetActive (false);
			medalImage2.gameObject.SetActive (false);
			medalImage3.gameObject.SetActive (false);
			break;
		}

		string itemName = mission.item.GetType ().Name;
		string itemPickedText = itemsCollected + "/" + mission.targetItemCount + " (" + mission.item.value + " per" + itemName + ")";
		itemCountText.text = itemName + "'s collected : " + itemPickedText;

		int itemCoins = itemsCollected * mission.item.value;
		int coinsEarned = (itemsCollected * mission.item.value) + coinsCollected;
		string coinsEarnedStr = coinsEarned + "";
		coinText.text = "Coins Earned : " + coinsEarnedStr;

		GameController.Instance.profile.medals += medalsEarned;
		GameController.Instance.profile.coins += coinsEarned;

		UserProfile.Save();
	}

	// Call this function when game is over, 
	public void OnGameOver(string reason) {
		HideAllControls ();
		gameoverMenu.SetActive (true);
		Text gameOverReason = gameoverMenu.transform.Find("GameOverReason").GetComponent<Text>();
		gameOverReason.text = reason;
	}

	public void HideAllControls() {
		joystick.gameObject.SetActive (false);
		settings.gameObject.SetActive (false);
		timerText.gameObject.SetActive (false);
		boostButton.gameObject.SetActive (false);
		drugCountText.gameObject.SetActive (false);
		ProgressBar.Instance.hideProgressBar ();
	}

	public void ShowAllControls() {
		joystick.gameObject.SetActive (true);
		settings.gameObject.SetActive (true);
		timerText.gameObject.SetActive (true);
		boostButton.gameObject.SetActive (true);
		drugCountText.gameObject.SetActive (true);
		ProgressBar.Instance.showProgressBar ();
	}

	void Start () {
		mission = GameController.Instance.mission;
		Debug.Log ("HP: " + mission.currentHp);
		maxHP = GameController.Instance.profile.spaceship.hp;
		rb = GetComponent<Rigidbody> ();
		CalibrateAccelerometer (); //TODO should be outside of here outside, in options perhaps

		GameObject g = GameObject.Find ("TimerText");
		cTimer = g.GetComponent<CountDownTimer> ();

		noOfCheckpoints = 0;
		totalCheckpoints = 10;
		itemsCollected = 0;

		/*spaceship = (GameObject) Resources.Load(GameController.Instance.profile.spaceship.prefab, typeof(GameObject));
		spaceship =  (GameObject) Instantiate (spaceship);

		spaceship.transform.SetParent(gameObject.transform);
		spaceship.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 180.0f);
		spaceship.transform.localScale = new Vector3 (40.0f, 30.0f, 40.0f);

		spaceship.transform.localPosition = new Vector3 (60.0f, 0.0f, 60.0f);*/

		UpdateDrugCount (false);
	
	}

	public void UpdateDrugCount(bool playSound) {
		if (playSound) {
			AudioSource[] audios = GetComponents<AudioSource> ();
			audios [1].Play ();
		}
		drugCountText.text = "Drugs : " + itemsCollected + "/" + mission.targetItemCount;
	}

	//function that gets called when player object collides with terrain  
	void OnCollisionEnter(Collision col){
		//reduce hp
		DecreaseHP(hpHit);
		if (mission.currentHp <= 0 || col.gameObject.tag.Equals ("Terrain")) {
			destroyOnTimer (Strings.wrecked);
		}
		if (!col.gameObject.tag.Equals ("Terrain")) {
			Destroy (col.gameObject);
			AudioSource[] audios = GetComponents<AudioSource> ();
			audios [2].Play ();
		}
	}

	//function that gets called on checkpoint touchdown
	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "mc_trigger")
			OnGameComplete (noOfCheckpoints, totalCheckpoints, itemsCollected, 0);
		else if (other.gameObject.tag == "drug") {
			itemsCollected++;
			UpdateDrugCount(true);
			Destroy (other.gameObject);
		} else {
			noOfCheckpoints++;
			cTimer.updateTimer (checkpointReward);
			Destroy (other.gameObject);
			AudioSource[] audios = GetComponents<AudioSource>();
			audios[0].Play();
		}
		Debug.Log ("Items collected" + itemsCollected);
	}
		
	public void destroyOnTimer(string reason){
		Destroy (gameObject);
		if (explosion != null)
			Instantiate (explosion, transform.position, transform.rotation);
		cTimer.stopTimer = true;
		OnGameOver (reason);
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
			movement = new Vector3 (joystick.InputDirection.x * maneuverability, joystick.InputDirection.y * maneuverability, speed * boost);
		} else {
			if (SystemInfo.deviceType == DeviceType.Desktop) {
				movement = new Vector3 ( Input.GetAxis("Horizontal") * maneuverability, Input.GetAxis("Vertical") * maneuverability, speed * boost);
			} else{
				_InputDir = getAccelerometer(Input.acceleration);
				movement = new Vector3 ( _InputDir.x * maneuverability * 2.0f, _InputDir.z  * maneuverability * 2.0f, speed * boost);
			}
		}
		if(rb.position.y > maxAltitude)
			rb.position = new Vector3 (rb.position.x, maxAltitude, rb.position.z);
		rb.velocity = movement;
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

	}

//	private void checkBoundary() {
//		if(rb.position.y > maxAltitude)
//			rb.position = new Vector3 (rb.position.x, maxAltitude, rb.position.z);
//		// make sure player doesn't go out of the boundary.
//	}
	// calibrates the Input.acceleration
	public void CalibrateAccelerometer () {
		dir = Vector3.zero;
		dir = Input.acceleration;
		if (dir.sqrMagnitude > 1)
			dir.Normalize();

//		Vector3 accelerationSnapshot = Input.acceleration;
//		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
//		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	//Method to get the calibrated input 
	Vector3 getAccelerometer(Vector3 accelerator){
		Vector3 accel = Input.acceleration;
		accel.x = accel.x - dir.x;
		accel.y = accel.y - dir.y;
		accel.z = accel.z - dir.z;
		return accel;
	}


	public void DecreaseHP(int hpToSubtract){
		mission.currentHp -= hpToSubtract;
		UpdateHP ();
	}

	public void UpdateHP(){
		ProgressBar.Instance.updateHpBar (mission.currentHp, maxHP);
	}


}
