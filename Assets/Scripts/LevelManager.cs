using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

	[Serializable] public class Count{
		public int minimum;
		public int maximum;

		public Count(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int rows = 18;
	public int columns = 18;
	public Count waterCount = new Count (5, 9);
	public Count itemCount = new Count (1, 5);
	public GameObject exit;
	public GameObject[] grassTiles;
	public GameObject[] waterTiles;
	public GameObject[] enemyTiles;
	public GameObject[] itemTiles;
	public GameObject[] outerWallTiles;

	private Transform levelHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList()
	{
		gridPositions.Clear();

		for(int x=1; x<rows-1; x++)
			for(int y=1; y<columns-1; y++)
				gridPositions.Add(new Vector3(x,y,0f));
	}

	void LevelSetup()
	{
					levelHolder = new GameObject ("Level").transform;

					for (int x=-1; x<rows+1; x++)
			for (int y=-1; y<columns+1; y++) {
			GameObject toInstantiate = grassTiles[Random.Range(0, grassTiles.Length)];

			if( x == -1 || x == columns || y == -1 || y == rows)
				toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

			GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
			instance.transform.SetParent(levelHolder);
		}

	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum+1);
		for (int i = 0; i< objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}
	 
	public void SetupScene (int level) {
		LevelSetup ();
		InitializeList ();

		LayoutObjectAtRandom (waterTiles, waterCount.minimum, waterCount.maximum);
		LayoutObjectAtRandom (itemTiles, itemCount.minimum, itemCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}

}
