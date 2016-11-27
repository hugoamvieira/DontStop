using UnityEngine;

// Might need this later for options menu (?)
public class MainMenuHandler : MonoBehaviour
{
	private GameObject _mainGUI;
	private GameObject _levelsGUI;

	private AudioSource _mainMenuAudio;

	void Awake()
	{
		_mainGUI = GameObject.Find("MainGUI");
		_mainGUI.SetActive(true);

		_mainMenuAudio = gameObject.GetComponent<AudioSource>();
		_mainMenuAudio.clip = Resources.Load("Music/MainMenu") as AudioClip;
		_mainMenuAudio.loop = true;
		_mainMenuAudio.Play();
	}
}