using UnityEngine;

public class MenuController : MonoBehaviour
{
	private GameObject _overlayPanel;
	public static bool paused { get; private set; }


	void Awake()
	{
		_overlayPanel = GameObject.Find("PauseMenu");
		_overlayPanel.SetActive(false);
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


	public void ExitToMenu()
	{
	}
}