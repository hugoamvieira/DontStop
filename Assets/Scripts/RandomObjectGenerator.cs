/*
 * Thanks to DMGregory @
 * http://gamedev.stackexchange.com/questions/119623/set-a-chance-to-spawn-for-each-gameobject-in-an-array
 * for explanation on how to randomly pick one item of the array with weights!
 */

using System.Collections;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour
{
	[System.Serializable]
	public class SpawnableObject
	{
		public GameObject gameObj;
		public float objWeight;
	}

	// Component
	private readonly System.Random _rand = new System.Random();
	private float _totalSpawnWeight;

	public SpawnableObject[] spawnableList;
	public int lowerSpawnDistance; // The minimum distance a spawnable can appear in front of the player
	public int upperSpawnDistance; // The maximum distance a spawnable can appear in front of the player
	public int spawnChance; // This is the chance an item has of spawning. This holds x in 1/x chances.


	// Updates when user messes in inspector and once at runtime
	void OnValidate()
	{
		_totalSpawnWeight = 0f;
		foreach (var spawnable in spawnableList)
			_totalSpawnWeight += spawnable.objWeight;
	}


	void Awake()
	{
		OnValidate();
	}


	void FixedUpdate()
	{
		RandomlyInstantiateObject();
	}


	public void RandomlyInstantiateObject()
	{
		// Choose which object will be spawned from array based on DMGregory's weights explanation
		float pick = Random.value * _totalSpawnWeight;
		int index = 0;
		float cumulativeWeight = spawnableList[0].objWeight;

		while (pick > cumulativeWeight && index < spawnableList.Length - 1)
		{
			index++;
			cumulativeWeight += spawnableList[index].objWeight;
		}

		// Get the chosen object (for better code readability below)
		SpawnableObject chosenObj = spawnableList[index];

		// TODO: Spawn Chance
		if (_rand.Next(1, spawnChance + 1) == spawnChance)
		{
			// Generate object position in X axis
			var randXPos = _rand.Next(lowerSpawnDistance, upperSpawnDistance);

			// Get player X and Y positions
			var playerXPos = PlayerController.PosX;
			var playerYPos = PlayerController.PosY;

			// Add that random value to the player position
			var objXPos = randXPos + playerXPos;

			// Create a vector with the position
			Vector3 generatedObjectPosition = new Vector3(objXPos, playerYPos);

			// Set object position
			chosenObj.gameObj.transform.position = generatedObjectPosition;

			// Instantiate object as a child of RandomlyGeneratedSet
			Instantiate(chosenObj.gameObj).transform.SetParent(GameObject.Find("RandomlyGeneratedSet").transform);
		}
	}
}