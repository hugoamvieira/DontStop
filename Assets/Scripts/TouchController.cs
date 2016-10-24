using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{
	public static bool TappedScreen()
	{
		foreach (var touch in Input.touches)
		{
			if (Input.touchCount == 1 && touch.phase == TouchPhase.Ended)
			{
				Debug.Log("TouchController: User Tapped Screen");
				return true;
			}
		}
		return false;
	}


	public static bool SwipedDown()
	{
		foreach (var touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Moved && touch.position.y < 0f)
			{
				Debug.Log("TouchController: User Swiped Down");
				return true;
			}
		}
		return false;
	}
}