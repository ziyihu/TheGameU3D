using UnityEngine;
using System.Collections;

public class NPCController : NPC {

	public static NPCController _instance;

	//get the tween of the task
	public TweenPosition taskTween;
	//does the player accept the task(default is false)
	public bool isInTask = false;
	//how many wolfs have been killed
	public int killCount = 0;
	//get the UILabel and change the content(after accept the task) 
	public UILabel desLabel;

	//Get all the buttons in the Task UI, control the buttons
	public GameObject acceptButton;
	public GameObject submitButton;
	public GameObject cancelButton;
	public GameObject giveupButton;

	//get the player status, add the coins
	private PlayerStatus status;

	void Awake(){
		_instance = this;
	}

	//when the mouse position is above the collider, this function will be called once per frame
	void OnMouseOver(){
		//when use the left mouse button click the task NPC
		if (Input.GetMouseButtonDown (0)) {
			if(isInTask){
				ShowTaskProgress();
			} else {
				ShowTaskDes();
			}
			ShowTask();
		}
	}

	//show the task UI
	void ShowTask(){
		//initiate the tween
		taskTween.gameObject.SetActive (true);
		//play the tween
		taskTween.PlayForward ();
	}

	//hide the task UI
	void HideTask(){
		taskTween.PlayReverse ();
	}

	//show the task description
	void ShowTaskDes(){
		//accept the task, the label will change
		desLabel.text = "New Task:\nKill 3 wolfs\n\nReward:\n1000 coins";
		//accept the task, so the accept button will change to the submit button
		submitButton.SetActive (false);
		acceptButton.SetActive (true);
		cancelButton.SetActive (true);
		giveupButton.SetActive (false);
	}
	//show the task progress
	void ShowTaskProgress(){
		//accept the task, the label will change
		desLabel.text = "Taken Task:\nAlready killed " + killCount + "/3 wolfs\n\nReward:\n1000 coins";
		//accept the task, so the accept button will change to the submit button
		submitButton.SetActive (true);
		acceptButton.SetActive (false);
		cancelButton.SetActive (false);
		giveupButton.SetActive (true);
	}


	//Task UI(close button clicked)
	public void OnCloseButtonClick(){
		HideTask ();
	}

	//Task UI(accept button clicked)
	public void OnAcceptButtonClick(){
		//show the different UI after click the accept button
		ShowTaskProgress ();
		//set the isInTask true
		isInTask = true;
	}

	//Task UI(submit button clicked)
	public void OnSubmitButtonClick(){
		//finish the task
		if (killCount >= 3) {
			status.GetCoin(1000);
			killCount = 0;
			ShowTaskDes();
		} 
		//doesn't finish the task yet
		else {
			HideTask();
		}
	}

	//Task UI(give up button clicked)
	public void OnGiveupButtonClick(){
		ShowTaskDes ();
		killCount = 0;
		isInTask = false;
	}

	// Use this for initialization
	void Start () {
		status = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnKillWolf(){
		if (isInTask) {
			killCount++;		
		}
	}

}
