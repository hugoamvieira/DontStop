using System.Collections;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
	private GameBehaviour _gameRef;

	void Awake()
	{
		_gameRef = gameObject.GetComponent<GameBehaviour>();
		StartCoroutine("StartEndlessMode");
	}


	void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
			_gameRef.LoadLevel("EndlessMode");
	}


	private IEnumerator StartEndlessMode()
	{
		yield return new WaitForSeconds(5f);
		_gameRef.LoadLevel("EndlessMode");
	}
}