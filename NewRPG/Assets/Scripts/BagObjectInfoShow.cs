using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BagObjectInfoShow : MonoBehaviour
{
  
    private BagObjectMove bagObjectMove;
    private EquipObject equipObject;
    private EquipStoreItem equipStoreItem;
    private bool isShowThisObjectInfo=false;
  

    // Use this for initialization
    void Awake () {
        bagObjectMove = GetComponent<BagObjectMove>();
        equipObject = GetComponent<EquipObject>();
        equipStoreItem = GetComponentInParent<EquipStoreItem>();
    }
	
	// Update is called once per frame
	void Update () {
        //如果当前焦点在UI界面上
        if (PublicFunctionManager.Instance.IsPointerOverUIObject())
        {//发射射线检测
            List<RaycastResult> raycastObject = PublicFunctionManager.Instance.RaycastResultUIObject();
            foreach (RaycastResult temp in raycastObject)
            {
                //如果检测到射线下面有自己，显示物品信息
                if (temp.gameObject==this.gameObject)
                {
                    switch (temp.gameObject.tag)
                    {
                        case TagsManager.bagObject:
                            BagManager._instance.SetDesText(bagObjectMove.id);
                            break;
                        case TagsManager.equipObject:
                            BagManager._instance.SetDesText(equipObject.id);
                            break;
                        case TagsManager.equipStoreItem:
                            BagManager._instance.SetDesText(equipStoreItem.id);
                            break;                       
                    }
                    isShowThisObjectInfo = true;
                    break;
                }
                else
                {
                    ReleaseObjectDes();
                }
            }
        }
    }
    void OnDestroy()
    {
        ReleaseObjectDes();
    }

    void ReleaseObjectDes()//释放持有的物品信息面板
    {
        if (isShowThisObjectInfo)//如果射线下不是自身，且自身已经显示过了，则隐藏
        {
            BagManager._instance.SetDesFalse();
            isShowThisObjectInfo = false;
        }
    }


}
