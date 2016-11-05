﻿using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
	private int _frameCounter;
	private float _timeCounter;
	private float _lastFramerate;
	private const float RefreshTime = 0.5f;


	void Awake()
	{
		// Set game FPS
		Application.targetFrameRate = 60;
	}


	void Start()
	{
	}


	void Update()
	{
		// UpdateFPS();
	}


	private void UpdateFPS()
	{
		if (_timeCounter < RefreshTime)
		{
			_timeCounter += Time.deltaTime;
			++_frameCounter;
		}

		else
		{
			_lastFramerate = _frameCounter / _timeCounter;
			_frameCounter = 0;
			_timeCounter = 0.0f;
		}

		Debug.Log("GameBehaviour: FPS = " + _lastFramerate);
	}
}