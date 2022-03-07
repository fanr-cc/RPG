using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour {

    public static ErrorManager _instance;

    private PlayerAction playerAction;
    public GameObject errorPanelPrefab;



    public GameObject diePanel;
    public GameObject discardObjectPanal;
    private GameObject ObjectGameObject;

    public GameObject sellPanal;
    private GameObject sellGameObject;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        playerAction = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
        diePanel.transform.localPosition = Vector3.zero;
        diePanel.SetActive(false);
        discardObjectPanal.SetActive(false);
        sellPanal.SetActive(false);
    }

    public void CoinLack(int have,int need)
    {
        GameObject go = Instantiate(errorPanelPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.parent = BagManager._instance.transform.parent;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<SetErrorPanelText>().desText.text = "您需要"+need+ "个金币，您现在只有" + have + "个金币，缺少" + (need- have) + "金币";
    }

    public void BagFull()
    {
        GameObject go = Instantiate(errorPanelPrefab);
        go.transform.parent = BagManager._instance.transform.parent;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<SetErrorPanelText>().desText.text = "您的背包慢了，赶紧去整理背包";
    }

    public void EquipmentNotMatch()
    {
        GameObject go = Instantiate(errorPanelPrefab);
        go.transform.parent = BagManager._instance.transform.parent;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<SetErrorPanelText>().desText.text = "装备不符合角色类型，装备失败";
    }

    public void SkillCD()
    {
        GameObject go = Instantiate(errorPanelPrefab);
        go.transform.parent = BagManager._instance.transform.parent;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<SetErrorPanelText>().desText.text = "技能正在CD，请稍等片刻";
    }

    public void MpLack()
    {
        GameObject go = Instantiate(errorPanelPrefab);
        go.transform.parent = BagManager._instance.transform.parent;
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<SetErrorPanelText>().desText.text = "MP不足，请恢复MP";
    }

    public void IsSell(GameObject sellGo)
    {
        sellPanal.SetActive(true);
        sellPanal.transform.position = Input.mousePosition;
        sellGameObject = sellGo;
        //判断是否丢弃背包物体
    }

    public void OnSell()
    {
        sellPanal.SetActive(false);
        Destroy(sellGameObject);
        BagObjectMove bom = sellGameObject.GetComponent<BagObjectMove>();
        if (bom!=null)
        {
            PlayerStatus._instance.CoinChange(bom.info.price_sell);
        }
    }

    public void OnCancelSell()
    {
        sellPanal.SetActive(false);
        sellGameObject = null;
    }


    public void IsDiscardObject(GameObject ObjectGo)
    {
        discardObjectPanal.SetActive(true);
        discardObjectPanal.transform.position = Input.mousePosition;
        ObjectGameObject = ObjectGo;
        //判断是否丢弃背包物体
    }

    public void OnDiscardObject()
    {
        discardObjectPanal.SetActive(false);
        Destroy(ObjectGameObject);
    }

    public void OnCancelDiscardObject()
    {
        discardObjectPanal.SetActive(false);
        ObjectGameObject = null;
    }

    public void Die()
    {
        diePanel.SetActive(true);
        //血量少于0您已死亡
    }
    public void OnDiePanelOKButton()
    {//复活
        playerAction.PlayerLive();
        PlayerStatus._instance.HpChange(PlayerStatus._instance.HpMax);
        PlayerStatus._instance.MpChange(PlayerStatus._instance.MpMax);
        diePanel.SetActive(false);
    }


}
