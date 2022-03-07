using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {

    public static SettingManager _instance;
    private AudioSource doTweenAudio;
    private DOTweenAnimation doTweenAnimation;
    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowSetting = false;

    private PlayerAction playerAction;
    public AudioSource BgAudio;
    public Slider BgSlider;

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

        playerAction = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnSettingShowTransform();
        }
    }

    //显示设置面板
    public void ShowSetting()
    {
        doTweenAnimation.DORestart();
        doTweenAudio.Play();
        isShowSetting = true;
    }
    //隐藏设置面板
    public void HideSetting()
    {
        Tween tweener = thisTransform.DOMove(doTweenPosition, 0.5f);
        tweener.SetEase(Ease.InQuad);
        doTweenAudio.Play();
        isShowSetting = false;
    }

    //设置面板显示状态的改变
    public void OnSettingShowTransform()
    {
        if (isShowSetting)
        {
            HideSetting();
        }
        else
        {
            ShowSetting();
        }
    }

    public void OnResetButtonClick()
    {
        playerAction.PlayerLive();
        doTweenAudio.Play();
    }
    public void OnExitButtonClick()
    {
        doTweenAudio.Play();
        Application.Quit();
    }
    public void ChangeBgSlider()
    {
        BgAudio.volume=BgSlider.value;
    }
    
}
