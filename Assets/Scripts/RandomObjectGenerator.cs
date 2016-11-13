/*
 * Thanks to DMGregory @
 * http://gamedev.stackexchange.com/questions/119623/set-a-chance-to-spawn-for-each-gameobject-in-an-array
 * for explanation on how to randomly pick one item of the array with weights!
 */

using System.Collections.Generic;
using UnityEngine;

// Bug: Spawn doesn't verify if tiling has happened at the position it's going to spawn the object
// Bug: (cont) so sometimes objects spawn and fall off the map because there's no tile yet
public class RandomObjectGenerator : MonoBehaviour
{
	[System.Serializable]
	public class SpawnableObject
	{
		public GameObject gameObj;
		public float gameYPos; // The Y pos at which the object spawns (for non rigidbody components)
		public float objWeight; // The weight of the object (in comparison with others)
		public int spawnChance; // Chance an item has of spawning. This holds x in 1/x chances
	}


	// Components
	private readonly System.Random _rand = new System.Random();
	private float _totalSpawnWeight;

	// Tracks the objects that were already spawned (for destruction purposes). float value is spawn x position.
	private Queue<KeyValuePair<GameObject, float>> _spawnedObjects;

	public SpawnableObject[] spawnableList; // Tracks the objects that can be spawned
	public int lowerSpawnDistance; // Minimum distance a spawnable can appear in front of the player
	public int upperSpawnDistance; // Maximum distance a spawnable can appear in front of the player
	public float objExpirationDistance; // Minimum elapsed distance at which the object will be automatically destroyed


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
		_spawnedObjects = new Queue<KeyValuePair<GameObject, float>>();
	}


	void FixedUpdate()
	{
		RandomlyInstantiateObject();
		DestroyObjectsOnDistanceExpired();
	}


	// This function picks an object from the array of spawnable objects based on its weight and spawns it
	// with "spawnChance" of spawning.
	private void RandomlyInstantiateObject()
	{
		// Choose which object will be spawned from array
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

		if (_rand.Next(1, chosenObj.spawnChance + 1) == chosenObj.spawnChance)
		{
			// Generate object position in X axis
			var randXPos = _rand.Next(lowerSpawnDistance, upperSpawnDistance);

			// Get player X-axis position
			var playerXPos = PlayerController.PosX;

			// Add that random value to the player position
			var objXPos = randXPos + playerXPos;

			// Create a vector with the position
			Vector3 generatedObjectPosition = new Vector3(objXPos, chosenObj.gameYPos);

			// Set object position
			chosenObj.gameObj.transform.position = generatedObjectPosition;

			// Instantiate object as a child of RandomlyGeneratedSet
			GameObject sceneObject = Instantiate(chosenObj.gameObj);
			sceneObject.transform.SetParent(GameObject.Find("RandomlyGeneratedSet").transform);

			// Push the instantiated object and its X-axis position to the queue
			_spawnedObjects.Enqueue(new KeyValuePair<GameObject, float>(sceneObject, objXPos));
		}
	}


	// This function polls the player X-axis position every frame and determines if the first created object
	// is elligible to be destroyed from the scene.
	// Bug: Should not be using Abs. Should be checking if difference is negative (which means the player already passed the object.
	// Bug: (cont) the way it is right now, if the object spawns 60 units from the player, it will despawn before the player gets to it.
	private void DestroyObjectsOnDistanceExpired()
	{
		// Check if any object exists in the Queue
		if (_spawnedObjects.Count == 0) return;

		// Get the oldest object from the stack (which is the first, since we're working with FIFO)
		var oldestObj = _spawnedObjects.Peek();

		// Remove it from stack if it has already been destroyed
		if (oldestObj.Key == null)
			_spawnedObjects.Dequeue();

		// Check player - object absolute distance against objExpirationDistance
		if (!(Mathf.Abs(PlayerController.PosX) - Mathf.Abs(oldestObj.Value) > objExpirationDistance)) return;

		// Destroy object and remove it from queue
		DestroyObject(oldestObj.Key);
		_spawnedObjects.Dequeue();
	}
}