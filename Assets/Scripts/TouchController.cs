using UnityEngine;

public class TouchController : MonoBehaviour
{
	private static bool _isSwiping;
	private static Vector2 _lastPos;


	// This function detects if the user swiped up on the screen and
	// returns a boolean value accordingly
	public static bool SwipedUp()
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
					return deltaPos.y > 0f;
				}
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