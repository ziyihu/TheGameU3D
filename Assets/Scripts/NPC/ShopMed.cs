using UnityEngine;
using System.Collections;

public class ShopMed : MonoBehaviour {

	//singlton desgin pattern(only this one instance in the whole project)
	public static ShopMed _instance;
	//get the tween
	public TweenPosition ShopUITweenPosition;
	//is the shop UI showing
	//default is false
	private bool isShow = false;
	//buy item id
	public int id;

	public bool isSuccess = false;

	void Awake(){
		_instance = this;
	}

	public void TransformState(){
		//when you click on the character, show the Shop UI, or hide the Shop UI
		if (isShow == false) {
			//show the Shop UI
			ShopUITweenPosition.PlayForward();
			isShow = true;
		} else {
			//hide the Shop UI
			ShopUITweenPosition.PlayReverse();
			isShow = false;
		}
	}

	//click the cancle button, hide the Shop UI
	public void OnCancleButtonClick(){
		ShopUITweenPosition.PlayReverse ();
	}

	//buy the first Item in the Shop
	public void OnBuyId1(){
		Buy (1,50);
	}
	//buy the second Item in the Shop
	public void OnBuyId2(){
		Buy (2,100);
	}
	//buy the third Item in the Shop
	public void OnBuyId3(){
		Buy (3,50);
	}
	//buy the forth Item in the Shop
	public void OnBuyId4(){
		Buy (4,150);
	}

	//the buy function
	void Buy(int id,int price){
		//if the player have enough coins to buy the item
		bool success = PlayerStatus._instance.UseCoin (price);
		//have enough coins
		if (success) {
			isSuccess = true;
			this.id = id;
		}
		//not enough coins
		else {
			isSuccess = false;
		}
	}

	void Update(){
		isSuccess = false;
	}
}
