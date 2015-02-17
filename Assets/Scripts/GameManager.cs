using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager instance = null;
	public LevelManager levelScript;
	public int playerEnergyPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 3;
	private List<Enemy> enemies;
	private bool enemiesMoving;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		levelScript = GetComponent<LevelManager> ();
		InitGame ();
	}

	void InitGame()
	{
		enemies.Clear ();
		levelScript.columns = 8;
		levelScript.rows = 8;
		levelScript.SetupScene (level);

	}

	public void GameOver()
	{
		enabled = true;
	}

	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving)
			return;

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

}
