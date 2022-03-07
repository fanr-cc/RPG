using UnityEngine;
using System.Collections;

public class Npc_EquipStore : MonoBehaviour {

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)&&!PublicFunctionManager.Instance.IsPointerOverUIObject())
        {
            BagManager._instance.ShowBag();
            EquipStoreManager._instance.ShowEquipStore();
        }
    }
}
