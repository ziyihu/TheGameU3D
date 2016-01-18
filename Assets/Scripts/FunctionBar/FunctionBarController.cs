using UnityEngine;
using System.Collections;

public class FunctionBarController : MonoBehaviour {

	//Get all the buttons in the Function bar
//	public GameObject EquipBtn;
//	public GameObject BagBtn;
//	public GameObject SkillBtn;
//	public GameObject TaskBtn;
//	public GameObject StoreBtn;
//	public GameObject SystemBtn;

	//bag UI tween position
	public TweenPosition BagTweenPosition;
	//system UI tween position
	public TweenPosition SystemTweenPosition;
	//skill UI tween position
	public TweenPosition SkillTweenPosition;

	//show the bag UI
	public void ShowBagTweenPosition(){
		BagTweenPosition.gameObject.SetActive (true);
		BagTweenPosition.PlayForward ();
	}
	//hide the bag UI
	public void HideBagTweenPosition(){
		BagTweenPosition.PlayReverse ();
	}

	//show the system UI
	public void ShowSystemTweenPosition(){
		SystemTweenPosition.gameObject.SetActive (true);
		SystemTweenPosition.PlayForward ();
	}
	//hide the system UI
	public void HideSystemTweenPosition(){
		SystemTweenPosition.PlayReverse ();
	}

	//show the skill UI
	public void ShowSkillTweenPosition(){
		SkillTweenPosition.gameObject.SetActive (true);
		SkillTweenPosition.PlayForward ();
	}
	//hide the skill UI
	public void HideSkillTweenPosition(){
		SkillTweenPosition.PlayReverse ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
