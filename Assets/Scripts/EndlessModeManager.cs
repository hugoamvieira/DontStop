using UnityEngine;

public class EndlessModeManager : MonoBehaviour
{
	private AudioSource _endlessModeAudio;
	private bool _gameOverPlaying;

	void Awake()
	{
		_gameOverPlaying = false;

		// Endless mode audio
		_endlessModeAudio = GetComponent<AudioSource>();
		_endlessModeAudio.clip = Resources.Load("Music/EndlessMode") as AudioClip;
		_endlessModeAudio.loop = true;
		_endlessModeAudio.pitch = 1.1f;
		_endlessModeAudio.Play();
	}


	void Update()
	{
		if (PlayerController.GameOver && !_gameOverPlaying)
		{
			// Play game over music
			_endlessModeAudio.clip = Resources.Load("Music/GameOver") as AudioClip;
			_endlessModeAudio.loop = true;
			_endlessModeAudio.pitch = 1.0f;
			_endlessModeAudio.Play();
			_gameOverPlaying = true;
		}
	}
}