using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

	public int damage = 1;
	public int pointsPerHeart = 10;
	public float restartLevelDelay = 1f;
	public Text liveText;

	public AudioClip liveSound, gameOverSound, attackSound;

	private Animator animator;
	private int live;

	protected override void Awake(){
		animator = GetComponent<Animator>();
		base.Awake();
	}

	protected override void Start(){
		liveText = GameObject.Find("LiveText").GetComponent<Text>();
		live = GameManager.instance.playerLivePoints;
		liveText.text = "Vida: " + live;
		base.Start();
	}

	private void OnDisable(){
		GameManager.instance.playerLivePoints = live;
	}

	void CheckIfGameOver(){
		if(live <= 0) {
			SoundManager.instance.PlaySingle(gameOverSound);
			GameManager.instance.GameOver();
		}
	}

	protected override void AttemptMove(int xDir, int yDir){
		live --;
		liveText.text = "Vida: " + live;
		base.AttemptMove(xDir, yDir);
		GameManager.instance.playerTurn = false;
	}

	void Update(){
		if(!GameManager.instance.playerTurn || GameManager.instance.doingSetup) return;

		int horizontal;
		int vertical;

		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical = (int)Input.GetAxisRaw("Vertical");

		if(horizontal!=0) vertical = 0;

		if(horizontal!=0 || vertical!=0) AttemptMove(horizontal, vertical);
	}
	
	protected override void OnCantMove(GameObject go){
		Enemy hitEnemy = go.GetComponent<Enemy>();
		
		if(hitEnemy != null){
			SoundManager.instance.PlaySingle(attackSound);
			animator.SetTrigger("playerAttack");
			hitEnemy.LoseLive();
		}
	}

	void Restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoseLive(int hit){
		live -= hit;
		liveText.text = "-"+hit+" Vida: " + live;
		CheckIfGameOver();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Exit")){
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		}else if(other.CompareTag("Live")){
			SoundManager.instance.PlaySingle(liveSound);
			live += pointsPerHeart;
			liveText.text = "+"+pointsPerHeart+" Vida: " + live;
			other.gameObject.SetActive(false);
		}
	}
}
