using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;


    public GameObject acceptButton;
    public GameObject cancelButton;
    public GameObject okButton;
    public Text des;
    private bool isQuest = false;
    private bool isShowQuest = false;
    public bool IsQuest { get { return isQuest; } }

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
        StartQuestDes();    
    }

  
    //接受任务按钮
    public void OnAcceptQuest()
    {
        isQuest = true;
        ShowAcceptDes(); //修改任务描述
    }


    //完成任务OK按钮
    public void OnOKQuest()
    {
        if (PlayerStatus._instance.KillCount >= 10)
        {
            //向PlayerStatus发送消息，表示完成任务了。
            PlayerStatus._instance.CoinChange(1000);
            PlayerStatus._instance.ExpChange(500);
            PlayerStatus._instance.KillCountChange(-10);
            isQuest = false;
            ShowOKDes(); //修改任务描述
        }
        else
        {
            HideQuest();
        }
    }

    //点击Accept之后的任务描述
    void ShowAcceptDes()
    {
        acceptButton.SetActive(false);
        cancelButton.SetActive(false);
        okButton.SetActive(true);
        UpdateQuestDes();
    }

    //点击OK之后的任务描述
    void ShowOKDes()
    {
        acceptButton.SetActive(true);
        cancelButton.SetActive(true);
        okButton.SetActive(false);
        UpdateQuestDes();
    }

    //任务面板隐藏显示转换
    public void OnQuestShowTransform()
    {
        if (isShowQuest)
        {
            HideQuest();
        }
        else
        {
            ShowQuest();
        }
    }

    //显示任务面板
    public void ShowQuest()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowQuest = true;
    }

    //点击隐藏任务面板
    public void HideQuest()
    {    
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowQuest = false;
    }

    //更新任务信息
    public void UpdateQuestDes()
    {
        if (isQuest)
        {
            des.text = "任务：\n请帮助村民消灭10只小野狼，野狼就在桥的对面。\n\n\n进度：\n目前已消灭" + PlayerStatus._instance.KillCount +
                       "/10只野狼。\n\n奖励：\n1000金币\n500经验";
        }
        else
        {
            des.text = "任务：\n最近村子被小野狼袭击，请帮助村民消灭10只野狼，野狼就在桥的对面。\n\n\n目标：\n消灭10只野狼。\n\n奖励：\n1000金币\n500经验";
        }
    }

    void StartQuestDes()
    {
        if (isQuest)
        {
            ShowAcceptDes();
        }
        else
        {
            ShowOKDes();
        }
        UpdateQuestDes();
    }
}

