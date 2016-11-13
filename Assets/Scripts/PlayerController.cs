using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region VARIABLES

	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;
	private SpriteRenderer _playerSr;

	// Anim states
	private const string AnimParamName = "AnimState";
	private const int RunAnimState = 1;
	private const int JumpAnimState = 2;
	private const int CrouchAnimState = 3;

	// Forces
	public float walkForce;
	public float jumpForce;

	// Max speeds
	public float maxSpeedX;
	public float maxSpeedY;

	// Flags
	public bool isJumping;
	public bool isCrouching;
	public int respawnTimer;
	public float respawnTransparency;

	// Player Environment
	private static float _posX;

	public static float PosX
	{
		get { return _posX; }
		set { _posX = value; }
	}

	private static float _posY;

	public static float PosY
	{
		get { return _posY; }
		set { _posY = value; }
	}

	private static bool _collidedShieldPowerup;

	public static bool CollidedShieldPowerup
	{
		get { return _collidedShieldPowerup; }
		set { _collidedShieldPowerup = value; }
	}

	private static bool _collidedSlowmoPowerup;

	public static bool CollidedSlowmoPowerup
	{
		get { return _collidedSlowmoPowerup; }
		set { _collidedSlowmoPowerup = value; }
	}

	private static bool _slowmoActive;

	public static bool SlowmoActive
	{
		get { return _slowmoActive; }
		set { _slowmoActive = value; }
	}

	private static bool _shieldActive; // Getter for shieldEnableTime active player property

	public static bool ShieldActive
	{
		get { return _shieldActive; }
		set { _shieldActive = value; }
	}

	private static bool _gameOver; // Determines whether game is over or not

	public static bool GameOver
	{
		get { return _gameOver; }
		set { _gameOver = value; }
	}

	private static float _distanceElapsed;

	public static float DistanceElapsed
	{
		get { return _distanceElapsed; }
		set { _distanceElapsed = value; }
	}

	private static int _playerScore;

	public static int PlayerScore
	{
		get { return _playerScore; }
		private set { _playerScore = value; }
	}

	// Power-ups' / Score variables (Because otherwise you would only be able to set these on the powerups which are generated
	// at runtime)
	private int _score;
	private float _collisionDecel;
	public float shieldEnableTime;
	public float slowmoEnableTime;
	public float slowmoFactor;

	// Player Colliders
	[SerializeField] private PolygonCollider2D[] _playerColliders;
	[SerializeField] private int _currentColliderIndex;

	#endregion

	void Awake()
	{
		// Get Components
		_anim = gameObject.GetComponent<Animator>();
		_playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
		_playerSr = gameObject.GetComponent<SpriteRenderer>();

		// Set player Y Position. It's here because I only need this property for the power ups
		// If it was on the Update method, power ups would spawn in the air whenever the player jumped.
		PosY = transform.position.y;

		// Set score
		PlayerScore = 0;

		// Set the power-ups properties (didn't want to make classes for each ¯\_(ツ)_/¯)
		PowerUpController.ShieldEnableTime = shieldEnableTime;
		PowerUpController.SlowmoEnableTime = slowmoEnableTime;
		PowerUpController.SlowmoFactor = slowmoFactor;

		// Playable state
		GameOver = false;

		// Set score for each successful jump / crouch
		_score = 100;

		// Collision deceleration
		_collisionDecel = -2f;
	}


	void FixedUpdate()
	{
		// Update Position
		if (gameObject.transform.position.x > 0)
			DistanceElapsed = Mathf.Abs(gameObject.transform.position.x);

		// Set/Update player X position
		PosX = transform.position.x;

		// Engage endless running mode
		if (_playerRigidbody.velocity.x <= maxSpeedX)
		{
			_playerRigidbody.AddForce(new Vector2(walkForce, 0));
		}


		// Handle user input
		if (Input.GetKey(KeyCode.Space) || TouchController.SwipedUp())
		{
			if (isCrouching) return;

			// If the player's Y-axis speed doesn't exceed the set max Y speed
			// and if the player is on the ground (ie. Y-axis velocity == 0):
			if (_playerRigidbody.velocity.y <= maxSpeedY && _playerRigidbody.velocity.y == 0f)
			{
				// Change to jump animation and add jumping force
				_anim.SetInteger(AnimParamName, JumpAnimState);
				_playerRigidbody.AddForce(new Vector2(0, jumpForce));
			}
		}

		else if (Input.GetKey(KeyCode.S) || TouchController.SwipedDown())
		{
			if (isJumping) return;

			// Change to crouch animation
			_anim.SetInteger(AnimParamName, CrouchAnimState);
		}

		else
		{
			// Running animation here due to nature of FixedUpdate()
			_anim.SetInteger(AnimParamName, RunAnimState);
		}

		// Check collision with power ups
		if (CollidedShieldPowerup)
		{
			StartCoroutine("ActivatePlayerShield");
		}
		else if (CollidedSlowmoPowerup)
		{
			StartCoroutine("ActivatePlayerSlowmo");
		}
	}


	void OnCollisionEnter2D(Collision2D collisionObject)
	{
		// Ignore collision if the player didn't collide with an obstacle
		if (!collisionObject.gameObject.name.Contains("Obstacle")) return;

		if (!ShieldActive)
			GameOver = true;

		else
		{
			DeactivatePlayerShield();

			var newPlayerPosX = collisionObject.transform.position.x +
			                    collisionObject.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x + 0.2f;

			// Slow player speed down
			_playerRigidbody.AddForce(new Vector2(_collisionDecel, 0));

			// Spawn the player on the other side of the box
			gameObject.transform.position = new Vector3(newPlayerPosX, PosY, 0);

			// Give that respawn effect
			StartCoroutine("RespawnPlayer");
		}
	}


	void OnTriggerEnter2D(Collider2D triggerObject)
	{
		if (!triggerObject.gameObject.name.Contains("Obstacle")) return;
		PlayerScore += _score;
	}


	// Changes colliders based on animation
	public void SetSpriteCollider(int spriteFrame)
	{
		_playerColliders[_currentColliderIndex].enabled = false;
		_currentColliderIndex = spriteFrame;
		_playerColliders[_currentColliderIndex].enabled = true;
	}


	// Gives that cool respawn effect by switching the transparency on the player
	private IEnumerator RespawnPlayer()
	{
		_playerSr.color = new Color(_playerSr.color.r, _playerSr.color.g, _playerSr.color.b, respawnTransparency);
		yield return new WaitForSeconds(respawnTimer);

		_playerSr.color = new Color(_playerSr.color.r, _playerSr.color.g, _playerSr.color.b, 1);
	}


	// This method activates the player shieldEnableTime, in which the player becomes invincible
	// for ShieldEnableTime seconds or until it collides with an object, whichever comes first.
	private IEnumerator ActivatePlayerShield()
	{
		if (ShieldActive) yield break;
		ShieldActive = true;

		Debug.Log("Shield is in effect");

		// Pause routine execution here for ShieldEnableTime seconds.
		yield return new WaitForSeconds(PowerUpController.ShieldEnableTime);

		// Coroutine resumed: Shield time has expired. Deactivate it.
		DeactivatePlayerShield();

		yield return null;
	}


	// This method activates player slow-mo, in which the game runs 0.4x slower.
	// This state lasts SlowMoEnableTime seconds.
	private IEnumerator ActivatePlayerSlowmo()
	{
		if (SlowmoActive) yield break;
		SlowmoActive = true;

		// Smooth transition to slow mo
		for (float factor = Time.timeScale; factor >= PowerUpController.SlowmoFactor; factor -= 0.1f)
			Time.timeScale = factor;

		// Since the for loop could never equal the var number
		Time.timeScale = PowerUpController.SlowmoFactor;
		Debug.Log("Slowmo is in effect. " + Time.timeScale);

		// Pause routine execution here for ShieldEnableTime seconds.
		yield return new WaitForSeconds(PowerUpController.SlowmoEnableTime);

		// Coroutine resumed: Slow-mo time has expired. Deactivate it.
		SlowmoActive = false;

		// Smooth transition to slow mo
		for (float factor = PowerUpController.SlowmoFactor; factor <= 1f; factor += 0.1f)
			Time.timeScale = factor;

		// Since the for loop could never equal the var number
		Time.timeScale = 1f;
		Debug.Log("Slowmo disabled. " + Time.timeScale);

		yield return null;
	}


	// Deactivates shield by stopping the coroutine and setting the shield flag as false
	// This function is used to premmaturely cancel the shield (object collision event)
	private void DeactivatePlayerShield()
	{
		// Stop Shield Routine
		StopCoroutine("ActivatePlayerShield");
		ShieldActive = false;

		Debug.Log("Shield deactivated.");
	}
}