using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;

	// Editable Vars
	public float walkSpeed;
	public float jumpForce;


	void Awake()
	{
		_anim = GetComponent<Animator>();
		_playerRigidbody = GetComponent<Rigidbody2D>();
	}


	void Start()
	{
	}


	void FixedUpdate()
	{
		// PC Bindings for now.
		// TODO: Change to touch bindings
		if (Input.GetKey(KeyCode.Space))
		{
			transform.Translate(new Vector3(walkSpeed, 0, 0));

			// Check if player has previous force applied to it on the Y axis. If not, make it jump
			if (_playerRigidbody.velocity.y > 0f || _playerRigidbody.velocity.y < 0f && _playerRigidbody)
			{
				// Play jump animation
				_anim.SetInteger("AnimState", 2);
			}

			else
			{
				// Play jump animation
				_anim.SetInteger("AnimState", 2);

				//Player jump
				_playerRigidbody.AddForce(new Vector2(0, jumpForce));
			}
		}

		else if (Input.GetKey(KeyCode.S))
		{
			_anim.SetInteger("AnimState", 3);
			transform.Translate(new Vector3(walkSpeed, 0, 0));
		}

		else
		{
			// Move player forward
			transform.Translate(new Vector3(walkSpeed, 0, 0));
			_anim.SetInteger("AnimState", 1);
		}
	}
}