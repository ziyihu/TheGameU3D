using UnityEngine;
using System.Collections;

//character state
public enum ControlWalkState{
	Moving,
	Idle
}

public class PlayerWalk : MonoBehaviour {
	//walk speed 
	public float speed = 5;
	//character state
	public ControlWalkState state = ControlWalkState.Idle;
	private PlayerDirection dir;
	//will use character controller to move the character
	private CharacterController controller;
	//detect weather the character is moving
	public bool isMove= false;
	public Animation animator;

	private PlayerAttack playerAttack;

	// Use this for initialization
	void Start () {
		//get the PlayerDirection.cs in the component
		dir = this.GetComponent<PlayerDirection> ();
		//get the character controller in the component
		controller = this.GetComponent<CharacterController> ();
		playerAttack = this.GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {
				if (playerAttack.state == PlayerState.ControlWalk) {
						//the distance between the current position and the target position
						float distance = Vector3.Distance (dir.targetPosition, transform.position);
						//not arrive the target position, use character controller move the character
						if (distance > 0.3f) {
								//the character is moving now
								isMove = true;
								//move the character
								controller.SimpleMove (transform.forward * speed);
								//change the animation, change to the walk 
								state = ControlWalkState.Moving;
						} else {
								//the character is standing now
								isMove = false;
								//change the animation to the idle
								state = ControlWalkState.Idle;
						}
				}

//		float h = Input.GetAxis ("Horizontal");
//		float v = Input.GetAxis ("Vertical");
//		if (Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1) {
//			animator.CrossFade ("Sword-Run");
//			Vector3 targetDir = new Vector3 (h, 0, v);
//			transform.LookAt (targetDir + transform.position);
//			controller.SimpleMove (transform.forward * speed);
//		}
		}

	public void SimpleMove(Vector3 targetPosition){
		//look towards the target position
		transform.LookAt (targetPosition);
		controller.SimpleMove (transform.forward * speed);
	}

}
