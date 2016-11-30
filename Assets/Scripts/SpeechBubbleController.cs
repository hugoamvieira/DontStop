using System.Collections;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
	private GameObject _speechBubble;

	void Awake()
	{
		_speechBubble = GameObject.Find("SpeechBubble");
		_speechBubble.SetActive(false);
		StartCoroutine("SpeechBubblePopup");
	}


	private IEnumerator SpeechBubblePopup()
	{
		_speechBubble.SetActive(false);

		yield return new WaitForSeconds(1f);
		_speechBubble.SetActive(true);

		yield return new WaitForSeconds(3f);
		_speechBubble.SetActive(false);
	}
}