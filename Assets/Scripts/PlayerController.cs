﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;

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

	// Flags (These are set through the animations)
	public bool isJumping;
	public bool isCrouching;

	// Player Environment
	public static float PosX { get; private set; }
	public static float PosY { get; private set; }
	public static bool CollidedShieldPowerup { get; set; }
	public static bool CollidedSlowmoPowerup { get; set; }
	private float _distanceElapsed;
	public static bool SlowmoActive { get; private set; }
	public static bool ShieldActive { get; private set; } // Getter for shieldEnableTime active player property

	// Power-ups' variables (Because otherwise you would only be able to set these on the powerups which are generated
	// at runtime)
	public float shieldEnableTime;
	public float slowmoEnableTime;
	public float slowmoFactor;


	void Awake()
	{
		// Get Components
		_anim = gameObject.GetComponent<Animator>();
		_playerRigidbody = gameObject.GetComponent<Rigidbody2D>();

		// Set player Y Position. It's here because I only need this property for the power ups
		// If it was on the Update method, power ups would spawn in the air whenever the player jumped.
		PosY = transform.position.y;

		// Set the power-ups properties (didn't want to make classes for each ¯\_(ツ)_/¯)
		PowerUpController.ShieldEnableTime = shieldEnableTime;
		PowerUpController.SlowmoEnableTime = slowmoEnableTime;
		PowerUpController.SlowmoFactor = slowmoFactor;
	}


	void FixedUpdate()
	{
		// Set/Update player X position
		PosX = transform.position.x;

		// Engage endless running mode
		if (_playerRigidbody.velocity.x <= maxSpeedX)
		{
			_playerRigidbody.AddForce(new Vector2(walkForce, 0));
		}


		// Handle user input
		if (Input.GetKey(KeyCode.Space) || TouchController.TapToJump())
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
			StartCoroutine(ActivatePlayerShield());
		}
		else if (CollidedSlowmoPowerup)
		{
			StartCoroutine(ActivatePlayerSlowmo());
		}
	}


	// This method activates the player shieldEnableTime, in which the player becomes invincible
	// for ShieldEnableTime seconds or until it collides with an object, whichever comes first.
	private IEnumerator ActivatePlayerShield()
	{
		if (ShieldActive) yield break;

		// TODO: Check if object collision here and destroy shieldEnableTime upon that.
		ShieldActive = true;

		Debug.Log("Shield is in effect");

		// Pause routine execution here for ShieldEnableTime seconds.
		yield return new WaitForSeconds(PowerUpController.ShieldEnableTime);

		// Coroutine resumed: Shield time has expired. Deactivate it.
		ShieldActive = false;
		Debug.Log("Shield deactivated.");

		yield return null;
	}


	// This method activates player slow-mo, in which the game runs 0.4x slower.
	// This state lasts SlowMoEnableTime seconds.
	private IEnumerator ActivatePlayerSlowmo()
	{
		if (SlowmoActive) yield break;

		// Set slow mo as true and factor in the SlowMoFactoring to the game timeScale
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
}