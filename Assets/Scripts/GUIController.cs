using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
	private static GameObject _overlayPanel;
	private static GameObject _gameOverPanel;
	private static GameObject _shieldPowerupGUI;
	private static GameObject _slowmoPowerupGUI;
	private static Text _guiDistanceText;
	private static Text _scoreText;

	public const string mainMenuSceneName = "MainMenu";
	public static bool paused { get; private set; }
	public float distanceMultiplier;


	void Awake()
	{
		_overlayPanel = GameObject.Find("PauseMenu");
		_gameOverPanel = GameObject.Find("GameOverMenu");
		_guiDistanceText = GameObject.Find("DistanceValue").GetComponent<Text>();
		_scoreText = GameObject.Find("ScoreValue").GetComponent<Text>();
		_shieldPowerupGUI = GameObject.Find("ShieldPowerupGUI");
		_slowmoPowerupGUI = GameObject.Find("SlowmoPowerupGUI");

		_overlayPanel.SetActive(false);
		_gameOverPanel.SetActive(false);
		_shieldPowerupGUI.SetActive(false);
	}


	void FixedUpdate()
	{
		// Set the distance with a distance multiplier so it isn't so high
		var distance = PlayerController.DistanceElapsed * distanceMultiplier;
		_guiDistanceText.text = distance.ToString("0.00m");

		// Set the score
		_scoreText.text = PlayerController.PlayerScore.ToString();

		// Check if shield / slowmo is active and enable shield in GUI acordingly
		_shieldPowerupGUI.SetActive(PlayerController.ShieldActive);
		_slowmoPowerupGUI.SetActive(PlayerController.SlowmoActive);
	}


	public static void ToggleGameOverMenu()
	{
		if (_gameOverPanel == null) return;
		_gameOverPanel.SetActive(true);
	}


	public void TogglePauseMenu()
	{
		paused = !paused;
		if (paused)
		{
			Time.timeScale = 0f;
			_overlayPanel.SetActive(true);
		}

		else
		{
			Time.timeScale = PlayerController.SlowmoActive ? PowerUpController.SlowmoFactor : 1f;
			_overlayPanel.SetActive(false);
		}
	}


	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void ExitToMenu()
	{
		SceneManager.LoadScene(mainMenuSceneName);
	}
}