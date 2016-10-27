using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;
	private PolygonCollider2D _playerCollider;
	private bool isJumping;
	private bool isCrouching;

	// Editable Vars
	public float walkSpeed;
	public float jumpForce;
	public Vector2[] standingCollider = new Vector2[4];
	public Vector2[] crouchingCollider = new Vector2[4];


	void Awake()
	{
		_anim = GetComponent<Animator>();
		_playerRigidbody = GetComponent<Rigidbody2D>();
		_playerCollider = GetComponent<PolygonCollider2D>();
	}


	void Start()
	{
	}


	void FixedUpdate()
	{
		// Events based on touch bindings. Has PC bindings due to testing on PC
		if (Input.GetKey(KeyCode.Space) || TouchController.TappedScreen())
		{
			if (!Input.GetKey(KeyCode.S) || TouchController.SwipedDown())
			{
				// Set flags
				isJumping = true;
				isCrouching = false;

				// Transform
				transform.Translate(new Vector3(walkSpeed, 0, 0));
			}

			// Check if player has previous force applied to it on the Y axis.
			if (_playerRigidbody.velocity.y > 0f || _playerRigidbody.velocity.y < 0f && _playerRigidbody)
			{
				// Jump animation
				_anim.SetInteger("AnimState", 2);
			}

			// If not, make it jump.
			else
			{
				// Jump animation
				_anim.SetInteger("AnimState", 2);

				//Player jump
				_playerRigidbody.AddForce(new Vector2(0, jumpForce));
			}
		}

		// Player swipes down, crouching begins
		else if (Input.GetKey(KeyCode.S) || TouchController.SwipedDown())
		{
			// Set flag
			isCrouching = true;
			isJumping = false;

			// Crouch animation
			_anim.SetInteger("AnimState", 3);
			transform.Translate(new Vector3(walkSpeed, 0, 0));
		}

		else
		{
			// Set flags
			isCrouching = false;
			isJumping = false;

			// Move player forward
			transform.Translate(new Vector3(walkSpeed, 0, 0));
			_anim.SetInteger("AnimState", 1);
		}

		// Handle colliders
		if (isCrouching)
		{
			_playerCollider.SetPath(0, crouchingCollider);
		}

		if (isJumping)
		{
			_playerCollider.SetPath(0, standingCollider);
		}
	}
}