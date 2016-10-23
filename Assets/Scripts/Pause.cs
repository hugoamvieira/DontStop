using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
	private bool gamePaused;


	void Start()
	{
		gamePaused = false;
	}


	void Update()
	{
		Debug.Log(Input.GetMouseButtonDown(0));
		Debug.Log(gamePaused);
		if (Input.GetMouseButtonDown(0))
		{
			gamePaused = !gamePaused;
		}

		Time.timeScale = gamePaused ? 0 : 1;
	}
}