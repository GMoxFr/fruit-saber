using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	WaitingForStart,
	Playing,
	GameOver
}

public enum Difficulty
{
	Easy,
	Normal,
	Hard
}

public class GameManager : MonoBehaviour
{
	// Singleton
	public static GameManager instance { get; private set; }

	// Variables (external)
	public int score = 0;
	public GameObject scoreText;
	public float setTimer = 120.0f;
	public float timeRemaining = 0.0f;
	public GameObject timerText;
	// Variables (internal)
	public GameState gameState = GameState.WaitingForStart;
	public Difficulty difficulty = Difficulty.Normal;

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
		// Wait for 5s before starting the game
		// Invoke("StartGame", 5.0f);
		StartGame();
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

	public void StartGame(Difficulty difficulty = Difficulty.Normal)
	{
		gameState = GameState.Playing;
		// if (SceneManager.GetActiveScene().name != "Game")
		// 	SceneManager.LoadScene("Game");

		// Reset the score and timer
		score = 0;
		timeRemaining = setTimer;

		// Find the score and timer text objects
		scoreText = GameObject.FindGameObjectWithTag("ScoreText");
		timerText = GameObject.FindGameObjectWithTag("TimerText");
	}

	public Difficulty GetDifficulty()
	{
		return difficulty;
	}
}
