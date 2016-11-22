using UnityEngine;

public class ShieldController : MonoBehaviour
{
	private SpriteRenderer _shieldSr;
	private PlayerController _playerRef;

	void Awake()
	{
		_shieldSr = gameObject.GetComponent<SpriteRenderer>();
		_playerRef = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();

		// Shield is deactivated (hidden) by default
		_shieldSr.enabled = false;
	}


	void FixedUpdate()
	{
		// If the player picks the ShieldEnableTime power up, then the it starts showing
		_shieldSr.enabled = _playerRef.ShieldActive;
	}
}