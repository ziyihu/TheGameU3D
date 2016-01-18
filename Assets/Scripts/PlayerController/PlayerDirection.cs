using UnityEngine;
using System.Collections;

public class PlayerDirection : MonoBehaviour {

	//Get the effect prefab, will use to show the effect later 
	public GameObject effect_click_prefab;
	//if the mouse is clicked
	private bool isMoving = false;
	//destination position, default value is (0,0,0)
	public Vector3 targetPosition = Vector3.zero;
	private PlayerWalk playerWalk;
	private PlayerAttack attack;

	// Use this for initialization
	void Start () {
		//at the beginning, no click action. 
		//set the target position as the current position
		targetPosition = transform.position;
		//get the PlayerWalk.cs component
		playerWalk = this.GetComponent<PlayerWalk> ();
		attack = this.GetComponent<PlayerAttack> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (attack.state == PlayerState.Death && attack.state == PlayerState.SkillAttack)	return;
		//if the click left mouse button,0 means left button
		if(Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)	{
			//detect if the place you click is on the ground
			//get the ray from the point you click
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//get the hit information
			RaycastHit hitInfo;
			//if the ray hit something isCollider is true, if the ray didn't hit anything return false
			bool isCollider = Physics.Raycast(ray, out hitInfo);
			//if the ray hit something and that thing is the ground
			if(isCollider && hitInfo.collider.tag == "Ground") {
				//the character is moving
				isMoving = true;
				//show the click effect at the hit point
				ShowClickEffect(hitInfo.point);
				//change the character face toward
				LookAtTarget(hitInfo.point);
			}
		}
		//if the mouse button is up
		if (Input.GetMouseButtonUp (0)) {
			isMoving = false;
		}
		//when click the mouse left button, isMoving is true
		//otherwise isMoving is false
		//need to detect the direction all the time
		//so if the player is not click the mouse button
		//need to detect weather the character is walking
		//need to see if the direction is right, if don't do this. will have some bugs
		if (isMoving) {
			//get the destination position
			//let the character face that position
			//detect if the place you click is on the ground
			//get the ray from the point you click
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//get the hit information
			RaycastHit hitInfo;
			//if the ray hit something isCollider is true, if the ray didn't hit anything return false
			bool isCollider = Physics.Raycast(ray, out hitInfo);
			//if the ray hit something and that thing is the ground
			if(isCollider && hitInfo.collider.tag == "Ground") {
				LookAtTarget(hitInfo.point);
			}
		} else {
			if (playerWalk.isMove){
				if(Input.GetKeyDown(KeyCode.Space)){
					Jump();
				}
				LookAtTarget(targetPosition);
			}
		}
	}

	//show the click effect, the position of the click is the variable hitPoint
	void ShowClickEffect(Vector3 hitPoint){
		//If initial the effect at the ground, the result is not good engouh
		//something has been covered by the ground, need to pull up a little bit
		hitPoint = new Vector3 (hitPoint.x,hitPoint.y+0.1f,hitPoint.z);
		//initial the effect, the first parameter is which effect need to be inital, the second parameter is the location of this effect,
		//the third parameter is whether this effect needs to be rotation or not(Quaternion.identity means do not need to)
		GameObject.Instantiate (effect_click_prefab, hitPoint, Quaternion.identity);
	}

	//let the character look face the position
	//the character just need to rotation in the y direction
	//do not need to change the x and z direction
	//targetPosition = new Vector3(targetPosition.x,targetPosition.y,targetPosition.z);
	//change the character face, should toward the target position
	void LookAtTarget(Vector3 hitPoint){
		targetPosition = hitPoint;
		targetPosition = new Vector3 (targetPosition.x, transform.position.y, targetPosition.z);
		this.transform.LookAt (targetPosition);
	}

	//if the character is walking, it can also jump
	void Jump() {
		transform.position = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
	}

}
