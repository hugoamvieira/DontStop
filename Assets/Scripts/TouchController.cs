using UnityEngine;

public class TouchController : MonoBehaviour
{
	// This function detects if the user tapped the screen and returns
	// a boolean value accordingly
	public static bool TappedScreen()
	{
		foreach (var touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Ended)
			{
				Debug.Log("TouchController: User Tapped Screen");
				return true;
			}
		}
		return false;
	}


	// This function detects if the user swiped down on the screen and
	// returns a boolean value accordingly
	public static bool SwipedDown()
	{
		foreach (var touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Moved && touch.deltaPosition.sqrMagnitude != 0f)
			{
				Debug.Log("TouchController: User Swiped Down Magnitude:" + touch.deltaPosition.sqrMagnitude);
				return true;
			}
		}
		return false;
	}
}