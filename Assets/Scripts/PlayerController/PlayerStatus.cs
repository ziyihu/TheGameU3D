using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public static PlayerStatus _instance;

	public int grade = 1;
	public int hp = 1000;
	public int mp = 1000;
	public int coin = 10000;

	public float hp_remain=100;
	public float mp_remain=100;

	//attack damage
	public float attack = 30;
	//defeat(when the others attack the player, it can reduce the damge)
	public int defeat = 30;
	//move speed
	public int speed = 20;

	public void GetCoin(int count){
		coin += count;
	}

	//use the coin, count is how many coins you wanna use
	public bool UseCoin(int count){
		if (coin >= count) {
			coin -= count;
			return true;
		}
		return false;
	}

	public void DrugUse(int hp,int mp) {
		hp_remain += hp;
		mp_remain += mp;
		if (hp_remain >= this.hp) {
			hp_remain = this.hp;
		}
		if (mp_remain >= this.mp) {
			mp_remain = this.mp;		
		}
		HeadStatusUI._instance.UpdateShow ();
	}

	public bool GetMP(int count){
		if(mp_remain >= count){
			mp_remain -= count;
			HeadStatusUI._instance.UpdateShow();
			return true;
		} else {
			return false;
		}
	}

	void Awake(){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
