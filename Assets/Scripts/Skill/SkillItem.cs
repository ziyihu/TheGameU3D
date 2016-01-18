using UnityEngine;
using System.Collections;

public class SkillItem : MonoBehaviour {
	
	public int id;
	private SkillInfo info;
	
	private UISprite iconname_sprite;
	private UILabel name_label;
	private UILabel applytype_label;
	private UILabel des_label;
	private UILabel mp_label;
	
	
	void InitProperty() {
		iconname_sprite = transform.Find("icon_name").GetComponent<UISprite>();
		name_label = transform.Find("property/name").GetComponent<UILabel>();
		applytype_label = transform.Find("property/applytype").GetComponent<UILabel>();
		des_label = transform.Find("property/des").GetComponent<UILabel>();
		mp_label = transform.Find("property/MP").GetComponent<UILabel>();
	}
	
	//show the update skills
	public void SetId(int id) {
		InitProperty();
		this.id = id;
		info = SkillsInfo._instance.GetSkillInfoById(id);
		iconname_sprite.spriteName = info.icon_name;
		name_label.text = info.name;
		switch (info.applyType) {
		case ApplyType.Passive:
			applytype_label.text = "Passive";
			break;
		case ApplyType.Buff:
			applytype_label.text = "Buff";
			break;
		case ApplyType.SingleTarget:
			applytype_label.text = "Single Target";
			break;
		case ApplyType.MultiTarget:
			applytype_label.text = "Multi Target";
			break;
		}
		des_label.text = info.des;
		mp_label.text = info.mp + "MP";
	}
	
}
