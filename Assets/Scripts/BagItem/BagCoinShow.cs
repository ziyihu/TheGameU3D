using UnityEngine;
using System.Collections;

public class BagCoinShow : MonoBehaviour {

	public UILabel coinNumLabel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ShowCoins ();
	}

	public void ShowCoins(){
		coinNumLabel.text = PlayerStatus._instance.coin.ToString ();
	}
}
