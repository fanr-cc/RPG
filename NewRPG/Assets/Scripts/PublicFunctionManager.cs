using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PublicFunctionManager : MonoBehaviour {

    private static PublicFunctionManager _instance;
    public GameObject takeDamageTextPrefab;
    private Transform parent;


    //efx
    public GameObject[] efxArray;
    private Dictionary<string, GameObject> efxDic = new Dictionary<string, GameObject>();


    public static PublicFunctionManager Instance{get{return _instance;}}
   
    void Awake()
    {
        _instance = this;
        parent = GameObject.FindGameObjectWithTag(TagsManager.enemyInfo).transform;

        foreach (GameObject temp in efxArray)
        {
            efxDic.Add(temp.name, temp);
        }
    }


    public GameObject GetEfxByName(string efxName)
    {
        GameObject efx = null;
        efxDic.TryGetValue(efxName, out efx);
        return efx;
    }
    //判断鼠标是否点击在UI界面上
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    //射线检测鼠标下UI界面的的物体
    public List<RaycastResult> RaycastResultUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }


    public void SetTakeDamageText(int damage,Transform trans)
    {
        GameObject takeDamageText = Instantiate(takeDamageTextPrefab);
        takeDamageText.transform.parent = parent;
        takeDamageText.transform.position = Camera.main.WorldToScreenPoint(trans.position) + new Vector3(0, 50, 0);

        takeDamageText.GetComponent<EnemyInfo>().SetInfo(trans);
        takeDamageText.GetComponent<Text>().text = "-" + damage;
    }


    public void LookAtHorizontal(Vector3 position,Transform trans)
    {
        //目标位置的Y轴坐标和自身保持一直，使角色只在X，Z轴旋转。
        position.y = trans.position.y;
        //角色朝向目标位置
        trans.LookAt(position);
    }
}
