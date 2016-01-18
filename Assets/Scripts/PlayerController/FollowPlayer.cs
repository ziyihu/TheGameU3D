using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	//get the player(Swordman)
	private Transform player;
	//the position between the player and the camera
	private Vector3 offsetPosition;
	//the distance between the player and the camera
	public float distance = 0;
	//scroll speed
	public float scrollSpeed = 10;
	//whether the camera is rotate or not
	private bool isRotating = false;
	//rotate speed
	public float rotateSpeed = 1;

	// Use this for initialization
	void Start () {
		//using Tags to get the player
		player = GameObject.FindGameObjectWithTag("Player").transform;
		//let the camera look at the player 
		transform.LookAt (player.position);
		//the position between the player and the camera
		offsetPosition = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
		//follow the player
		transform.position = player.position + offsetPosition;
		//let the camera get closer to the player or get fatter to the player
		ScrollView ();
		//rotate the view
		RotateView ();
	}

	//using the mouse scroll(middle button) to control the location of the camera
	void ScrollView() {
		//Input.GetAxis("Mouse ScrollWheel")
		//the camera go back, return negative number
		//the camera go forward, retrun positive number
		//get the distance
		distance = offsetPosition.magnitude;
		distance += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		//can't get too close or too far
		//closet distance is 4, farthest distance is 18
		distance = Mathf.Clamp (distance, 4, 18);
		//offsetPosition.normalized return the direction of the offsetPosition
		//times distance let the direction remain the same, larger the distance between the character and the camera
		offsetPosition = offsetPosition.normalized * distance;
	}

	//using the right mouse button click to control the rotate of the camera
	void RotateView() {
		//get the mouse movement in the X direction
		//Input.GetAxis ("Mouse X");
		//get the mouse movement in the Y direction
		//Input.GetAxis ("Mouse Y");
		//mouse right button is clicked
		if (Input.GetMouseButtonDown (1)) {
			//if click the right button, the camera will rotate
			isRotating = true;
		} else if (Input.GetMouseButtonUp (1)) {
			//if the right button is relesed, the camera will not rotate
			isRotating = false;
		}
		if (isRotating) {
			//rotate around a point,a axis with a angle
			//the first parameter is the point
			//the second parameter is the axis(Verctor3.up is y-axis, so the player will rotate by y-axis)
			//the third parameter is the rotate angle
			//rotate in the left-right direction
			transform.RotateAround(player.position, player.up, rotateSpeed * Input.GetAxis("Mouse X"));

			Vector3 originalPosition = transform.position;
			Quaternion originalRotation = transform.rotation;

			//rotate in the top-buttom direction
			transform.RotateAround(player.position, player.right, rotateSpeed * Input.GetAxis("Mouse Y"));
			//the top-buttom rotate should be restricted in a range
			float x = transform.eulerAngles.x;
			//the angle should between 10 degree to 80 degree
			if(x < 10 || x > 80) {
				//if the angle is bigger than 80 or smaller than 10
				//let the rotation useless
				transform.position = originalPosition;
				transform.rotation = originalRotation;
			}

		}
		//update the offsetPoisition
		offsetPosition = transform.position - player.position;
	}
}
