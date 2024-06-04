using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Singleton
	public static GameManager instance { get; private set; }

	// Variables
	public int score = 0;
	public GameObject scoreText;
	public float timeRemaining = 60.0f;
	public GameObject timerText;

	// Awake is called when the script instance is being loaded
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
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (scoreText != null)
		{
			scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
		}

		if (timerText != null)
		{
			timerText.GetComponent<TMPro.TextMeshProUGUI>().text = "Time left: " + timeRemaining.ToString("F1") + "s";
		}

		if (timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;
		}
	}

	public void DecrementScore(int value = 1)
	{
		score -= value;
	}

	public void IncrementScore(int value = 1)
	{
		score += value;
	}

	public float GetTimeRemaining()
	{
		return timeRemaining;
	}
}
