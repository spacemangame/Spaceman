using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MissionController : MonoBehaviour {

	public GameObject[] hazards { get; set; }
	public GameObject[] collectibles { get; set; }
	public Gun activeGun { get; set;}
	public Vector3 spawnValues; 

	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float colStartWait;
	public float colSpawnWait;
	public float waveWait;

	public Text pointText;
	public Text hpText;
    public Text restartText;
	public Text itemText;

    private bool gameOver;
    private bool restart;

	public Mission mission { get; set; }

    void Update()
    {
        if (restart)
        {
            if (Input.GetButton("Fire1"))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    void Start(){
		mission = GameController.Instance.mission;
        gameOver = false;
        restart = false;
        restartText.text = "";

		UpdatePoints();
		UpdateHP();
		UpdateItem ();
		StartCoroutine (SpawnWaves ());
		StartCoroutine (SpawnCollectibles ());
	

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

		// TODO should be an array
		activeGun = GameController.Instance.profile.spaceship.primaryGun;

	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);

		while(true){
			for(int i=0; i < hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x),Random.Range(0, spawnValues.y)-0.5f, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;


				GameObjectObstacle gc = (GameObjectObstacle) hazard.GetComponent<GameObjectObstacle> ();
				GameObject obstacleClone = (GameObject) Instantiate (hazard, spawnPosition, spawnRotation);
				Helper.addGameObjectObstacle(obstacleClone, gc.obstacle);

				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

            if (gameOver)
            {
                restartText.text = "Click anywhere to restart";
                restart = true;
                break;
            }
        }
	}

	IEnumerator SpawnCollectibles(){
		yield return new WaitForSeconds (colStartWait);
		while (true) {
			GameObject collectible = collectibles [Random.Range (0, collectibles.Length)];
			Vector3 spawnPosition;
			Quaternion spawnRotation = Quaternion.identity;
			float x = Random.Range (-spawnValues.x, spawnValues.x);
			for (int i = 0; i < Random.Range(2,8); i++) {
				spawnPosition = new Vector3 (x, spawnValues.y, spawnValues.z + (i*2.0f));

				GameObjectCollectible gc = (GameObjectCollectible) collectible.GetComponent<GameObjectCollectible> ();
				GameObject collectibleClone = (GameObject) Instantiate (collectible, spawnPosition, spawnRotation);
				Helper.addGameObjectCollectible (collectibleClone, gc.collectible);
			}


			yield return new WaitForSeconds (colSpawnWait);

            if (gameOver)
            {
                restartText.text = "Click anywhere to restart";
                restart = true;
                break;
            }
        }
	}
		
	public void DecreasePoints(int val) {
		mission.currentCoins -= val;
		UpdatePoints ();
	}
	
	public void AddPoints(int newPointsValue){
		mission.currentCoins += newPointsValue;
		UpdatePoints ();
	}

	void UpdatePoints(){
		pointText.text = "Points: " + mission.currentCoins;
	}

	public long getHP() {
		return mission.currentHp;
	}

	public void DecreaseHP(int hpToSubtract){
		mission.currentHp -= hpToSubtract;
		UpdateHP ();
	}

	public void UpdateHP(){
		hpText.text = "HP: " + mission.currentHp;
	}

	public int getItemCount() {
		return mission.pickedItemCount;
	}

	public void UpdateItem() {
		itemText.text = mission.item.GetType ().Name + "s: " + mission.pickedItemCount + "/" + mission.targetItemCount;
	}

	public void DecreaseItem() {
		mission.pickedItemCount--;
		UpdateItem ();
		DecreasePoints (mission.item.value);
	}

    public void GameOver()
    {
        restartText.text = "Game Over!";
        gameOver = true;
    }
}
