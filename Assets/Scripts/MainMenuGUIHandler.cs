using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGUIHandler : MonoBehaviour
{
	private GameObject _mainGUI;
	private GameObject _levelsGUI;

	void Awake()
	{
		_mainGUI = GameObject.Find("MainGUI");
		_levelsGUI = GameObject.Find("LevelModeGUI");

		_mainGUI.SetActive(true);
		_levelsGUI.SetActive(false);
	}


	public void LoadMainMenuLevelGUI()
	{
		_levelsGUI.SetActive(true);
		_mainGUI.SetActive(false);

		// If the buttons were created already, then don't do anything
		if (_levelsGUI.transform.FindChild("LevelsPanel").childCount != 0) return;

		// Case it's the first time running, then render the levels on screen
		var btnPrefab = Resources.Load("LevelButton");
		for (var i = 1; i <= GameBehaviour.NumberOfLevels; i++)
		{
			GameObject b = (GameObject) Instantiate(btnPrefab);

			// Set it as a child of LevelsPanel (where the GridLayout is)
			// Set position and text accordingly
			b.transform.SetParent(_levelsGUI.transform.FindChild("LevelsPanel"));
			b.transform.position = new Vector3(_levelsGUI.transform.position.x, _levelsGUI.transform.position.y);
			b.GetComponentInChildren<Text>().text += i;

			var i2 = i; // No access to i directly due to lambda expression
			GameBehaviour gb = new GameBehaviour();
			b.GetComponent<Button>().onClick.AddListener(
				() => gb.LoadLevel("Level" + i2)
			);
		}
	}


	public void LoadMainMenuGUI()
	{
		// Set main GUI as active
		_levelsGUI.SetActive(false);
		_mainGUI.SetActive(true);
	}
}