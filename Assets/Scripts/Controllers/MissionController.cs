using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MissionController : MonoBehaviour {

	public GameObject[] hazards { get; set; }
	public GameObject[] collectibles { get; set; }
	public GameObject item { get;set;}

	public Gun primaryGun { get; set;}
	public Gun secondaryGun { get; set;}
	public Vector3 spawnValues; 

	public float startWait;

	public Text pointText;
	public Text itemText;
	public Image settings;
	public Image joystick;
	public Image fireButtonPrimary;
	public Image fireButtonSecondary;

	public GameObject gameoverMenu;
	public GameObject gamesuccessMenu;

	public Button ReturnToMenu;
	public Button StartBonus;
	public Text BonusText;

	public Text coinText;
	//public Text medalText;
	public Image medalImage1;
	public Image medalImage2;
	public Image medalImage3;
	public Text itemCountText;
	public Text splashText;

    private bool gameOver;
	private int maxHP;

	private Mission mission;

	Coroutine obstacleRoutine { get; set;}
	Coroutine gameStatusRoutine {get; set;}

	public int currentWave { get; set; }

    void Start() {
		mission = GameController.Instance.mission;
        gameOver = false;
		maxHP = GameController.Instance.profile.spaceship.hp;

		UpdatePoints();
		UpdateHP();
		UpdateItem ();
		currentWave = mission.waveCount;

		obstacleRoutine = StartCoroutine (SpawnObstacles());
		gameStatusRoutine = StartCoroutine (CheckGameStatus ());

		hazards = new GameObject[mission.obstacles.Count];
		for (int i = 0; i < mission.obstacles.Count; i++) {
			Obstacle hazard = mission.obstacles [i];
			hazards[i] = (GameObject) Resources.Load(mission.obstacles[i].prefab, typeof(GameObject));
			Helper.addGameObjectObstacle (hazards [i], hazard);
		}

		collectibles = new GameObject[mission.collectibles.Count];
		for (int i = 0; i < mission.collectibles.Count; i++) {
			Collectible collectible = mission.collectibles [i];
			collectibles[i] = (GameObject) Resources.Load(collectible.prefab, typeof(GameObject));
			Helper.addGameObjectCollectible (collectibles [i], collectible);
		}

		item = (GameObject) Resources.Load(mission.item.prefab, typeof(GameObject)); 
		Helper.addGameObjectCollectible (item, mission.item);

		primaryGun = GameController.Instance.profile.spaceship.primaryGun;
		primaryGun.currentAmmo = primaryGun.ammo;
		UpdateActiveGunImages (true);

		if (mission.type != Constant.Bonus) {
			if (mission.secondaryGun != null) {
				secondaryGun = mission.secondaryGun;
				secondaryGun.currentAmmo = secondaryGun.ammo;
				UpdateActiveGunImages (false);
			} else {
				fireButtonSecondary.gameObject.SetActive (false);
			}	
		} else {
			fireButtonPrimary.gameObject.SetActive (false);
			fireButtonSecondary.gameObject.SetActive (false);
		}
	}

	public void EndSpawningRoutines() {
		StopCoroutine (obstacleRoutine);
		StopCoroutine (gameStatusRoutine);
	}

	public void onGameOver() {
		hideAllControls ();
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

	public void hideAllControls() {
		pointText.gameObject.SetActive (false);
		itemText.gameObject.SetActive (false);
		joystick.gameObject.SetActive (false);
		settings.gameObject.SetActive (false);
		fireButtonPrimary.gameObject.SetActive (false);
		fireButtonSecondary.gameObject.SetActive (false);
		ProgressBar.Instance.hideProgressBar ();
	}

	public void showAllControls() {
		pointText.gameObject.SetActive (true);
		itemText.gameObject.SetActive (true);
		joystick.gameObject.SetActive (true);
		settings.gameObject.SetActive (true);
		fireButtonPrimary.gameObject.SetActive (true);
		fireButtonSecondary.gameObject.SetActive (true);
		ProgressBar.Instance.showProgressBar ();
		if (mission.type == Constant.Bonus) {
			fireButtonPrimary.gameObject.SetActive (false);
			fireButtonSecondary.gameObject.SetActive (false);
		} else {
			if (mission.secondaryGun == null) {
				fireButtonSecondary.gameObject.SetActive (false);
			}	
		}
	}

	public void onMissionComplete() {

		hideAllControls ();
		gamesuccessMenu.SetActive (true);

		int medalsEarned = (int)System.Math.Ceiling ( (((double)mission.pickedItemCount) / mission.targetItemCount) * mission.maxMedalEarned);
		//medalText.text = "Medal(s) Earned : " + medalsEarned;
		int caseSwitch = medalsEarned;
		switch (caseSwitch)
		{
		case 1:
			medalImage1.gameObject.SetActive (true);
			break;
		case 2:
			medalImage1.gameObject.SetActive (true);
			medalImage2.gameObject.SetActive (true);
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
		string itemPickedText = mission.pickedItemCount + "/" + mission.targetItemCount + " (" + mission.item.value + " per" + itemName + ")";
		itemCountText.text = itemName + "'s collected : " + itemPickedText;

		int itemCoins = mission.pickedItemCount * mission.item.value;
		string coinsEarned =  itemCoins + (((mission.currentCoins - itemCoins) == 0) ? "": (" + " + (mission.currentCoins - itemCoins) + " = " + mission.currentCoins));
		coinText.text = "Coins Earned : " + coinsEarned;

		GameController.Instance.profile.medals += medalsEarned;

		GameController.Instance.profile.coins += mission.currentCoins;
		mission.medalEarned = medalsEarned;
		UserProfile.Save ();
	}

	IEnumerator CheckGameStatus() {
		yield return new WaitForSeconds ((float) (startWait + mission.waveWait + mission.wave.spawnWait));

		while (true) {

			if (gameOver) {
				Time.timeScale = 0;
				EndSpawningRoutines ();
				onGameOver ();
				break;
			} else {
				
				GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag ("Enemy");
				GameObject[] asteroidGameObjects = GameObject.FindGameObjectsWithTag ("asteroid");

				if (enemyGameObjects.Length + asteroidGameObjects.Length == 0) {
					Time.timeScale = 0;
					EndSpawningRoutines ();
					onMissionComplete ();

					Mission bonusMission = GameController.GetBonusMission ();

					if (bonusMission != null) {
						GameController.Instance.mission = bonusMission;
						ReturnToMenu.gameObject.SetActive (false);
						BonusText.gameObject.SetActive (true);
						StartBonus.gameObject.SetActive (true);
					} 
					break;
				} 

				yield return new WaitForSeconds (1);

			}
		}
	}

	IEnumerator SpawnObstacles(){

		yield return new WaitForSeconds (startWait);

		for(int count = mission.waveCount; count> 0;count--) {

			currentWave--;

			StartCoroutine (SpawnItems ());

			StartCoroutine (SpawnCollectibles ());

			for (int i=0; i < mission.wave.obstacleCount; i++) {
				
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x),Random.Range(0, spawnValues.y)-0.5f, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;

				GameObjectObstacle gc = (GameObjectObstacle) hazard.GetComponent<GameObjectObstacle> ();
				GameObject obstacleClone = (GameObject) Instantiate (hazard, spawnPosition, spawnRotation);
				Helper.addGameObjectObstacle(obstacleClone, gc.obstacle);

				yield return new WaitForSeconds ((float) mission.wave.spawnWait);
			}

			yield return new WaitForSeconds (mission.waveWait);
		}

	}

	IEnumerator SpawnCollectibles() {

		Quaternion spawnRotation = Quaternion.identity;

		if (mission.collectibles.Count > 0) {

			for (int i = 0; i < Random.Range (mission.collectibles.Count, mission.wave.collectibleCount); i++) {
	
				GameObject collectible = collectibles [Random.Range (0, collectibles.Length)];
				Vector3 spawnPosition;


				spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range (0, spawnValues.y) - 0.5f, spawnValues.z);

				GameObjectCollectible gc = (GameObjectCollectible)collectible.GetComponent<GameObjectCollectible> ();
				GameObject collectibleClone = (GameObject)Instantiate (collectible, spawnPosition, spawnRotation);
				Helper.addGameObjectCollectible (collectibleClone, gc.collectible);

				yield return new WaitForSeconds ((float)(mission.wave.spawnWait * mission.wave.obstacleCount) / mission.wave.collectibleCount);
			}
		}
	}

	IEnumerator SpawnItems() {
		if (mission.type == Constant.Pickup || mission.type == Constant.Bonus) {
			
			Quaternion spawnRotation = Quaternion.identity;

			for (int i = 0; i < mission.wave.itemCount; i++) {

				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), Random.Range (0, spawnValues.y) - 0.5f, spawnValues.z);

				GameObjectCollectible gc = (GameObjectCollectible)item.GetComponent<GameObjectCollectible> ();
				GameObject itemClone = (GameObject)Instantiate (item, spawnPosition, spawnRotation);
				Helper.addGameObjectCollectible (itemClone, gc.collectible);

				if (mission.type == Constant.Bonus) {
					yield return new WaitForSeconds ((float) mission.wave.spawnWait);
				} else {
					yield return new WaitForSeconds ((float)(mission.wave.spawnWait * mission.wave.obstacleCount) / mission.wave.itemCount);
				}



			}
		}

	}
		
	public void DecreasePoints(int val) {
		if (mission.currentCoins >= 0) {
			mission.currentCoins -= val;
			UpdatePoints ();
		}
	}
	
	public void AddPoints(int newPointsValue){
		mission.currentCoins += newPointsValue;
		UpdatePoints ();
	}

	void UpdatePoints(){
		pointText.text = System.String.Format(Strings.poinIndicator, mission.currentCoins);
	}

	public long getHP() {
		return mission.currentHp;
	}

	public void DecreaseHP(int hpToSubtract){
		mission.currentHp -= hpToSubtract;
		UpdateHP ();
	}

	public void UpdateHP(){
		ProgressBar.Instance.updateHpBar (mission.currentHp, maxHP);
	}

	public int getItemCount() {
		return mission.pickedItemCount;
	}

	public void UpdateItem() {
		itemText.text = System.String.Format(Strings.itemIndicator, mission.item.GetType ().Name , mission.pickedItemCount, mission.targetItemCount);
	}

	public void DecreaseItem() {
		mission.pickedItemCount--;
		UpdateItem ();
		showMessage (System.String.Format(Strings.lostItem, mission.item.GetType ().Name, mission.item.value));
		DecreasePoints (mission.item.value);
	}
	public void AddItem() {
		mission.pickedItemCount++;
		UpdateItem ();
		showMessage (System.String.Format(Strings.addItem, mission.item.GetType ().Name, mission.item.value));
	}

    public void GameOver()
    {
        gameOver = true;
    }

	public int getEnemyGunHP() {
		return mission.enemyGunHP;
	}

	private void showMessage(string message) {
		StartCoroutine(Message.show(splashText, message));
	}

	public void DecreaseBullet(bool isPrimary) {
		Gun currGun = isPrimary ? primaryGun : secondaryGun;
		if (currGun.currentAmmo > 0) {
			currGun.currentAmmo--;
			UpdateBulletCount (isPrimary);
		}

		if (currGun.currentAmmo == 0) {
			Image fb = isPrimary ? fireButtonPrimary : fireButtonSecondary;
			fb.gameObject.SetActive (false);
		}
	}

	private void UpdateBulletCount(bool isPrimary) {
		Text textCount;
		Gun currGun;
		if (isPrimary) {
			textCount = fireButtonPrimary.GetComponentInChildren<Text> ();
			currGun = primaryGun;
		} else {
			textCount = fireButtonSecondary.GetComponentInChildren<Text> ();
			currGun = secondaryGun;
		}
		textCount.text = currGun.currentAmmo >= 0 ? currGun.currentAmmo.ToString() : "∞";
	}

	private void UpdateActiveGunImages(bool isPrimary) {
		Sprite image;
		Image fireBtnSprite;
		if (isPrimary) {
			image = Resources.Load<Sprite> ("Images/" + primaryGun.texture);
			fireBtnSprite = fireButtonPrimary.GetComponentInChildren<Image> ();
		} else {
			image = Resources.Load<Sprite> ("Images/" + secondaryGun.texture);
			fireBtnSprite = fireButtonSecondary.GetComponentInChildren<Image> ();
		}

		fireBtnSprite.sprite = image;
		UpdateBulletCount (isPrimary);

	}

	public bool HasBullet(bool isPrimary) {
		if (isPrimary) {
			return primaryGun.currentAmmo != 0;
		}
		return secondaryGun.currentAmmo != 0;
	}
}
