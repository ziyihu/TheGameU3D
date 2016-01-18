using UnityEngine;
using System.Collections;

public class BabyWolfBorn : MonoBehaviour {

	public int maxNum = 6;
	private int currentNum = 0;

	public float time = 3;
	private float timer = 0;

	//wolf baby perfab
	public GameObject prefabBabyWolf;

	void Update(){
		if (currentNum <= maxNum) {
			timer += Time.deltaTime;
			if(timer >= time){
				Vector3 pos = transform.position;
				pos.x += Random.Range(-3,3);
				pos.z += Random.Range(-3,3);
				GameObject go = GameObject.Instantiate(prefabBabyWolf, pos , Quaternion.identity) as GameObject;
				go.GetComponent<WolfBaby>().born = this;
				timer = 0;
				currentNum += 1;
			}
		}
	}

	public void MinusNumber(){
		currentNum -= 1;
	}
}
