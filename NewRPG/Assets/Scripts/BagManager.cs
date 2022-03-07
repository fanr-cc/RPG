using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{//该类用于背包的显示隐藏、拾取物品、更新物品信息

    public static BagManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowBag = false;

    public Text coinText;//金币面板
    public List<Transform> bagGridList;
    public List<Transform> ShortCutList;

    public GameObject bagObjectPrefab;//用于实例化物品
   

    //以下变量，用于显示背包中物品信息
    public GameObject objectDes;//显示物品信息
    private Text desText; //显示物品信息文本
    private bool isShowObjectDes = false;

   
    void Awake()
    {
        _instance = this;


    }

    void Start()
    {
        doTweenAudio = GetComponent<AudioSource>();
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置

        desText = objectDes.GetComponentInChildren<Text>();
        objectDes.SetActive(false);

    }

    void Update()
    {
        if (isShowObjectDes)//如果显示物品信息，则物品信息列表和鼠标位置保持一致
        {
             objectDes.transform.position = Input.mousePosition;
        }
    }

    //将物体添加到物品栏//（按理说，应该在字典中查找ID信息，若字典中有该ID信息，则初始化）
    public void GetObject(int id,int count,int listnumber=1)
    {
        if (count<0)
        {
            return;
        }

        List<Transform> list = new List<Transform>();
        ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(id);
        bool isFindObject = false;//用于记录是否找到该物体
        BagObjectMove bagObjectCSharp = null;//用于存储查找到的物品的脚本
        bool isBagFull = true;

        switch (listnumber)
        {
            case 1://正常拾取装备，将装备放置在列表和快捷栏里
                if (info.type == ObjectType.Drug)
                {
                    list.AddRange(ShortCutList);
                    list.AddRange(bagGridList);
                }
                else
                {
                    list.AddRange(bagGridList);
                    list.AddRange(ShortCutList);
                }

                foreach (Transform temp in ShortCutList)
                {
                    SkillCd skillCdCSharp = temp.GetComponentInChildren<SkillCd>();
                    if (skillCdCSharp != null)
                    {
                        list.Remove(temp);
                    }
                }
                break;
            case 2://在快捷栏的物品上放置技能时，将物品放置在背包里
                list.AddRange(bagGridList);
                break;
        }
        
        //查找背包里是否有该物体
        foreach (Transform temp in list)
        {
            bagObjectCSharp = temp.GetComponentInChildren<BagObjectMove>();//获取子物体的BagObject脚本
          
            if ( bagObjectCSharp!=null&& bagObjectCSharp.id == id)
            { //如果脚本不为空，且查找到了该物体
                isFindObject = true;//表示找到了该物体
                 break;//直接返回，这时，bagObjectCSharp就是查找到的物体的BagObject脚本。   
            }   
        }

        if (isFindObject)//如果有该ID物体，将该物体的数量加count
        {
            bagObjectCSharp.ChangeCount(count);
        }
        else//如果没有该ID物体，在新的空格内放置
        {
            foreach (Transform temp in list)//查找背包
            {
                bagObjectCSharp = temp.GetComponentInChildren<BagObjectMove>();//获取子物体的BagObject脚本
                if (bagObjectCSharp == null)//如果有格子，则新建物品
                {
                    GameObject go = GameObject.Instantiate(bagObjectPrefab);//创建物品
                    go.transform.parent = temp;//父亲设置为查找到的空格
                    go.transform.localPosition = Vector2.zero;//物品的坐标归0
                    bagObjectCSharp= go.GetComponent<BagObjectMove>();//获取物体上的BagObject脚本
                    bagObjectCSharp.SetNewObjectById(id);    //给物品发送消息，初始化物品属性
                    bagObjectCSharp.ChangeCount(count-1);
                    isBagFull = false;
                    break;
                }
            }
            if (isBagFull)
            {
                ErrorManager._instance.BagFull();//背包满了，提示背包已满
            }
         }  
    }

    public void UpdateCoin()
    {
        coinText.text = PlayerStatus._instance.Coin.ToString();
    }


    //显示背包面板
    public void ShowBag()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowBag = true;
    }
    //隐藏背包面板
    public void HideBag()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowBag = false;
    }

    //背包面板显示状态的改变
    public void OnBagShowTransform()
    {
        if (isShowBag)
        {
            HideBag();
        }
        else
        {
            ShowBag();
        }
    }


    //显示物品信息
    public void SetDesText(int id)
    {
        if ( !isShowObjectDes)//如果没人使用物品信息组建，则开始显示物品信息
        {
            isShowObjectDes = true;  //标志位：正在使用物品信息
            objectDes.SetActive(true);//显示物品信息组建
            objectDes.transform.position = Input.mousePosition;//更新物品信息组建位置

            //更新物品信息内容
            string str = "\n物品信息：\n";
            ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(id);
            str += "名称：" + info.name + "\n";

            switch (info.type)
            {
                case ObjectType.Drug:
                    str += "类型：药品\n";
                    str += "HP：+" + info.hp + "\n";
                    str += "MP：+" + info.mp + "\n";
                    str += "买入价：" + info.price_buy + "\n";
                    str += "出售价：" + info.price_sell;
                    break;
                case ObjectType.Equip:
                    str += "类型：装备\n";
                    switch (info.careerType)
                    {
                        case CareerType.Magician:
                            str += "使用职业：Magician\n";
                            break;
                        case CareerType.Swordman:
                            str += "使用职业：Swordman\n";
                            break;
                        case CareerType.Common:
                            str += "使用职业：Common\n";
                            break;
                    }
                    switch (info.equipType)
                    {
                        case EquipType.Headgear:
                            str += "装备部位：Headgear\n";
                            break;
                        case EquipType.Armor:
                            str += "装备部位：Armor\n";
                            break;
                        case EquipType.RightHand:
                            str += "装备部位：RightHand\n";
                            break;
                        case EquipType.LeftHand:
                            str += "装备部位：LeftHand\n";
                            break;
                        case EquipType.Shoe:
                            str += "装备部位：Shoe\n";
                            break;
                        case EquipType.Accessory:
                            str += "装备部位：Accessory\n";
                            break;
                    }              
                    str += "攻击：+" + info.attack + "\n";
                    str += "防御：+" + info.defense + "\n";
                    str += "速度：+" + info.speed + "\n";
                    str += "买入价：" + info.price_buy + "\n";
                    str += "出售价：" + info.price_sell;
                    break;
                case ObjectType.Mat:
                    str += "类型：材料\n";
                    str += "出售价：" + info.price_sell;
                    break;
            }
            desText.text = str;
        }       
    }

    //将物品信息BagObjectDes设置为隐藏
    public void SetDesFalse()
    {
        if (isShowObjectDes)//如果信息正在使用，使用信息的物品发送不使用消息时，设置物品为未使用
        {
            isShowObjectDes = false;
            objectDes.SetActive(false);
        }
    }

}

