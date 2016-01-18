using UnityEngine;
using System.Collections;

public class MedicBarUI : MonoBehaviour {
	//press 1-6 button to use the medic 
	//add HP or MP
	public KeyCode keyCode;
	private PlayerStatus ps;

	// Use this for initialization
	void Start () {
		ps = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ();
	}
	
	// Update is called once per frame
	void Update () {
		//press the keyCode
		if (Input.GetKeyDown (keyCode)) {
			if (keyCode == KeyCode.Alpha1) {
				ps.DrugUse(100,0);
				HeadStatusUI._instance.UpdateShow();
			}else if(keyCode == KeyCode.Alpha2){
				ps.DrugUse(300,0);
				HeadStatusUI._instance.UpdateShow();
			}else if(keyCode == KeyCode.Alpha3){
				ps.DrugUse(0,100);
				HeadStatusUI._instance.UpdateShow();
			}else if(keyCode == KeyCode.Alpha4){
				ps.DrugUse(300,300);
				HeadStatusUI._instance.UpdateShow();
			}else if(keyCode == KeyCode.Alpha5){
				ps.DrugUse(200,200);
				HeadStatusUI._instance.UpdateShow();
			}
		}
	}
}
