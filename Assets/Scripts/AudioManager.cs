using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Singleton instance
	public static AudioManager instance;

	// Sounds
	public AudioClip fruitSlash;
	public AudioClip fruitSpawn;
	public AudioClip music;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			if (instance != this) Destroy(gameObject);
		}

		if (music != null)
		{
			PlayMusic();
		}
	}

	public void PlayMusic() {
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = music;
		audioSource.loop = true;
		audioSource.Play();
	}

	public void PauseMusic() {
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.Pause();
	}

	public void PlayFruitSlash(GameObject fruit) {
		AudioSource audioSource = fruit.AddComponent<AudioSource>();
		audioSource.clip = fruitSlash;
		audioSource.Play();
	}

	public void PlayFruitSpawn(GameObject fruit) {
		AudioSource audioSource = fruit.AddComponent<AudioSource>();
		audioSource.clip = fruitSpawn;
		audioSource.Play();
	}
}
