using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState {
	ControlWalk,
	NormalAttack,
	SkillAttack,
	Death
}

public enum AttackState{
	Moving,
	Idle,
	Attack
}

public class PlayerAttack : MonoBehaviour {

	public AudioClip missClip;
	public AudioClip attackClip;
	public AudioClip buffskillClip;
	public AudioClip healskillClip;
	public AudioClip singltonskillClip;
	public AudioClip multiskillClip;

	public AttackState attackState = AttackState.Idle;

	public PlayerState state = PlayerState.ControlWalk;
	//normal attack animation name
	public string animationNormalAttack = "Sword-Attack1";

	public string animationIdle = "Sword-Idle";
	public string animationNow = "Sword-Attack1";

	//normal attack time
	public float timeNormalAttack;
	//attack 1 time per second
	public float rateNormalAttack = 1;
	//timer
	private float attackTimer = 0;
	//attack distance
	public float minDistance = 2;
	//normal attack damage
	public int normalAttack = 30;

	//normal attack effect
	public GameObject effect;

	private Transform targetNormalAttack;

	private PlayerWalk walk;
	//show the normal attack effect
	public bool showEffect = false;

	private PlayerStatus ps;

	//miss rate
	public float missRate = 0.25f;
	//general hud text prefab init
	public GameObject hudtextPrefab;
	//current hud text game object
	private GameObject hudtextGo;
	//follow the player hud text
	private GameObject hudtextFollow;
	private HUDText hudtext;
	//effext dictionary
	public GameObject[] efxArray;
	private Dictionary<string,GameObject> efxDict = new Dictionary<string, GameObject>();
	//using single target skill
	//need to locking an enemy
	private bool isLockingTarget = false;

	private SkillInfo skillInfo;


	void Awake(){
		walk = this.GetComponent<PlayerWalk> ();
		ps = this.GetComponent<PlayerStatus> ();
		hudtextFollow = transform.Find ("HUDText").gameObject;

		foreach (GameObject go in efxArray) {
			efxDict.Add(go.name,go);
		}
	}

	void Start(){
		hudtextGo = NGUITools.AddChild (HUDTextRoot._instance.gameObject, hudtextPrefab);
		hudtext = hudtextGo.GetComponent<HUDText> ();
		UIFollowTarget followTarget = hudtextGo.GetComponent<UIFollowTarget> ();
		followTarget.target = hudtextFollow.transform;
		followTarget.gameCamera = Camera.main;
	}


	void Update(){
		//when click on an enemy
		if (isLockingTarget == false && Input.GetMouseButtonDown (0) && state != PlayerState.Death) {
			//use a ray to detect whether the click is above an enemy
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			bool isCollider = Physics.Raycast(ray, out hitInfo);

			//is collider and get the enemy
			if(isCollider && hitInfo.collider.tag == "Enemy"){
				//when we clicked on an enemy
				targetNormalAttack = hitInfo.collider.transform;
				//normal attack state
				state = PlayerState.NormalAttack;
				attackTimer = 0;
				showEffect = false;
			}
			//click on anything else(not enemy)
			else {
				state = PlayerState.ControlWalk;
				targetNormalAttack = null;
			}
		}

		if (state == PlayerState.NormalAttack) {
			//if the enemy is dead or not exist
			if(targetNormalAttack == null){
				state = PlayerState.ControlWalk;
			} else {
			float distance = Vector3.Distance(transform.position, targetNormalAttack.position);
			if(distance <= minDistance){
				transform.LookAt(targetNormalAttack.position);
				//the enemy is in the attack distance
				//do the attack
				attackState = AttackState.Attack;
				attackTimer += Time.deltaTime;
				animation.CrossFade(animationNow);
				//timer is more than the normal attack time
				if(attackTimer >= timeNormalAttack){
					animationNow = animationIdle;
					//make sure only show one time effect
					if(showEffect == false){
						showEffect = true;
						//show the effect
						GameObject.Instantiate(effect, targetNormalAttack.position,Quaternion.identity);
						targetNormalAttack.GetComponent<WolfBaby>().TakeDamge(normalAttack);
					}
				}
				if(attackTimer >= (1f/rateNormalAttack))
				{//end the timer,attack again
					attackTimer = 0;
					showEffect = false;
					animationNow = animationNormalAttack;
				}
			} 
			else {
				//the enemy is not in the range
				//go towards to the enemy and then do the attack
				attackState = AttackState.Moving;
				walk.SimpleMove(targetNormalAttack.position);
				//show the aimation
			}
			}
		} else if(state == PlayerState.Death){
			animation.CrossFade("Sword-Death");

		}

		if(isLockingTarget && Input.GetMouseButtonDown(0)){
			OnLockTarget();
		}
	}

	
	public void TakeDamage(int attack) {
		if (state == PlayerState.Death)	return;
		float value = Random.Range(0f,1f);
		if(value < missRate){
			AudioSource.PlayClipAtPoint(missClip,transform.position,1f);
			hudtext.Add("MISS",Color.gray,1);
		} else {
			//play the audio
			AudioSource.PlayClipAtPoint(attackClip,transform.position,1f);
			hudtext.Add("-"+attack,Color.white,1);
			ps.hp_remain -= attack;
			if(ps.hp_remain <= 0){
				state = PlayerState.Death;
			}
		}
		HeadStatusUI._instance.UpdateShow ();
	}

