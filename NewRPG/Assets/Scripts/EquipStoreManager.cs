using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EquipStoreManager : MonoBehaviour {

    public static EquipStoreManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowEquipStore = false;

    public  GameObject equipStoreItemPrefab;
    public RectTransform equipStoreGroup;

    void Awake()
    {
        _instance = this;
    }

    void Start () {
        doTweenAudio = GetComponent<AudioSource>();
        doTweenAnimation = GetComponent<DOTweenAnimation>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置

        InitEquipStore();
    }



    public void InitEquipStore()
    {
        for (int i = 2001; i < 2023; i++)
        {
            InstantiateEquipStoreItem(i);
        }
        SetSkillGroupSize();
    }

    void InstantiateEquipStoreItem(int id)
    {
        GameObject go = Instantiate(equipStoreItemPrefab);
        go.transform.parent = equipStoreGroup;
        EquipStoreItem itemCSharp = go.GetComponent<EquipStoreItem>();
        itemCSharp.SetNewEquipStoreItemById(id);
    }

    void SetSkillGroupSize()
    {//动态设置列表大小
        equipStoreGroup.sizeDelta = new Vector2(370, 100 * equipStoreGroup.childCount);//根据其孩子的个数，修改SkillGroup的大小
    }

    //显示武器商店面板
    public void ShowEquipStore()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowEquipStore = true;
    }
    //隐藏武器商店面板
    public void HideEquipStore()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowEquipStore = false;
    }

    //武器商店面板显示状态的改变
    public void OnEquipStoreShowTransform()
    {
        if (isShowEquipStore)
        {
            HideEquipStore();
        }
        else
        {
            ShowEquipStore();
        }
    }
}
