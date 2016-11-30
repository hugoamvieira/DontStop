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


	private IEnumerator StartEndlessMode()
	{
		yield return new WaitForSeconds(6f);
		_gameRef.LoadLevel("EndlessMode");
	}
}