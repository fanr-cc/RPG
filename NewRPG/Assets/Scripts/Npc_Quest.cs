using UnityEngine;
using System.Collections;

public class Npc_Quest : MonoBehaviour
{
    //鼠标位于该GameObject的Collider上是每帧调用该方法。
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)&&!PublicFunctionManager.Instance.IsPointerOverUIObject())
        {
            QuestManager._instance.ShowQuest();
        }
    }
}
