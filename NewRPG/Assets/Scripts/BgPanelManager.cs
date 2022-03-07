using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class BgPanelManager : MonoBehaviour
{//当鼠标点击面板标题时，拖动面板
    private bool isBgPanelMove = false;
    private Transform bgPanel;
    private Vector3 distancePosition=Vector3.zero;//记录面板和鼠标的相距位置
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> results = PublicFunctionManager.Instance.RaycastResultUIObject();
            foreach (RaycastResult temp  in results)
            {
                if (temp.gameObject.tag==TagsManager.bgPanel)
                {
                    bgPanel = temp.gameObject.transform.parent;
                    distancePosition = Input.mousePosition - bgPanel.position;
                    isBgPanelMove = true;
                    break;
                }
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            isBgPanelMove = false;
        }
        if (isBgPanelMove)
        {
            bgPanel.position = Input.mousePosition - distancePosition;
        }
    }
}
