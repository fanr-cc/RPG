using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{

    public static StatusManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowStatus = false;

    public Text attackText;
    public Text defenseText;
    public Text speedText;
    public Text point_RemainText;
    public Text summaryText;
    public Text summary_PlusText;

    public GameObject attack_AddButton;
    public GameObject defense_AddButton;
    public GameObject speed_AddButtonButton;
   
    void Awake()
    {
        _instance = this;
    }

    void Start () {
        UpdateStatusText();
        doTweenAudio = GetComponent<AudioSource>();
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置
    }

//Button按键增加属性
    public void OnAddAttack()
    {
        PlayerStatus._instance.AttackChange(1);
        PlayerStatus._instance.Point_RemainChange(-1);
    }
    public void OnAddDefense()
    {
        PlayerStatus._instance.DefenseChange(1);
        PlayerStatus._instance.Point_RemainChange(-1);
    }
    public void OnAddSpeed()
    {
        PlayerStatus._instance.SpeedChange(1);
        PlayerStatus._instance.Point_RemainChange(-1);
    }


    //修改Text组件的值
    public void UpdateStatusText()
    {
        attackText.text ="攻击："+ PlayerStatus._instance.Attack + "+" + PlayerStatus._instance.Attack_Plus;
        defenseText.text ="防御："+ PlayerStatus._instance.Defense + "+" + PlayerStatus._instance.Defense_Plus;
        speedText.text = "速度："+PlayerStatus._instance.Speed + "+" + PlayerStatus._instance.Speed_Plus;
        point_RemainText.text = "剩余点数："+PlayerStatus._instance.Point_Remain;
        summaryText.text = "总结：\n攻击：" + PlayerStatus._instance.AttackSum + "\n防御：" + PlayerStatus._instance.DefenseSum +
                           "\n速度：" + PlayerStatus._instance.SpeedSum;
        summary_PlusText.text = "\n攻击范围：" + PlayerStatus._instance.AttackRange + "\n攻击速度：" + PlayerStatus._instance.AttackSpeed +
                          "\n移动速度：" + PlayerStatus._instance.MoveSpeed;
    }

    public void ShowAddButton()
    {
        attack_AddButton.SetActive(true);
        defense_AddButton.SetActive(true);
        speed_AddButtonButton.SetActive(true);
    }

    public void HideAddButton()
    {
        attack_AddButton.SetActive(false);
        defense_AddButton.SetActive(false);
        speed_AddButtonButton.SetActive(false);
    }

    //显示状态面板
    public void ShowStatus()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowStatus = true;
    }
    //隐藏状态面板
    public void HideStatus()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowStatus = false;
    }

    //状态面板显示状态的改变
    public void OnStatusShowTransform()
    {
        if (isShowStatus)
        {
            HideStatus();
        }
        else
        {
            ShowStatus();
        }
    }
}
