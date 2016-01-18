using UnityEngine;
using System.Collections;

public class BagItemController : MonoBehaviour {
	//the item id
	public int id;
	//the item number
	public int number;

	private PlayerStatus ps;

	public UILabel numberLabel;


	// Use this for initialization
	void Start () {
		numberLabel = transform.GetComponentInChildren<UILabel> ();
		ps = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ShopMed._instance.isSuccess) {
			AddItem(ShopMed._instance.id);
		}
	}

	public void AddItem(int buyId){
		if (id == buyId) {
			number += 1;
			numberLabel.text = "X" + this.number.ToString();
		}
	}

	//Use this item, minus one, add hp or mp
	public void MinusItem(){
		if(number >= 0){
			number -= 1;
			numberLabel.text = "X" + this.number.ToString();

			if (id == 1) {
				ps.DrugUse(100,0);
			}else if(id == 2){
				ps.DrugUse(300,0);
			}else if(id == 3){
				ps.DrugUse(0,100);
			}else if(id == 4){
				ps.DrugUse(300,300);
			}else if(id == 5){
				ps.DrugUse(200,200);
			}
		} else {
			return;
		}
	}


}
