using UnityEngine;

public class ShieldController : MonoBehaviour
{
	private SpriteRenderer _shieldSr;

	void Awake()
	{
		_shieldSr = gameObject.GetComponent<SpriteRenderer>();

		// Shield is deactivated (hidden) by default
		_shieldSr.enabled = false;
	}


	void FixedUpdate()
	{
		// If the player picks the shieldEnableTime power up, then the it starts showing
		_shieldSr.enabled = PlayerController.ShieldActive;
	}


	// TODO: Check shield collision
}