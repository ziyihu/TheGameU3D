using UnityEngine;
using System.Collections;

public class MedicineNPC : NPC {
	//when the mouse in on the object, will call this function
	public void OnMouseOver(){
		//if left mouse button click on the object
		//show the medicine store UI
		if (Input.GetMouseButtonDown (0)) {
			ShopMed._instance.TransformState();
		}
	}
}
