using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EquipManager : MonoBehaviour {

    public static EquipManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowEquip = false;

    public Transform headgear;
    public Transform armor;
    public Transform rightHand;
    public Transform leftHand;
    public Transform shoe;
    public Transform accessory;


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
    }

   

    //显示装备面板
    public void ShowEquip()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowEquip = true;
    }
    //隐藏装备面板
    public void HideEquip()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowEquip = false;
    }

    //装备面板显示状态的改变
    public void OnEquipShowTransform()
    {
        if (isShowEquip)
        {
            HideEquip();
        }
        else
        {
            ShowEquip();
        }
    }
}
