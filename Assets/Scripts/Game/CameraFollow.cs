using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	//Reference to what I want the camera to follow (Player)
	public Transform PlayerTransform;
	private Vector3 cameraOffset;

	//The higher the value of the smooth factor the faster the camera will lock onto the player
	//The smaller it is, the smoother the camera will be
	[Range(0.01f, 1.0f)]
	public float smoothFactor = 0.5f;


	// Use this for initialization
	void Start () {
		cameraOffset = transform.position - PlayerTransform.position;
	}

	//FixedUpdate updates the position in the fixed update physics loop
	void FixedUpdate () {
		Vector3 newPos = PlayerTransform.position + cameraOffset;

		transform.position = Vector3.Lerp (transform.position, newPos, smoothFactor);
	}
}
