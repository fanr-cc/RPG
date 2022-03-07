using UnityEngine;
using System.Collections;

public class Npc_DrugStore : MonoBehaviour {

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)&&!PublicFunctionManager.Instance.IsPointerOverUIObject())
        {
            BagManager._instance.ShowBag();
            DrugStoreManager._instance.ShowDrugStore();
        }
    }
}
