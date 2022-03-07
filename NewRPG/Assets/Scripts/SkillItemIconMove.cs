using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SkillItemIconMove : MonoBehaviour
{

    private bool isMoveSkillItemIcon=false;
    private Transform parent;
    private GameObject skillItemIconCopy;


    void Awake()
    {
        parent = SkillManager._instance.transform.parent; //设置父物体
    }

    void Update()   
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveStart();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isMoveSkillItemIcon)
            {
                 MoveFinish();
            }
        }

        if (isMoveSkillItemIcon)
        {
            skillItemIconCopy.transform.position = Input.mousePosition;
        }
    }

    //开始移动图标
    void MoveStart()
    {
        List<RaycastResult> results = PublicFunctionManager.Instance.RaycastResultUIObject();
        foreach (RaycastResult temp in results)
        {
            if (temp.gameObject==this.gameObject)
            {
                skillItemIconCopy = Instantiate(this.gameObject);
                skillItemIconCopy.transform.parent=parent;
                isMoveSkillItemIcon = true;
            }
        }
    }

    void MoveFinish()
    {
        GameObject go = null;
        BagObjectMove goBagObject = null;
        isMoveSkillItemIcon = false;
        Destroy(skillItemIconCopy);
        //创建快捷方式
        List<RaycastResult> results = PublicFunctionManager.Instance.RaycastResultUIObject();
        foreach (RaycastResult temp in results)
        {
            if (temp.gameObject.tag==TagsManager.shortCutGrid)
            {
                go = temp.gameObject;
            }
            if (temp.gameObject.tag==TagsManager.bagObject)
            {
                goBagObject = temp.gameObject.GetComponent<BagObjectMove>();
            }
        }

        if (go!=null)
        {
            if (goBagObject!=null)
            {
                BagManager._instance.GetObject(goBagObject.id, goBagObject.count, 2);
                Destroy(goBagObject.gameObject);
            }
            go.GetComponent<ShortCut>().CreatSkillItemShortCut(transform.parent.GetComponent<SkillItem>().id);
        }
    }
}
