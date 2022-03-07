using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipObject : MonoBehaviour
{

    public int id;
    private ObjectInfo info;
    public Image equipImage;//不知道为什么，预制体的Image组建不能通过GetComponent获取；


    // Use this for initialization


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DropEquip();//右键卸除装备
        }
    }

    void DropEquip ()
    {
        List<RaycastResult> raycastObject = PublicFunctionManager.Instance.RaycastResultUIObject();
        //遍历检测结果，如果检测到鼠标点击自身，则物体开始移动
        foreach (RaycastResult temp in raycastObject)
        {
            if (temp.gameObject == this.gameObject)
            {
                PlayerStatus._instance.RemoveEquipList(id);
                PlayerStatus._instance.EquipChange();
                BagManager._instance.GetObject(id, 1);//将身上装备换下
                Destroy(this.gameObject);
            }
        }
    }


    public void SetNewEquipById(int id) //创建新装备，设置其图片
    {
        this.id = id;
        //通过ID查找物体信息    
        info = ObjectInfoManager._instance.GetObjectInfoById(id);
        //在文件夹中查找物体图片
        equipImage.sprite = Resources.Load("bagObject\\" + info.icon_name, typeof(Sprite)) as Sprite;
    }
}
