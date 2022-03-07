using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipStoreItem : MonoBehaviour
{


    public int id;
    public Image Icon;
    public Text nameText;
    public Text careerTypeText;
    public Text equipTypeText;
    public Text priceText;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetNewEquipStoreItemById(int id) //创建新技能，设置其图片
    {
        this.id = id;
        //通过ID查找物体信息    
        ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(id);
        //在文件夹中查找物体图片
        Icon.sprite = Resources.Load("bagObject\\" + info.icon_name, typeof(Sprite)) as Sprite;
        nameText.text = info.name;
        priceText.text = "售价:" + info.price_buy;
        switch (info.careerType)
        {
            case CareerType.Magician:
                careerTypeText.text = "职业：Magician";
                break;
            case CareerType.Swordman:
                careerTypeText.text = "职业：Swordman";
                break;
            case CareerType.Common:
                careerTypeText.text = "职业：Common";
                break;
        }
        switch (info.equipType)
        {
            case EquipType.Accessory:
                equipTypeText.text = "部位：Accessory";
                break;
            case EquipType.Armor:
                equipTypeText.text = "部位：Armor";
                break;
            case EquipType.Headgear:
                equipTypeText.text = "部位：Headgear";
                break;
            case EquipType.LeftHand:
                equipTypeText.text = "部位：LeftHand";
                break;
            case EquipType.RightHand:
                equipTypeText.text = "部位：RightHand";
                break;
            case EquipType.Shoe:
                equipTypeText.text = "部位：Shoe";
                break;
        }
    }


    public void OnBuyEquip()
    {
        DrugStoreManager._instance.Buy(id);
    }
}
