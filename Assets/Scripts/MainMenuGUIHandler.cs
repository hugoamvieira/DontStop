using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGUIHandler : MonoBehaviour
{
	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}


	public void LoadMainMenuLevelGUI()
	{
	}
}