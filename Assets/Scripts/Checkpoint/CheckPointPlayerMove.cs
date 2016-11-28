using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

	public bool VRMode { get; set; }

	public GameObject mainCanvas;
	public GameObject scoreVRCanvas;
	public GameObject mainCamera;
	public GameObject gvrViewerMain;
	public GameObject gvrReticle;

	public GameObject terrain;
	public GameObject checkpoints;
	public GameObject collectibles;
	public GameObject obstacles;
	public GameObject movingObstacles;

	public bool gameStarted = false;
	public bool gameOver = false;

	public bool initialised = false;

	private Quaternion calibrationQuaternion;

	private Rigidbody rb;
	private float boost = 1.0f;
	private CountDownTimer cTimer;

	public Image settings;
	public Text timerText;
	public Text itemCountText;
	public ProgressBar progressBar;

	public Text vrTimerText;
	private CountDownTimer vrCTimer;
	public ProgressBar vrProgressBar;

	public GameObject gameoverMenu;
	public GameObject gamesuccessMenu;
	public GameObject gameStartMenu;
	public GameObject delayedStartMenu;
	public Text delayedCountText;

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

	private float startCount = 3.0f;

	public UserProfile profile { get; set; }

	public GameObject spaceship { get; set; }

	// Call this function when game is completed successfully
	public void OnGameComplete(int noOfCheckpoints, int totalCheckpoints, int itemsCollected, int coinsCollected = 0) {
		gameOver = true;

		HideAllControls ();

		cTimer.stopTimer = true;
		gamesuccessMenu.SetActive (true);

		int medalsEarned = (int) System.Math.Ceiling((((double) itemsCollected) / mission.targetItemCount ) * mission.maxMedalEarned);

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


	public void ToggleVRMode() {

		Debug.Log ("Toggle VR called");

		if (!initialised) return;

		if (GameController.Instance.profile.isVREnabled == null)
			GameController.Instance.profile.isVREnabled = false;

		GameController.Instance.profile.isVREnabled = !GameController.Instance.profile.isVREnabled;

		Debug.Log ("VR Enabled : " + GameController.Instance.profile.isVREnabled);

		UserProfile.Save ();

		GameController.Instance.missions = DataGenerator.GenerateMissions();

		GameController.Instance.mission = GameController.Instance.missions.Find(x => x.id == GameController.Instance.mission.id);
		GameController.Instance.mission.secondaryGun = GameController.Instance.profile.secondaryGun;

		GameController.StartMission ();
	}

	public void AdjustCanvas() {
		//return;
		if (VRMode) {

			Canvas canvas = mainCanvas.GetComponent<Canvas> ();
			canvas.renderMode = RenderMode.WorldSpace;

			Vector3 transform;
			if (!gameStarted) {
				transform = new Vector3 (0f, 1, 140);
			} else {
				transform = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 150);
			}

			mainCanvas.transform.position = transform;

			float scale = 0.2366001f;
			mainCanvas.transform.localScale = new Vector3 (scale, scale, scale);


			gvrReticle.SetActive (true);
		}
	}

	public void resetCanvas() {
		
		if (VRMode) {
			gvrReticle.SetActive (false);
			Canvas canvas = mainCanvas.GetComponent<Canvas> ();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			mainCanvas.transform.position = new Vector3 (679f, 382f, 20f);
			mainCanvas.transform.localScale = new Vector3 (1.6975f, 1.6975f, 1f);
		}
	}

	// Call this function when game is over, 
	public void OnGameOver(string reason) {
		cTimer.stopTimer = true;
		gameOver = true;
		HideAllControls ();
		gameoverMenu.SetActive (true);
		Text gameOverReason = gameoverMenu.transform.Find("GameOverReason").GetComponent<Text>();
		gameOverReason.text = reason;
	}

	public void HideAllControls() {
		try {
			joystick.gameObject.SetActive (false);
			settings.gameObject.SetActive (false);
			timerText.gameObject.SetActive (false);
			boostButton.gameObject.SetActive (false);
			drugCountText.gameObject.SetActive (false);
			progressBar.hideProgressBar ();

		} catch(System.Exception e) {
			Debug.Log (e.Message);
		}

		if (VRMode && gameOver)
			hideMap ();

		AdjustCanvas ();
	}

	public void ShowAllControls() {
		resetCanvas ();
		if (!VRMode) {
			joystick.gameObject.SetActive (true);
			settings.gameObject.SetActive (true);
			boostButton.gameObject.SetActive (true);
			drugCountText.gameObject.SetActive (true);
		}

		timerText.gameObject.SetActive (true);
		progressBar.showProgressBar ();
	}

	private void instantiateVRPrefabs() {
		gvrViewerMain = Instantiate ((GameObject)Resources.Load ("GvrViewerMain", typeof(GameObject)));

		GameObject eventSystem = EventSystem.current.gameObject;
		StandaloneInputModule standaloneInputModule = eventSystem.GetComponent<StandaloneInputModule> ();
		DestroyImmediate (standaloneInputModule);

		eventSystem.AddComponent<GazeInputModule> ();
		eventSystem.AddComponent<StandaloneInputModule>();

		gvrReticle = Instantiate ((GameObject)Resources.Load ("GvrReticle", typeof(GameObject)));
		gvrReticle.transform.SetParent (Camera.main.gameObject.transform);

		scoreVRCanvas = GameObject.Find ("GameScoreCanvas");

	}

	void Start () {

		//GvrViewer.Instance.VRModeEnabled = false;

		mission = GameController.Instance.mission;
		profile = GameController.Instance.profile;

		VRMode = profile.isVREnabled;

		maxHP = GameController.Instance.profile.spaceship.hp;
		rb = GetComponent<Rigidbody> ();

		GameObject g = GameObject.Find ("TimerText");
		cTimer = g.GetComponent<CountDownTimer> ();
		cTimer.stopTimer = true;

		if (VRMode) {

			instantiateVRPrefabs ();

			g = GameObject.Find ("VRTimerText");
			vrTimerText = g.GetComponent<Text> ();
			vrCTimer = g.GetComponent<CountDownTimer> ();
			vrCTimer.stopTimer = true;

			g = GameObject.Find ("VRProgressBar");
			g = g.transform.Find ("ProgressBarBG").gameObject;
			vrProgressBar = g.GetComponent<ProgressBar> ();

			gvrViewerMain.SetActive (false);
			GvrViewer.Instance.VRModeEnabled = VRMode;
		}

		noOfCheckpoints = 0;
		totalCheckpoints = 10;
		itemsCollected = 0;



		if (VRMode) {
			gvrViewerMain.SetActive (true);
			Debug.Log ("This is VR mode");
			if (mission.restarted) {
				scoreVRCanvas.SetActive (false);
				gameStartMenu.SetActive (true);
				HideAllControls ();
				StartGame ();
			} else {
				showStartScreen ();
				Debug.Log ("Invoking start screen");
			}
		} else {

			delayedStart ();

			//delayedStartMenu.SetActive (true);
			//drugCountText.gameObject.SetActive (false);
			//progressBar.hideProgressBar ();
			//Invoke ("delayedStart", 3.0f);
		}
	}

	void Update() {
		startCount -= Time.deltaTime;
		string seconds = ((int)startCount % 60).ToString ();

		delayedCountText.text = seconds;
	}

	public void delayedStart() {
		
		delayedStartMenu.SetActive (false);
		UpdateDrugCount (false);
		gameStarted = true;
		resetCanvas ();
		cTimer.StartTimer ();
		progressBar.showProgressBar ();
		drugCountText.gameObject.SetActive (true);
	}

	public void showStartScreen() {

		cTimer.stopTimer = true;
		vrCTimer.stopTimer = true;
		scoreVRCanvas.SetActive (false);
		gameStartMenu.SetActive (true);
		HideAllControls ();
		hideMap ();
	}

	public void showMap() {
		terrain.SetActive (true);
		checkpoints.SetActive (true);
		collectibles.SetActive (true);
		scoreVRCanvas.SetActive (true);
		if (obstacles != null) {
			obstacles.SetActive (true);
			movingObstacles.SetActive (true);
		}
	}

	public void hideMap() {
		terrain.SetActive (false);
		checkpoints.SetActive (false);
		collectibles.SetActive (false);
		scoreVRCanvas.SetActive (false);

		if (obstacles != null) {
			obstacles.SetActive (false);
			movingObstacles.SetActive (false);
		}
	}

	public void StartGame() {
		scoreVRCanvas.SetActive (true);

		cTimer = vrCTimer;
		timerText = vrTimerText;
		progressBar = vrProgressBar;
		cTimer.stopTimer = false;
		UpdateDrugCount (false);

		gameStartMenu.SetActive (false);
		showMap ();
		ShowAllControls ();
		CalibrateAccelerometer ();
		gameStarted = true;
		cTimer.StartTimer ();
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

		if (gameStarted) {
			//reduce hp
			DecreaseHP (hpHit);
			if (mission.currentHp <= 0 || col.gameObject.tag.Equals ("Terrain")) {
				destroyOnTimer (Strings.wrecked);
			}
			if (!col.gameObject.tag.Equals ("Terrain")) {
				Destroy (col.gameObject);
				AudioSource[] audios = GetComponents<AudioSource> ();
				audios [2].Play ();
			}
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
	}

	public void destroyOnTimer(string reason){
		if (gameStarted) {

			Destroy (gameObject);
			if (explosion != null)
				Instantiate (explosion, transform.position, transform.rotation);
			cTimer.stopTimer = true;
			OnGameOver (reason);
		}
	}

	void FixedUpdate() {

		//Switch to nonVR mode
		if (VRMode && GvrViewer.Instance.BackButtonPressed) {
			initialised = true;
			ToggleVRMode ();
			return;
		}

		if (!gameStarted || gameOver)
			return;

		Vector3 moveCamTo = transform.position - transform.forward * 4.0f + Vector3.up * 3.0f;
		//		float bias = 0.7f;
		//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);

		Camera.main.transform.position = moveCamTo;
		Camera.main.transform.LookAt (transform.position + transform.forward * 10.0f);
		Vector3 movement;

		if (VRMode) {
			scoreVRCanvas.transform.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 150f);
		}
		boost = 1.0f;

		if (VRMode && GvrViewer.Instance.Triggered) {
			boost = 2.0f;
			boostButton.pressBoost ();
		} else if (Input.GetButton ("Jump") || boostButton.boost) {
			boost = 2.0f;
		}

		if (VRMode) {
			_InputDir = getAccelerometer (Input.acceleration);
			movement = new Vector3 (_InputDir.x * maneuverability * 2.0f, _InputDir.z * maneuverability * 2.0f, speed * boost);
		} else {
			movement = new Vector3 (joystick.InputDirection.x * maneuverability, joystick.InputDirection.y * maneuverability, speed * boost);
		}

		if (SystemInfo.deviceType == DeviceType.Desktop) {
			movement = new Vector3 ( Input.GetAxis("Horizontal") * maneuverability, Input.GetAxis("Vertical") * maneuverability, speed * boost);
		} 

		if (rb.position.y > maxAltitude)
			rb.position = new Vector3 (rb.position.x, maxAltitude, rb.position.z);
		rb.velocity = movement;
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

	}
	public void CalibrateAccelerometer () {
		dir = Vector3.zero;
		dir = Input.acceleration;
		if (dir.sqrMagnitude > 1)
			dir.Normalize();
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
		progressBar.updateHpBar (mission.currentHp, maxHP);
	}


}
