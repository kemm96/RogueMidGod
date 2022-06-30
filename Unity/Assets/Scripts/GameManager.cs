using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public float turnDelay = 0.1f;
	public float levelStartDelay = 2f;

	public BoardManager boardScript;
	public int playerLivePoints = 100;
	[HideInInspector] public bool playerTurn = true;

	private List<Enemy> enemies = new List<Enemy>();
	private bool enemiesMoving;

	private int level = 0;
	private GameObject levelImage;
	private Text levelText;
	public bool doingSetup;

	private void Awake(){
		if(GameManager.instance == null){
			GameManager.instance = this;
		}else if(GameManager.instance == this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		boardScript = GetComponent<BoardManager>();
	}

	/* private void Start(){
		InitGame();
	} */

	void InitGame(){
		doingSetup = true;
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Piso " + level;
		levelImage.SetActive(true);

		enemies.Clear();
		boardScript.SetupScene(level);
		
		Invoke("HideLevelImage", levelStartDelay);

	}

	private void HideLevelImage(){
		levelImage.SetActive(false);
		doingSetup = false;
	}

	public void GameOver(){
		levelText.text = "Haz muerto en el piso " + level + ", malaya";
		levelImage.SetActive(true);
		enabled = false;
	}

	IEnumerator MoveEnemies(){
		enemiesMoving = true;
		yield return new WaitForSeconds(0.3f);

		if(enemies.Count == 0){
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i=0; i<enemies.Count; i++)
		{	
			if(enemies[i].active){
				enemies[i].MoveEnemy();
				yield return new WaitForSeconds(enemies[i].moveTime);
			}
		}

		enemiesMoving = false;
		playerTurn = true;
	}

	private void Update(){
		if (playerTurn || enemiesMoving || doingSetup) return;
		StartCoroutine(MoveEnemies());
	}

	public void addEnemytoList(Enemy enemy){
		enemies.Add(enemy);
	}

	private void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishloading;
	}	

	private void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishloading;
	}

	private void OnLevelFinishloading(Scene scene, LoadSceneMode mode){
		level++;
		InitGame();
	}
}
