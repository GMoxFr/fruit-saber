using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
	WaitingForStart,
	Playing,
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
	public GameObject[] difficultyPlanets;
	public SpawnBox spawnBox;
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

		if (timeRemaining > 0 && gameState == GameState.Playing)
		{
			timeRemaining -= Time.deltaTime;
		}

		if (timeRemaining <= 0 && gameState == GameState.Playing)
		{
			EndGame();
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
		this.difficulty = difficulty;

		// Hide the difficulty selection planets
		foreach (GameObject planet in difficultyPlanets)
		{
			planet.SetActive(false);
		}

		// Reset the score and timer
		score = 0;
		timeRemaining = setTimer;

		// Find the score and timer text objects
		scoreText = GameObject.FindGameObjectWithTag("ScoreText");
		timerText = GameObject.FindGameObjectWithTag("TimerText");

		// Start the fruit spawning coroutine after a short delay (2 seconds)
		StartCoroutine(StartFruitSpawning());
	}

	public void EndGame()
	{
		gameState = GameState.WaitingForStart;

		// Show the difficulty selection planets after a short delay
		StartCoroutine(ShowDifficultyPlanets());
	}

	public Difficulty GetDifficulty()
	{
		return difficulty;
	}

	public void SetDifficulty(Difficulty difficulty)
	{
		this.difficulty = difficulty;
		StartGame(difficulty);
	}

	IEnumerator ShowDifficultyPlanets()
	{
		yield return new WaitForSeconds(10.0f);

		foreach (GameObject planet in difficultyPlanets)
		{
			planet.SetActive(true);
		}
	}

	IEnumerator StartFruitSpawning()
	{
		yield return new WaitForSeconds(2.0f);

		spawnBox.StartCoroutine(spawnBox.SpawnFruits());
	}
}
