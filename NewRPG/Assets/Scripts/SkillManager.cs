using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SkillManager : MonoBehaviour
{

    public static SkillManager _instance;
    private PlayerAction player;

    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowSkill = false;

    public RectTransform skillGroup;
    public GameObject skillItemPrefab;
    private SkillItem[] skillItemList;
    public SkillCd[] skillCdList;
    void Awake()
    {
        _instance = this;
        skillCdList = new SkillCd[6];
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
        doTweenAudio = GetComponent<AudioSource>();
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置
        InitSkill();
        UpdateSkillItem();
    }



    public void ReadySkillItemUse(int id)
    {
        bool isCD = false;
        foreach (SkillItem temp in skillItemList)
        {
            if (temp != null && temp.id == id)
            {
                isCD = temp.IsCD;
            }
        }
        if (!isCD)
        {
            //准备使用技能
            player.ReadySkillItemUse(id);
        }
        else
        {
            ErrorManager._instance.SkillCD();
        }
    }
    //使用技能时控制技能刷新
    public void SkillItemUse(int id)
    {
        foreach (SkillItem temp in skillItemList)
        {
            if (temp != null && temp.id == id)
            {
                temp.SkillItemCD();
            }
        }
        foreach (SkillCd temp in skillCdList)
        {
            if (temp != null && temp.id == id)
            {
                temp.SetSkillUse();
            }
        }
    }


    public void UpdateSkillItem()
    {
        foreach (SkillItem item in skillItemList)
        {
            item.UpdateSkillEnable();
        }
    }


    void InitSkill()
    {//初始化技能
        if (PlayerStatus._instance.HeroType == HeroType.Swordman)
        {
            for (int i = 4001; i < 4007; i++)
            {
                InstantiateskillItem(i);
            }
        }
        else
        {
            for (int i = 5001; i < 5007; i++)
            {
                InstantiateskillItem(i);
            }
        }       
        SetSkillGroupSize();
        skillItemList = GetComponentsInChildren<SkillItem>();
    }


    void InstantiateskillItem(int id)
    {//实例化技能
        GameObject go = Instantiate(skillItemPrefab);
        go.transform.parent = skillGroup;
        SkillItem itemCSharp = go.GetComponent<SkillItem>();
        itemCSharp.SetNewSkillById(id);
    }


    void SetSkillGroupSize()
    {//动态设置列表大小
        skillGroup.sizeDelta = new Vector2(370, 100 * skillGroup.childCount);//根据其孩子的个数，修改SkillGroup的大小
    }


    //显示设置面板
    public void ShowSkill()
        {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowSkill = true;
    }
    //隐藏设置面板
    public void HideSkill()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowSkill = false;
    }

    //设置面板显示状态的改变
    public void OnSkillShowTransform()
    {
        if (isShowSkill)
        {
            HideSkill();
        }
        else
        {
            ShowSkill();
        }
    }
}
