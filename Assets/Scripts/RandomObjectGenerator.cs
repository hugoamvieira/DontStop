/*
 * Thanks to DMGregory @
 * http://gamedev.stackexchange.com/questions/119623/set-a-chance-to-spawn-for-each-gameobject-in-an-array
 * for explanation on how to randomly pick one item of the array with weights!
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Bug: Spawn doesn't verify if tiling has happened at the position it's going to spawn the object
// Bug: (cont) so sometimes objects spawn and fall off the map because there's no tile yet
public class RandomObjectGenerator : MonoBehaviour
{
	[System.Serializable]
	public class SpawnableObject
	{
		public GameObject GameObj;
		public float GameYPos; // The Y pos at which the object spawns (for non rigidbody components)
		public float ObjWeight; // The weight of the object (in comparison with others)
		public int SpawnChance; // Chance an item has of spawning. This holds x in 1/x chances
	}


	// Components
	private readonly System.Random _rand = new System.Random();
	private float _totalSpawnWeight;
	private PlayerController _playerRef;

	// Tracks the objects that were already spawned (for destruction purposes). float value is spawn x position.
	private Queue<KeyValuePair<GameObject, float>> _spawnedObjects;

	private float _lastObjSpawnXPos; // The X-axis position of the last spawned object
	private int _noOfObjects;

	public SpawnableObject[] SpawnableList; // Tracks the objects that can be spawned
	public float ObjSpawnMinDistance; // The delta variable that will not allow an object to be spawned near another
	public int LowerSpawnDistance; // Minimum distance a spawnable can appear in front of the player
	public int UpperSpawnDistance; // Maximum distance a spawnable can appear in front of the player
	public float ObjExpirationDistance; // Minimum elapsed distance at which the object will be automatically destroyed


	// Updates when user messes in inspector and once at runtime
	void OnValidate()
	{
		_totalSpawnWeight = 0f;
		foreach (var spawnable in SpawnableList)
			_totalSpawnWeight += spawnable.ObjWeight;
	}


	void Awake()
	{
		OnValidate();
		_spawnedObjects = new Queue<KeyValuePair<GameObject, float>>();
		_playerRef = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
		_lastObjSpawnXPos = 0f;
	}


	void FixedUpdate()
	{
		RandomlyInstantiateObject();
		DestroyObjectsOnDistanceExpired();
	}


	// This function picks an object from the array of spawnable objects based on its weight and spawns it
	// with "SpawnChance" of spawning.
	private void RandomlyInstantiateObject()
	{
		var spawn = true;
		// Choose which object will be spawned from array
		float pick = Random.value * _totalSpawnWeight;
		int index = 0;
		float cumulativeWeight = SpawnableList[0].ObjWeight;

		while (pick > cumulativeWeight && index < SpawnableList.Length - 1)
		{
			index++;
			cumulativeWeight += SpawnableList[index].ObjWeight;
		}

		// Get the chosen object (for better code readability below)
		SpawnableObject chosenObj = SpawnableList[index];

		if (_rand.Next(1, chosenObj.SpawnChance + 1) == chosenObj.SpawnChance)
		{
			// Generate object position in X axis
			var randXPos = _rand.Next(LowerSpawnDistance, UpperSpawnDistance + 1); // +1 because Next() is [x,y[

			// Add that random value to the player position
			var objXPos = randXPos + _playerRef.PosX;

			if (_noOfObjects > 0)
			{
				// The new object will not be created if the distance between it and the old object is less than ObjSpawnMinDistance
				var objDistanceDelta = objXPos - _lastObjSpawnXPos;
				if (objDistanceDelta < 0f || objDistanceDelta <= ObjSpawnMinDistance) return;
			}

			// Create a vector with the position
			Vector3 generatedObjectPosition = new Vector3(objXPos, chosenObj.GameYPos);

			// Set object position
			chosenObj.GameObj.transform.position = generatedObjectPosition;

			// Instantiate object as a child of RandomlyGeneratedSet
			GameObject sceneObject = Instantiate(chosenObj.GameObj);
			sceneObject.transform.SetParent(GameObject.Find("RandomlyGeneratedSet").transform);

			// Object was instantiated.
			++_noOfObjects;
			_lastObjSpawnXPos = objXPos;

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

		// Check player - object absolute distance against ObjExpirationDistance
		if (!(Mathf.Abs(_playerRef.PosX) - Mathf.Abs(oldestObj.Value) > ObjExpirationDistance)) return;

		// Destroy object and remove it from queue
		DestroyObject(oldestObj.Key);
		_spawnedObjects.Dequeue();
	}
}