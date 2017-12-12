using System.Collections.Generic;	// allows to use lists
using System;						// allows to serialise custom data
using UnityEngine;
using Random = UnityEngine.Random;	// forces to use Random class from UnityEngine namespace

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;		// columns of base grid
	public int rows = 8;		// rows of base grid
	public Count wallCount = new Count (5,9);	// how many walls we want on each level
	public Count foodCount = new Count (1,5);	// how many food items per level
	public GameObject exit;		// holders for elements
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;	// just a thing to help keep the hierarchy clean
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitialiseList(){
		gridPositions.Clear();

		// creates a list of posible positions for blocking elements
		for (int x = 1; x < columns -1; x++){
			for (int y = 1; y < rows -1; y++){
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}

	void BoardSetup(){
		boardHolder = new GameObject("Board").transform;

		//lay out the floor and outer wall tiles
		for (int x = -1; x < columns +1; x++){
			for (int y = -1; y < rows +1; y++){
				GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)];

				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f),Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range(0,gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);	// guarantees that the position wont be overwritten by a next call
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range(minimum, maximum+1);

		for (int i = 0; i < objectCount; i++){
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level){
		BoardSetup();
		InitialiseList();
		LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
		int enemyCount = (int)Mathf.Log(level,2f);
		LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
		Instantiate(exit, new Vector3(columns-1, rows-1,0f),Quaternion.identity);

	}
}