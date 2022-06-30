using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public int playerDamage;
	public int live;

	private Animator animator;
	private Transform target;
	private bool skipMove;
	public bool active = true;

	public AudioClip attackSound;

	protected override void Awake(){
		animator = GetComponent<Animator>();
		base.Awake();
	}

	protected override void Start(){
		GameManager.instance.addEnemytoList(this);
		target = GameObject.FindGameObjectWithTag("Player").transform;
		base.Start();
	}

	protected override void AttemptMove(int xDir, int yDir){
		base.AttemptMove(xDir, yDir);
	}

	public void MoveEnemy(){
		int xDir = 0;
		int yDir = 0;
		if(Math.Abs(target.position.x - transform.position.x) <0.1){
			yDir = target.position.y > transform.position.y ? 1 : -1;
		}else{
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
		AttemptMove(xDir, yDir);
	}

	protected override void OnCantMove(GameObject go){
		if(!active) return;
		Player hitPlayer = go.GetComponent<Player>();
		if(hitPlayer != null){
			SoundManager.instance.PlaySingle(attackSound);
			animator.SetTrigger("enemyAttack");
			hitPlayer.LoseLive(playerDamage);
		}
	}

	public void LoseLive(){
		live --;
		
		if (live <= 0){
			active = false;
			gameObject.SetActive(false);
		}
	}
}
