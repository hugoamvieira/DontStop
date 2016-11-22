using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
	private GameObject _overlayPanel;
	private GameObject _gameOverPanel;
	private GameObject _shieldPowerupGUI;
	private GameObject _slowmoPowerupGUI;
	private Text _guiDistanceText;
	private Text _scoreText;

	private PlayerController _playerRef;

	public const string MainMenuSceneName = "MainMenu";
	public static bool Paused { get; private set; }


	void Awake()
	{
		_overlayPanel = GameObject.Find("PauseMenu");
		_gameOverPanel = GameObject.Find("GameOverMenu");
		_guiDistanceText = GameObject.Find("DistanceValue").GetComponent<Text>();
		_scoreText = GameObject.Find("ScoreValue").GetComponent<Text>();
		_shieldPowerupGUI = GameObject.Find("ShieldPowerupGUI");
		_slowmoPowerupGUI = GameObject.Find("SlowmoPowerupGUI");

		_playerRef = GameObject.Find("Player").gameObject.GetComponent<PlayerController>();

		_overlayPanel.SetActive(false);
		_gameOverPanel.SetActive(false);
		_shieldPowerupGUI.SetActive(false);
	}


	void FixedUpdate()
	{
		// Set the distance with a distance multiplier so it isn't so high
		_guiDistanceText.text = _playerRef.DistanceElapsed.ToString("0.00m");

		// Set the score
		_scoreText.text = _playerRef.PlayerScore.ToString();

		// Check if shield / slowmo is active and enable shield in GUI acordingly
		_shieldPowerupGUI.SetActive(_playerRef.ShieldActive);
		_slowmoPowerupGUI.SetActive(_playerRef.SlowmoActive);
	}


	public void ToggleGameOverMenu()
	{
		if (_gameOverPanel == null) return;
		_gameOverPanel.SetActive(true);
	}


	public void TogglePauseMenu()
	{
		Paused = !Paused;
		if (Paused)
		{
			Time.timeScale = 0f;
			_overlayPanel.SetActive(true);
		}

		else
		{
			Time.timeScale = _playerRef.SlowmoActive ? PowerUpController.SlowmoFactor : 1f;
			_overlayPanel.SetActive(false);
		}
	}


	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void ExitToMenu()
	{
		SceneManager.LoadScene(MainMenuSceneName);
	}
}