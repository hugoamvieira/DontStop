using UnityEngine;

public class MovingBarrelController : MonoBehaviour
{
	private Rigidbody2D _barrelRigidbody;
	[SerializeField] private float _barrelTorque;

	void Awake()
	{
		_barrelRigidbody = gameObject.GetComponent<Rigidbody2D>();
		_barrelRigidbody.AddTorque(_barrelTorque);
	}

	// Update is called once per frame
	void Update()
	{
	}
}