using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DrugStoreManager : MonoBehaviour {

    public static DrugStoreManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowDrugStore = false;

    public GameObject buyWindow;
    public InputField inputCount;
    public Text priceSumText;

    private int count;//购买数量
    private int price;//单价
    private int priceSum;//购买总金额
    private int id;//记录购买物品信息的ID

    void Awake()
    {
        _instance = this;
    }
 
    void Start () {
        doTweenAudio = GetComponent<AudioSource>();
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置
        buyWindow.SetActive(false);

    }

    //购买按键相关指令
    public void OnBuy1001(){Buy(1001);}
    public void OnBuy1002(){Buy(1002);}
    public void OnBuy1003(){Buy(1003);}

    public void Buy(int id)
    {
        //显示购买面板
        buyWindow.SetActive(true);
        buyWindow.transform.position = Input.mousePosition;
        //根据ID查找购买物品信息
        ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(id);
        this.id = id;
        price = info.price_buy;
        count = 1;
        inputCount.text = count.ToString();
        priceSumText.text = price.ToString();
    }


    //增加购买数量按键
    public void OnAddButtonClick()
    {
        count++;
        inputCount.text = count.ToString();
    }
    //更新购买总金额文本信息
    public void SetPriceSumText(string value)
    {
        if (value == ""){ count = 1;}
        else { count = int.Parse(value);}
        priceSum = count*price;
        priceSumText.text = priceSum.ToString();
    }

    public void OnOkButtonClick()
    {
        if (count>0)
        {
            bool isCoinEnough = PlayerStatus._instance.CoinChange(-priceSum);
            if (isCoinEnough) { BagManager._instance.GetObject(id,count);}
        }
       
        HideBuyWindow();
    }

    public void OnCancelButtonClick()
    {
        HideBuyWindow();
    }
    //隐藏购买窗口
    void HideBuyWindow()
    {
        buyWindow.SetActive(false);
    }



    //显示药品商店面板
    public void ShowDrugStore()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowDrugStore = true;
    }
    //隐藏药品商店面板
    public void HideDrugStore()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowDrugStore = false;
    }

    //药品商店面板显示状态的改变
    public void OnDrugStoreShowTransform()
    {
        if (isShowDrugStore)
        {
            HideDrugStore();
        }
        else
        {
            ShowDrugStore();
        }
    }
}
