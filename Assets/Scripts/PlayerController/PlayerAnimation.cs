using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	private PlayerWalk walk;
	private PlayerAttack attack;

	// Use this for initialization
	void Start () {
		//get the component PlayerWalk.cs
		walk = this.GetComponent<PlayerWalk>();
		attack = this.GetComponent<PlayerAttack> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(attack.state == PlayerState.ControlWalk){
			//if the character is moving, play the walk animation
			if (walk.state == ControlWalkState.Moving) {
				PlayAnimation("Sword-Run");
			} 
			//if the character is standing, play the idle animation
			else if (walk.state == ControlWalkState.Idle){
				PlayAnimation("Sword-Idle");
			}
		} else if(attack.state == PlayerState.NormalAttack) {
			if(attack.attackState == AttackState.Moving){
				PlayAnimation("Sword-Run");
			} 
		}
	}

	//play Animation
	void PlayAnimation(string animationName){
		//play the animation
		animation.CrossFade (animationName);
	}
}