	void OnDestory(){
		GameObject.Destroy (hudtextGo);
	}

	public void UseSkill(SkillInfo skillInfo){
		switch (skillInfo.applyType) {
		case ApplyType.Passive:
			StartCoroutine(OnPassiveSkillUse(skillInfo));
			break;
		case ApplyType.Buff:
			StartCoroutine(OnBuffSkillUse(skillInfo));
			break;
		case ApplyType.SingleTarget:
			OnSingleTargetSkillUse(skillInfo);
			break;
		case ApplyType.MultiTarget:
			OnMultiTargetSkillUse(skillInfo);
			break;
		}
	}
	//deal with the add hp and mp skill
	IEnumerator OnPassiveSkillUse(SkillInfo info){
		AudioSource.PlayClipAtPoint (healskillClip, transform.position, 1f);
		state = PlayerState.SkillAttack;
		animation.CrossFade (info.aniname);
		yield return new WaitForSeconds(info.anitime);
		state = PlayerState.ControlWalk;
		int hp = 0;
		int mp = 0;
		if (info.applyProperty == ApplyProperty.HP) {
			hp = info.applyValue;
		} else {
			mp = info.applyValue;
		}
		//instance the effect
		GameObject prefab = null;
		efxDict.TryGetValue (info.efx_name, out prefab);
		GameObject.Instantiate (prefab, transform.position, Quaternion.identity);
		//after the animation, add the hp or mp
		ps.DrugUse (hp, mp);
	}
	//deal with the buff skill
	IEnumerator OnBuffSkillUse(SkillInfo info){
		AudioSource.PlayClipAtPoint (buffskillClip, transform.position, 1f);
		state = PlayerState.SkillAttack;
		animation.CrossFade (info.aniname);
		yield return new WaitForSeconds(info.anitime);
		state = PlayerState.ControlWalk;
		switch (info.applyProperty) {
		case ApplyProperty.Attack:
			normalAttack = normalAttack + info.applyValue;
			break;
		case ApplyProperty.AttackSpeed:
			rateNormalAttack = rateNormalAttack * (info.applyValue/100f);
			break;
		}
		//buff last time
		yield return new WaitForSeconds (info.applyTime);
		//after the time, remove the buff
		switch (info.applyProperty) {
		case ApplyProperty.Attack:
			normalAttack = normalAttack - info.applyValue;
			break;
		case ApplyProperty.AttackSpeed:
			rateNormalAttack = rateNormalAttack / (info.applyValue/100f);
			break;
		}
	}
	//deal with single target skill
	void OnSingleTargetSkillUse(SkillInfo info){
		state = PlayerState.SkillAttack;
		isLockingTarget = true;
		skillInfo = info;
	}
	//deal with multi target skill
	void OnMultiTargetSkillUse(SkillInfo info){
		state = PlayerState.SkillAttack;
		isLockingTarget = true;
		skillInfo = info;
	}
	void OnLockTarget(){
		isLockingTarget = false;
		switch (skillInfo.applyType) {
		case ApplyType.SingleTarget:
			StartCoroutine(OnLockSingleTarget());
			break;
		case ApplyType.MultiTarget:
			StartCoroutine(OnLockMultiTarget());
			break;
		}
	}
	IEnumerator OnLockSingleTarget(){
		//detect if the place you click is on the ground
		//get the ray from the point you click
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//get the hit information
		RaycastHit hitInfo;
		//if the ray hit something isCollider is true, if the ray didn't hit anything return false
		bool isCollider = Physics.Raycast(ray, out hitInfo);
		//if the ray hit something and that thing is the ground
		if(isCollider && hitInfo.collider.tag == "Enemy") {
			AudioSource.PlayClipAtPoint (singltonskillClip, transform.position, 1f);
			//select an enemy
			animation.CrossFade (skillInfo.aniname);
			yield return new WaitForSeconds(skillInfo.anitime);
			state = PlayerState.ControlWalk;
			//show the effect
			GameObject prefab = null;
			efxDict.TryGetValue (skillInfo.efx_name, out prefab);
			GameObject.Instantiate (prefab, hitInfo.collider.transform.position, Quaternion.identity);
			hitInfo.collider.GetComponent<WolfBaby>().TakeDamge(110);
		} else {
			state = PlayerState.ControlWalk;
		}
	}
	IEnumerator OnLockMultiTarget(){
		//detect if the place you click is on the ground
		//get the ray from the point you click
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//get the hit information
		RaycastHit hitInfo;
		//if the ray hit something isCollider is true, if the ray didn't hit anything return false
		bool isCollider = Physics.Raycast(ray, out hitInfo,9);
		//9's layer is the ground
		//if the ray hit something and that thing is the ground
		if(isCollider) {
			AudioSource.PlayClipAtPoint (multiskillClip, transform.position, 1f);
			//select a position
			animation.CrossFade (skillInfo.aniname);
			yield return new WaitForSeconds(skillInfo.anitime);
			state = PlayerState.ControlWalk;
			//show the effect
			GameObject prefab = null;
			efxDict.TryGetValue (skillInfo.efx_name, out prefab);
			GameObject.Instantiate (prefab, hitInfo.point + Vector3.up*0.5f, Quaternion.identity);
	//		hitInfo.collider.GetComponent<WolfBaby>().TakeDamge(110);
		} else {
			state = PlayerState.ControlWalk;
		}
	}
}

