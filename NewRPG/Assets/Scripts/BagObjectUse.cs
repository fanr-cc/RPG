using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BagObjectUse : MonoBehaviour
{
   
    public GameObject equipObjectPrefab;//用于实例化装备面板上的装备的预制体
    private EquipObject equipObjectCSharp;//用于记录装备物体的CSharp脚本
    private BagObjectMove bagObjectMove;//用于获取本物体的ID等属性

    void Awake()
    {
        bagObjectMove = GetComponent<BagObjectMove>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {//射线检测
            List<RaycastResult> raycastObject = PublicFunctionManager.Instance.RaycastResultUIObject();
            //遍历检测结果，如果检测到鼠标点击自身，则物体开始移动
            foreach (RaycastResult temp in raycastObject)
            {
                if (temp.gameObject == this.gameObject)
                {
                    ObjectUse();
                }
            }
        }
    }

    


    public void ObjectUse()
    {
        Transform parent = null;//每次搜索都初始化其属性//用于记录装备的部位，装备物体的时候在该部位下创建预制体
        ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(bagObjectMove.id);
        switch (info.type)
        {
            case ObjectType.Drug:
            PlayerStatus._instance.HpChange(info.hp);
            PlayerStatus._instance.MpChange(info.mp);
            if (bagObjectMove.count>1)
            {
                bagObjectMove.ChangeCount(-1);
            }
            else
            {
                Destroy(gameObject);
            }  
            break;

            case ObjectType.Equip:
            switch (info.careerType)
            {
                case CareerType.Magician:
                    if (PlayerStatus._instance.HeroType==HeroType.Swordman)
                    {
                    ErrorManager._instance.EquipmentNotMatch();
                    return;
                    }
                    break;

                case CareerType.Swordman:
                    if (PlayerStatus._instance.HeroType == HeroType.Magician)
                    {
                        ErrorManager._instance.EquipmentNotMatch();
                        return;
                    }
                    break;
            }

            switch (info.equipType)
            {
                case EquipType.Accessory:
                    parent = EquipManager._instance.accessory;
                    break;
                case EquipType.Armor:
                    parent = EquipManager._instance.armor;
                    break;
                case EquipType.Headgear:
                    parent = EquipManager._instance.headgear;
                    break;
                case EquipType.LeftHand:
                    parent = EquipManager._instance.leftHand;
                    break;
                case EquipType.RightHand:
                    parent = EquipManager._instance.rightHand;
                    break;
                case EquipType.Shoe:
                    parent = EquipManager._instance.shoe;
                    break;
                }
                if (parent != null)
                {
                    equipObjectCSharp = parent.GetComponentInChildren<EquipObject>();
                    if (equipObjectCSharp == null)
                    {//如果该部位没有物体。
                        GameObject go = Instantiate(equipObjectPrefab);//创建物品
                        go.transform.parent = parent;//父亲设置为查找到的空格
                        go.transform.localPosition = Vector3.zero;//物品的坐标归0
                        equipObjectCSharp = go.GetComponent<EquipObject>();
                        equipObjectCSharp.SetNewEquipById(info.id);
                        PlayerStatus._instance.AddEquipList(info.id);
                        PlayerStatus._instance.EquipChange();//根据更换的装备，更改角色属性
                    }
                    else
                    {  //如果该部位下有装备，更新其信息
                        BagManager._instance.GetObject(equipObjectCSharp.id,1);//将身上装备换下
                        PlayerStatus._instance.RemoveEquipList(equipObjectCSharp.id);

                        //改变物体图片
                        equipObjectCSharp.SetNewEquipById(info.id);//换上新的装备
                        PlayerStatus._instance.AddEquipList(info.id);

                        PlayerStatus._instance.EquipChange();//更改角色属性
                    }
                }

                if (bagObjectMove.count > 1)
                {
                    bagObjectMove.ChangeCount(-1);
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
            }           
    }
}
