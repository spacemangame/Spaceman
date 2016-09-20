using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public GameObject[] collectibles;
	public Vector3 spawnValues; 

	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float colStartWait;
	public float colSpawnWait;
	public float waveWait;

	public Text scoreText;
	public Text pointText;
	public Text hpText;
    public Text restartText;

    private bool gameOver;
    private bool restart;
	private int score;
	private int points;
	private int hp;

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
		score = 0;
		points = 0;
		hp = 100;
        gameOver = false;
        restart = false;
        restartText.text = "";
        UpdateScore ();
		UpdatePoints ();
		UpdateHP ();
		StartCoroutine (SpawnWaves ());
		StartCoroutine (SpawnCollectibles ());
	}

	IEnumerator SpawnWaves(){
		yield return new WaitForSeconds (startWait);
		while(true){
			for(int i=0; i < hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x),spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
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
				Instantiate (collectible, spawnPosition, spawnRotation);
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


	public void AddScore(int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

	public void AddPoints(int newPointsValue){
		points += newPointsValue;
		UpdatePoints ();
	}

	void UpdateScore(){
		scoreText.text = "Score: " + score;
	}

	void UpdatePoints(){
		pointText.text = "Points: " + points;
	}

	public int getHP() {
		return hp;
	}

	public void DecreaseHP(int hpToSubtract){
		hp -= hpToSubtract;
		UpdateHP ();
	}

	void UpdateHP(){
		hpText.text = "HP: " + hp;
	}

    public void GameOver()
    {
        restartText.text = "Game Over!";
        gameOver = true;
    }
}
