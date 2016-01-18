using UnityEngine;
using System.Collections;

public class HeadStatusUI : MonoBehaviour {

	public static HeadStatusUI _instance;

	private UISlider hpBar;
	private UISlider mpBar;

	private UILabel hpLabel;
	private UILabel mpLaebl;

	private PlayerStatus ps;

	void Awake(){
		_instance = this;
		hpBar = transform.Find ("HPBar").GetComponent<UISlider> ();
		mpBar = transform.Find ("MPBar").GetComponent<UISlider> ();
		hpLabel = transform.Find ("HPBar/Thumb/Label").GetComponent<UILabel> ();
		mpLaebl = transform.Find ("MPBar/Thumb/Label").GetComponent<UILabel> ();

	}

	void Start(){
		ps = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStatus> ();
		UpdateShow ();
	}



	public void UpdateShow(){
		hpBar.value = ps.hp_remain / ps.hp;
		mpBar.value = ps.mp_remain / ps.mp;

		hpLabel.text = ps.hp_remain + "/" + ps.hp;
		mpLaebl.text = ps.mp_remain + "/" + ps.mp;
	}
}
