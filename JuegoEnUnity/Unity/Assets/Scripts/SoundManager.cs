using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource sfxSource;

	public static SoundManager instance;

	private void Awake(){
		if(SoundManager.instance == null){
			SoundManager.instance = this;
		}else if(SoundManager.instance != this){
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void PlaySingle(AudioClip clip){
		sfxSource.pitch = 1f;
		sfxSource.clip = clip;
		sfxSource.Play();
	}
}
