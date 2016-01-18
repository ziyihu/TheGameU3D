using UnityEngine;
using System.Collections;

public class UseSkillControl : MonoBehaviour {

	public KeyCode keyCode;
	private PlayerStatus ps;
	private PlayerAttack pa;
	private SkillInfo skillInfo;

	// Use this for initialization
	void Start () {
		ps = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ();
		pa = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();

	}
	
	// Update is called once per frame
	void Update () {
		//Press any keys
		if (Input.GetKeyDown (keyCode)) {
			//key W, add HP skill.
			if (keyCode == KeyCode.W) {
				//set the skill information
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5002);
				//release the skill
				//1.get the skill required mp
				bool success = ps.GetMP(skillInfo.mp);
				if (success) {
					//use the MP,release the skill
					pa.UseSkill(skillInfo);
				} else {
				
				}
			}
			//key E, add MP skill
			else if(keyCode == KeyCode.E){
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5003);
				pa.UseSkill(skillInfo);
			}
			//Key R, buff attack
			else if(keyCode == KeyCode.R){
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5004);
				bool success = ps.GetMP(skillInfo.mp);
				if (success) {
					pa.UseSkill(skillInfo);
				}
			}
			//Key T, buff attack speed
			else if(keyCode == KeyCode.T){
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5005);
				bool success = ps.GetMP(skillInfo.mp);
				if (success) {
					pa.UseSkill(skillInfo);
				}
			}
			//Key Q, magic ball
			else if(keyCode == KeyCode.Q){
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5001);
				bool success = ps.GetMP(skillInfo.mp);
				if (success) {
					pa.UseSkill(skillInfo);
				}
			}
			//Key Y, multi target skill
			else if(keyCode == KeyCode.Y){
				skillInfo = SkillsInfo._instance.GetSkillInfoById(5006);
				bool success = ps.GetMP(skillInfo.mp);
				if (success) {
					pa.UseSkill(skillInfo);
				}
			}
		} 

	

	}

}