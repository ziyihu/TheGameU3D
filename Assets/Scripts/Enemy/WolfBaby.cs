using UnityEngine;
using System.Collections;

public enum WolfState{
	Idle,
	Walk,
	Attack,
	Death
}

public class WolfBaby : MonoBehaviour {

	public WolfState state = WolfState.Idle;
	public int attack = 30;
	//some properties of the wolf baby
	public int hp = 100;
	public float missRate = 0.2f;
	//control the walking and idling time
	private float time = 1;
	private float timer = 0;

	public string animationIdle;
	public string animationWalk;
	public string animationDead;

	public float moveSpeed = 1;
	//control the animtion between the walking and idling
	public string animationMoveCurrent;
	//control the wolf baby move
	private CharacterController babyController;
	//normal color, under attack the color will change to red
	private Color normal;
	//whether the wolf baby attacking
	private bool isAttack = false;

	//attack rate per second
	public int attackRate = 1;
	//attack timer
	private float attackTimer = 0;
	//two types of attacks
	public string animationAttackCurrent = "WolfBaby-Attack1";
	//normal attack
	public string animationNormalAttack = "WolfBaby-Attack1";
	public float timeNormalAttack = 0.633f;
	//crazu attacl
	public string animationCrazyAttack = "WolfBaby-Attack2";
	public float timeCrazyAttack = 0.733f;
	//in 10 attacks, only 2 crazy attacks, 8 normal attacks
	public float crazyAttackRate = 0.2f;
	//attack target
	public Transform target;
	//attack distance
	public float minDistance = 2;
	public float maxDistance = 5;

	private GameObject hudtextFollow;
	private GameObject hudtextGo;
	public GameObject hudtextPrefab;

	public HUDText hudtext;
	private UIFollowTarget followTarget;

	public GameObject body;

	public BabyWolfBorn born;

	void Awake(){
		babyController = this.GetComponent<CharacterController> ();
//		body = transform.Find ("Wolf_Baby").gameObject;
//		normal = body.renderer.material.color;
		hudtextFollow = transform.Find ("HUDText").gameObject;
	}

	// Use this for initialization
	void Start () {
		animationMoveCurrent = animationIdle;
		//hudtextGo = GameObject.Instantiate (hudtextPrefab,Vector3.zero,Quaternion.identity) as GameObject;
		//hudtextGo.transform.parent = HUDTextRoot._instance.gameObject.transform;

		hudtextGo = NGUITools.AddChild (HUDTextRoot._instance.gameObject, hudtextPrefab);

		hudtext = hudtextGo.GetComponent<HUDText> ();
		followTarget = hudtextGo.GetComponent<UIFollowTarget> ();
		followTarget.target = hudtextFollow.transform;
		followTarget.gameCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		//wolf baby is dead
		if (state == WolfState.Death) {
			animation.CrossFade(animationDead);
		} 
		//wolf baby is attacking
		else if(state == WolfState.Attack) {
			AutoAttack();
		} 
		//wolf baby is walking or idling
		else {
			//play the current animation
			animation.CrossFade(animationMoveCurrent);
			//if the animation is waling, move the character forward
			if(animationMoveCurrent == animationWalk){
				babyController.SimpleMove(transform.forward*moveSpeed);
			}
			timer += Time.deltaTime;
			if( timer>=time ){
				timer = 0;
				RandomState();
			}
		}

		//for test
//		if (Input.GetMouseButtonDown (1)) {
//			TakeDamge(1);
//		}
	}

	void RandomState(){
		int value = Random.Range (0, 2);
		if (value == 0) {
			animationMoveCurrent = animationIdle;
		} else {
			if(animationMoveCurrent != animationWalk){
				//when from the idle to the walk, give it a random direction
				transform.Rotate(transform.up * Random.Range(0,360));
			}
			animationMoveCurrent = animationWalk;
		}
	}

	public void TakeDamge(int attack){
		if(state == WolfState.Death) return;
		state = WolfState.Attack;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		float value = Random.Range (0f, 1f);
		//miss, don't hit the wolf
		if (value < missRate) {
			hudtext.Add("Miss",Color.gray,1);
		}
		//hit the wolf
		else {
			//show the missing hp
			hudtext.Add("-"+attack,Color.red,1);
			this.hp -= attack;
			StartCoroutine(ShowBodyRed());
			isAttack = true;
			attackTimer = 0;
			//if the hp is less than 0,the wolf must be dead
			if(hp<=0){
				state = WolfState.Death;
				Destroy(this.gameObject, 2);
			}
		}
	}

	IEnumerator ShowBodyRed(){
		body.renderer.material.color = Color.red;
		yield return new WaitForSeconds (1f);
		body.renderer.material.color = Color.grey;
	}

	void AutoAttack(){
		//no target to attack
		if (target != null) {
			PlayerState playerState = target.GetComponent<PlayerAttack>().state;
			if(playerState == PlayerState.Death){
				//stop auto attack
				target = null;
				state = WolfState.Idle;
				return;
			}
			float distance = Vector3.Distance(target.position, transform.position);
			if(distance > maxDistance){
				//stop auto attack
				target = null;
				state = WolfState.Idle;
			} else if(distance < minDistance){
				transform.LookAt(target);
				//do the auto attack
				attackTimer += Time.deltaTime;
				animation.CrossFade(animationAttackCurrent);
				//normal attack
				if(animationAttackCurrent == animationNormalAttack){
					//the attack animation has been played over
					if(attackTimer > timeNormalAttack){
						//cause the damager
						target.GetComponent<PlayerAttack>().TakeDamage(attack);
						//reset to the idle
						animationAttackCurrent = animationIdle;
					} 
				} 
				//crazy attack
				else if(animationAttackCurrent == animationCrazyAttack){
					if(attackTimer > timeCrazyAttack){
						//cause the damager
						//TODO
						//reset to the idle
						animationAttackCurrent = animationIdle;
					}
				}
				if(attackTimer > (1f/attackRate)){
					//attack again
					RandomAttack();
					//reset timer to 0
					attackTimer = 0;
				}
			} else {
				//distance between the max and min distance
				//move towrads to the player
				transform.LookAt(target);
				babyController.SimpleMove(transform.forward * moveSpeed);
				animation.CrossFade(animationWalk);
			}
		} else {
			state = WolfState.Idle;
		}
	}

	void RandomAttack(){
		float value = Random.Range (0f, 1f);
		if (value < crazyAttackRate) {
			//do the crazy attack
			animationAttackCurrent = animationCrazyAttack;
		} else {
			//do the normal attack
			animationAttackCurrent = animationNormalAttack;
		}
	}

	void OnDestroy(){
		GameObject.Destroy (hudtextGo);
		NPCController._instance.OnKillWolf ();
		born.MinusNumber ();
	}

	void OnMouseEnter(){
		CursorManager._instance.SetAttack ();
	}

	void OnMouseExit(){
		CursorManager._instance.SetNormal ();
	}
}
