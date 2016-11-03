using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	public static float ShieldEnableTime = 30f;
	public static float SlowMoEnableTime = 30f;
	public static float SlowMoFactoring = 0.6f;


	void OnTriggerEnter2D(Collider2D collider)
	{
		// If player collided with the SlowMo power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			PlayerController.CollidedSlowmo = true;
		}

		// Player collided with Shield power up
		else
		{
			PlayerController.CollidedShield = true;
		}
	}


	void OnTriggerExit2D(Collider2D collider)
	{
		// If player ended collision with the SlowMo Power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			PlayerController.CollidedSlowmo = false;
		}

		// Player ended collision with Shield power up
		else
		{
			PlayerController.CollidedShield = false;
		}

		// Destroy object
		DestroyObject(gameObject);
	}
}