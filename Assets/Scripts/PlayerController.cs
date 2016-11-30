using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;
	private SpriteRenderer _playerSr;
	private AudioSource _playerAudio;

	// Anim states
	private const string AnimParamName = "AnimState";
	private const int RunAnimState = 1;
	private const int JumpAnimState = 2;
	private const int CrouchAnimState = 3;

	// Forces
	public float WalkForce;
	public float JumpForce;

	// Max speeds
	public float MaxSpeedX;
	public float MaxSpeedY;

	// Flags
	public bool IsJumping;
	public bool IsCrouching;
	public int RespawnTimer;
	public float RespawnTransparency;

	// Player Environment
	private float _posX;

	public float PosX
	{
		get { return _posX; }
		set { _posX = value; }
	}

	private float _posY;

	public float PosY
	{
		get { return _posY; }
		set { _posY = value; }
	}

	private bool _collidedShieldPowerup;

	public bool CollidedShieldPowerup
	{
		get { return _collidedShieldPowerup; }
		set { _collidedShieldPowerup = value; }
	}

	private bool _collidedSlowmoPowerup;

	public bool CollidedSlowmoPowerup
	{
		get { return _collidedSlowmoPowerup; }
		set { _collidedSlowmoPowerup = value; }
	}

	private bool _slowmoActive;

	public bool SlowmoActive
	{
		get { return _slowmoActive; }
		set { _slowmoActive = value; }
	}

	private bool _shieldActive; // Getter for ShieldEnableTime active player property

	public bool ShieldActive
	{
		get { return _shieldActive; }
		set { _shieldActive = value; }
	}

	private bool _gameOver; // Determines whether game is over or not

	public bool GameOver
	{
		get { return _gameOver; }
		set { _gameOver = value; }
	}

	private float _distanceElapsed;

	public float DistanceElapsed
	{
		get { return _distanceElapsed; }
		set { _distanceElapsed = value; }
	}

	private int _playerScore;

	public int PlayerScore
	{
		get { return _playerScore; }
		private set { _playerScore = value; }
	}

	private int _obstaclesJumped;

	public int ObstaclesJumped
	{
		get { return _obstaclesJumped; }
		set { _obstaclesJumped = value; }
	}

	private int _objectsCollected;

	public int ObjectsCollected
	{
		get { return _objectsCollected; }
		set { _objectsCollected = value; }
	}

	// Power-ups' / Score variables (Because otherwise you would only be able to set these on the powerups which are generated
	// at runtime)
	private int _score;
	private float _distanceMultiplier;
	private float _collisionDecel;
	public float ShieldEnableTime;
	public float SlowmoEnableTime;
	public float SlowmoFactor;

	// Counters for jump and crouch soundfx
	private bool _jumpFXPlayed;
	private bool _crouchFXPlayed;

	// Player Colliders
	[SerializeField] private PolygonCollider2D[] _playerColliders; // Assigned in the editor
	[SerializeField] private int _currentColliderIndex;


	void Awake()
	{
		// Get Components
		_anim = gameObject.GetComponent<Animator>();
		_playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
		_playerSr = gameObject.GetComponent<SpriteRenderer>();
		_playerAudio = gameObject.GetComponent<AudioSource>();

		// Set player Y Position. It's here because I only need this property for the power ups
		// If it was on the Update method, power ups would spawn in the air whenever the player jumped.
		PosY = transform.position.y;

		// Set score and death count
		PlayerScore = 0;
		ObjectsCollected = 1;

		// Reset FX played counters
		_jumpFXPlayed = false;
		_crouchFXPlayed = false;

		// Set the power-ups properties (didn't want to make classes for each ¯\_(ツ)_/¯)
		PowerUpController.ShieldEnableTime = ShieldEnableTime;
		PowerUpController.SlowmoEnableTime = SlowmoEnableTime;
		PowerUpController.SlowmoFactor = SlowmoFactor;

		// Playable state
		GameOver = false;

		// Set score for each successful jump / crouch
		_score = 100;
		_distanceMultiplier = 0.5f;

		// Collision deceleration
		_collisionDecel = -2f;
	}


	void FixedUpdate()
	{
		// Update Position
		if (gameObject.transform.position.x > 0)
			DistanceElapsed = Mathf.Abs(gameObject.transform.position.x) * _distanceMultiplier;

		// Set/Update player X position
		PosX = transform.position.x;

		// Engage endless running mode
		if (_playerRigidbody.velocity.x <= MaxSpeedX)
		{
			_playerRigidbody.AddForce(new Vector2(WalkForce, 0));
		}


		// Handle user input
		if (Input.GetKey(KeyCode.Space) || TouchController.SwipedUp())
		{
			if (IsCrouching) return;

			// If the player's Y-axis speed doesn't exceed the set max Y speed
			// and if the player is on the ground (ie. Y-axis velocity == 0):
			if (_playerRigidbody.velocity.y <= MaxSpeedY && _playerRigidbody.velocity.y == 0f)
			{
				// Change to jump animation and add jumping force
				_anim.SetInteger(AnimParamName, JumpAnimState);
				_playerRigidbody.AddForce(new Vector2(0, JumpForce));

				if (_jumpFXPlayed) return;
				_playerAudio.clip = Resources.Load("SoundFX/Jump") as AudioClip;
				_playerAudio.volume = 0.2f;
				_playerAudio.Play();
				_jumpFXPlayed = true;
			}
		}

		else if (Input.GetKey(KeyCode.S) || TouchController.SwipedDown())
		{
			if (IsJumping) return;

			// Change to crouch animation
			_anim.SetInteger(AnimParamName, CrouchAnimState);

			if (_crouchFXPlayed) return;
			_playerAudio.clip = Resources.Load("SoundFX/Crouch") as AudioClip;
			_playerAudio.volume = 0.2f;
			_playerAudio.Play();
			_crouchFXPlayed = true;
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

		// Reset the FX variables for the next frame
		_jumpFXPlayed = false;
		_crouchFXPlayed = false;
	}


	void OnCollisionEnter2D(Collision2D collisionObject)
	{
		// Ignore collision if the player didn't collide with an obstacle
		if (!collisionObject.gameObject.name.Contains("Obstacle")) return;

		if (!ShieldActive && ObjectsCollected == 0)
			GameOver = true;

		else if (ShieldActive)
		{
			DeactivatePlayerShield();
			SpawnPlayerAfterObject(collisionObject);
			StartCoroutine("RespawnPlayer");
		}

		else if (!ShieldActive && ObjectsCollected > 0)
		{
			ObjectsCollected = 0;
			SpawnPlayerAfterObject(collisionObject);
			StartCoroutine("RespawnPlayer");
		}
	}


	void OnTriggerEnter2D(Collider2D triggerObject)
	{
		if (triggerObject.gameObject.name.Contains("Obstacle"))
		{
			PlayerScore += _score;
			ObstaclesJumped++;
		}
	}


	// Changes colliders based on animation
	public void SetSpriteCollider(int spriteFrame)
	{
		_playerColliders[_currentColliderIndex].enabled = false;
		_currentColliderIndex = spriteFrame;
		_playerColliders[_currentColliderIndex].enabled = true;
	}


	// Spawns player after the object it crashed into
	void SpawnPlayerAfterObject(Collision2D collisionObject)
	{
		var newPlayerPosX = collisionObject.transform.position.x +
		                    collisionObject.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x + 0.2f;

		_playerRigidbody.AddForce(new Vector2(_collisionDecel, 0));
		gameObject.transform.position = new Vector3(newPlayerPosX, PosY, 0);
	}


	// Gives that cool respawn effect by switching the transparency on the player
	private IEnumerator RespawnPlayer()
	{
		_playerSr.color = new Color(_playerSr.color.r, _playerSr.color.g, _playerSr.color.b, RespawnTransparency);
		yield return new WaitForSeconds(RespawnTimer);

		_playerSr.color = new Color(_playerSr.color.r, _playerSr.color.g, _playerSr.color.b, 1);
	}


	// This method activates the player ShieldEnableTime, in which the player becomes invincible
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