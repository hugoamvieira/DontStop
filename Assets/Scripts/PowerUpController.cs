using UnityEngine;

public class PowerUpController : MonoBehaviour
{
	public static float ShieldEnableTime { get; set; }
	public static float SlowmoEnableTime { get; set; }
	public static float SlowmoFactor { get; set; }

	void OnTriggerEnter2D(Collider2D collider)
	{
		// If player collided with the SlowMo power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			PlayerController.CollidedSlowmoPowerup = true;
		}

		// Player collided with Shield power up
		else
		{
			PlayerController.CollidedShieldPowerup = true;
		}
	}


	void OnTriggerExit2D(Collider2D collider)
	{
		// If player ended collision with the SlowMo Power Up
		if (gameObject.name.Contains("SlowMoPowerUp") && collider.gameObject.name.Equals("Player"))
		{
			PlayerController.CollidedSlowmoPowerup = false;
		}

		// Player ended collision with Shield power up
		else
		{
			PlayerController.CollidedShieldPowerup = false;
		}

		// Destroy object
		DestroyObject(gameObject);
	}
}