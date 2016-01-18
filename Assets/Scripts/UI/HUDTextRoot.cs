using UnityEngine;
using System.Collections;

public class HUDTextRoot : MonoBehaviour {

	public static HUDTextRoot _instance;

	void Awake(){
		_instance = this;
	}
}
