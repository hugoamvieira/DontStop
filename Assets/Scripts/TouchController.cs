using System;
using System.Collections;
using UnityEngine;

public class TouchController : MonoBehaviour
{
	private static bool _isSwiping;
	private static Vector2 _lastPos;
	private const float TapThreshold = 8f;

	// This function detects if the user tapped the screen and returns
	// a boolean value accordingly
	public static bool TapToJump()
	{
		foreach (var touch in Input.touches)
		{
			// If the position delta for both x and y doesn't exceed a threshold
			// (in place for those weird taps), then the user wants to jump.
			return touch.phase == TouchPhase.Ended && Mathf.Abs(touch.deltaPosition.x) < TapThreshold &&
			       Mathf.Abs(touch.deltaPosition.y) < TapThreshold;
		}

		return false;
	}


	// This function detects if the user swiped down on the screen and
	// returns a boolean value accordingly
	public static bool SwipedDown()
	{
		foreach (var touch in Input.touches)
		{
			if (touch.deltaPosition.sqrMagnitude != 0f)
			{
				// Get position delta for touch
				Vector2 deltaPos = touch.deltaPosition;

				// Calculate absolute values for x and y deltas.
				// If y < x && y < 0f, means the user is swiping down, so return true.
				if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y))
				{
					return deltaPos.y < 0f;
				}
			}
		}

		return false;
	}
}