using UnityEngine;

public class EndlessModeManager : MonoBehaviour
{
	private AudioSource _endlessModeAudio;
	private PlayerController _playerRef;
	private bool _gameOverPlaying;

	[System.Serializable]
	public struct DifficultyLevel
	{
		public float PlayerDistanceTrigger;
		public float NewMaxVelocityX;
		public bool Active;
	}

	public DifficultyLevel[] DifficultyLevels;


	void Awake()
	{
		_playerRef = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();
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
		if (_playerRef.GameOver && !_gameOverPlaying)
		{
			// Play game over music
			_endlessModeAudio.clip = Resources.Load("Music/GameOver") as AudioClip;
			_endlessModeAudio.loop = true;
			_endlessModeAudio.pitch = 1.0f;
			_endlessModeAudio.Play();
			_gameOverPlaying = true;
		}

		if (!_playerRef.GameOver)
		{
			for (var i = 0; i < DifficultyLevels.Length; i++)
			{
				if (_playerRef.DistanceElapsed > DifficultyLevels[i].PlayerDistanceTrigger && !DifficultyLevels[i].Active)
				{
					DifficultyLevels[i].Active = true;
				}

				if (DifficultyLevels[i].Active)
					_playerRef.MaxSpeedX = DifficultyLevels[i].NewMaxVelocityX;
			}
		}
	}
}