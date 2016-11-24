using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
	private int _frameCounter;
	private float _timeCounter;
	private float _lastFramerate;
	private PlayerController _playerRef;
	private LevelModeManager _levelRef;
	private GUIController _guiRef;
	private const float RefreshTime = 0.5f;

	// Number of levels
	public static int NumberOfLevels { get; private set; }

	void Awake()
	{
		// Set game FPS
		Application.targetFrameRate = 60;

		// Get player reference
		if (GameObject.Find("Player") != null)
			_playerRef = GameObject.Find("Player").GetComponent<PlayerController>();

		if (GameObject.Find("Level Manager").GetComponent<LevelModeManager>() != null)
			_levelRef = GameObject.Find("Level Manager").GetComponent<LevelModeManager>();

		if (GameObject.Find("Level Manager").GetComponent<GUIController>() != null)
			_guiRef = GameObject.Find("Level Manager").GetComponent<GUIController>();

		// Set timeScale as 1 (For restarting purposes)
		Time.timeScale = 1f;

		// Set number of existing levels
		NumberOfLevels = 5;
	}


	void Update()
	{
		// UpdateFPS();
		if (_playerRef != null && _playerRef.GameOver)
			EndGame();

		if (_playerRef != null && _playerRef.LevelCompleted && _levelRef != null)
			LevelCompleted();
	}


	private void LevelCompleted()
	{
		var playTime = _playerRef.LevelCompleteTime;
		Time.timeScale = 0f;

		var noOfStars = _levelRef.LevelCompleted(playTime);
		_guiRef.ToggleLevelCompleteMenu(noOfStars);
	}


	private void EndGame()
	{
		Time.timeScale = 0f;
		_guiRef.ToggleGameOverMenu();
	}


	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene(levelName);
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