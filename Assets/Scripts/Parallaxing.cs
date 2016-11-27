using UnityEngine;

public class Parallaxing : MonoBehaviour
{
	private float[] _parallaxScales; // Proportion of the camera's movement to move the bg.
	private Transform _camera; // Reference to the camera's transform
	private Vector3 _previousCamPos; // Stores the position of the camera in the previous frame

	public Transform[] Backgrounds; // Array of the images to be parallaxed.
	public float ParallaxSmoothing; // Parallax Ammount (How smooth the parallax is). Needs to be above 0


	void Awake()
	{
		// Set camera reference
		_camera = Camera.main.transform;
	}


	void Start()
	{
		// Store previous frame camera position
		_previousCamPos = _camera.position;

		// Create the list of scales w/ the same no. of elements as background items.
		_parallaxScales = new float[Backgrounds.Length];

		// Assign the background z position to its corresponding parallax scale
		for (int i = 0; i < Backgrounds.Length; i++)
		{
			_parallaxScales[i] = Backgrounds[i].position.z * -1;
		}
	}


	void FixedUpdate()
	{
		for (int i = 0; i < Backgrounds.Length; i++)
		{
			// Parallax = Opposite of the camera movement because the previous frame multiplied by scale
			float parallax = (_previousCamPos.x - _camera.position.x) * _parallaxScales[i];

			// Set a target x position that is the current position + parallax
			float bgTargetPosX = Backgrounds[i].position.x + parallax;

			// Create a new target position which is the bg's current position + target x pos
			Vector3 bgTargetPos = new Vector3(bgTargetPosX, Backgrounds[i].position.y, Backgrounds[i].position.z);

			// Fade between current position and the target position using a Lerp
			Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, bgTargetPos, ParallaxSmoothing * Time.deltaTime);
		}

		// Set _previousCamPos to the camera's position at the end of the frame
		_previousCamPos = _camera.position;
	}
}