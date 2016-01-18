using UnityEngine;
using System.Collections;

public class SkillUI : MonoBehaviour {

	public static SkillUI _instance;

	public int[] magicianSkillIdList;
	//public int[] swordmanSkillIdList;

	public UIGrid grid;
	public GameObject skillItemPrefab;

	void Awake(){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		foreach (int id in magicianSkillIdList){
			//add a skillItem to the grid
			GameObject itemGo = NGUITools.AddChild(grid.gameObject,skillItemPrefab);
			//let the grid manage the skill item prefab
			grid.AddChild(itemGo.transform);
			//show the item
			itemGo.GetComponent<SkillItem>().SetId(id);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
