using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	private PlayerController _playerRef;

	public static float ShieldEnableTime { get; set; }
	public static float SlowmoEnableTime { get; set; }
	public static float SlowmoFactor { get; set; }


	void Awake()
	{
		_playerRef = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		// If player collided with the SlowMo power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			_playerRef.CollidedSlowmoPowerup = true;
		}

		// Player collided with Shield power up
		else if (gameObject.name.Contains("ShieldPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			_playerRef.CollidedShieldPowerup = true;
		}
	}


	void OnTriggerExit2D(Collider2D collider)
	{
		// If player ended collision with the SlowMo Power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			_playerRef.CollidedSlowmoPowerup = false;
		}

		else if (gameObject.name.Contains("ScoreObject") && collider.gameObject.name.Equals("Player"))
		{
			_playerRef.ObjectsCollected++;
		}

		// Player ended collision with Shield power up
		else
		{
			_playerRef.CollidedShieldPowerup = false;
		}

		// Destroy object
		DestroyObject(gameObject);
	}
}