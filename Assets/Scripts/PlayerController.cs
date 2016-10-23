using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// Components
	private Animator _anim;
	private Rigidbody2D _playerRigidbody;
	private SpriteRenderer _playerSr;

	// Editable Vars
	public float walkSpeed;
	public float jumpForce;


	void Start()
	{
	}


	void OnEnable()
	{
		_anim = GetComponent<Animator>();
		_playerRigidbody = GetComponent<Rigidbody2D>();
		_playerSr = GetComponent<SpriteRenderer>();
	}


	void Update()
	{
		// PC Bindings for now.
		// TODO: Change to touch bindings
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			// Play jump animation
			_anim.SetInteger("AnimState", 2);
			transform.Translate(new Vector3(walkSpeed, 0, 0));

			// Check if player has previous force applied to it on the Y axis. If not, make it jump
			if (_playerRigidbody.velocity.y > 0f || _playerRigidbody.velocity.y < 0f && _playerRigidbody)
			{
			}
			else
			{
				//Player jump
				_playerRigidbody.AddForce(new Vector2(0, jumpForce));
			}
		}

		else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
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