using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
	private static GameObject _overlayPanel;
	private static GameObject _gameOverPanel;
	private static Text _guiDistanceText;

	public const string mainMenuSceneName = "MainMenu";
	public static bool paused { get; private set; }
	public float distanceMultiplier;


	void Awake()
	{
		_overlayPanel = GameObject.Find("PauseMenu");
		_gameOverPanel = GameObject.Find("GameOverMenu");
		_guiDistanceText = GameObject.Find("DistanceValue").GetComponent<Text>();

		_overlayPanel.SetActive(false);
		_gameOverPanel.SetActive(false);
	}


	void FixedUpdate()
	{
		// Set the distance with a distance multiplier so it isn't so high
		var distance = PlayerController.DistanceElapsed * distanceMultiplier;
		_guiDistanceText.text = distance.ToString("0.00m");
	}


	public static void ToggleGameOverMenu()
	{
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