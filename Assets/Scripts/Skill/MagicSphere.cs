using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicSphere : MonoBehaviour {

	public int attack = 120;

	public List<WolfBaby> wolfList = new List<WolfBaby> ();

	public void OnTriggerEnter(Collider col){
		if (col.tag == "Enemy") {
			WolfBaby baby = col.GetComponent<WolfBaby>();
			int index = wolfList.IndexOf(baby);
			if(index == -1){
				baby.TakeDamge(attack);
				wolfList.Add(baby);
			}
		}
	}
}
