﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))] // Requires a SpriteRenderer element
public class Tiling : MonoBehaviour
{
	private float _spriteWidth; // Element width

	// Performance Reasons
	private Camera _camera;
	private Transform _objTransform;

	public int offsetX = 2; // Offset to prevent wrong clipping

	// Used to check if it's needed to instantiate things
	public bool hasRightSprite = false;


	void Awake()
	{
		// Get reference to camera and object's transform
		_camera = Camera.main;
		_objTransform = transform;
	}

	void Start()
	{
		// Get SpriteRenderer and sprite width
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		_spriteWidth = sr.sprite.bounds.size.x;
	}


	void Update()
	{
		// Does it need new sprites? If not, no action is needed
		if (hasRightSprite == false)
		{
			// Calculate the camera's extent which is half the width of what the camera can see.
			float camHorizontalExtent = _camera.orthographicSize * Screen.width / Screen.height;

			// Calculate the x pos where the camera can see the edge of the sprite
			float edgeVisiblePosRight = (_objTransform.position.x + (_spriteWidth / 2)) - camHorizontalExtent;
			// This might not be needed for me

			// Check if camera can see the sprite edge
			if (_camera.transform.position.x >= edgeVisiblePosRight - offsetX && hasRightSprite == false)
			{
				InstantiateSprite();
				hasRightSprite = true;
			}
		}
	}

	// Creates a new sprite on the right side of an existent element.
	void InstantiateSprite()
	{
		// Calculating new sprite position
		Vector3 newSpritePos = new Vector3(_objTransform.position.x + _spriteWidth, _objTransform.position.y,
			_objTransform.position.z);

		// Instantiate sprite and save it to a variable
		Transform newSprite = (Transform) Instantiate(_objTransform, newSpritePos, _objTransform.rotation);

		// Parent the objects so it doesn't just fill the hierarchy
		newSprite.parent = _objTransform.parent;
	}
}