using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public LevelManager levelScript;

	private int level = 12;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		levelScript = GetComponent<LevelManager> ();
		InitGame ();
	}

	void InitGame()
	{
		levelScript.columns = 8;
		levelScript.rows = 8;
		levelScript.SetupScene (level);

	}

	// Update is called once per frame
	void Update () {
	
	}
}
