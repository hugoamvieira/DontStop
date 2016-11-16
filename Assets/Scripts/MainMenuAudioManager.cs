using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
	private AudioSource _mainMenuAudio;

	// Use this for initialization
	void Awake()
	{
		_mainMenuAudio = GetComponent<AudioSource>();
		_mainMenuAudio.clip = Resources.Load("Music/MainMenu") as AudioClip;
		_mainMenuAudio.loop = true;
		_mainMenuAudio.Play();
	}
}