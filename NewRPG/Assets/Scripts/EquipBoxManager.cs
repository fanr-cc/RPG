using UnityEngine;
using System.Collections;

public class EquipBoxManager : MonoBehaviour {

    public static EquipBoxManager _instance;

    public AudioSource doTweenAudio;

    private RectTransform thisTransform;
    private Vector3 doTweenPosition;
    private bool isShowSetting = false;

    private PlayerAction playerAction;
    public AudioSource BgAudio;
    public Animation anima;
    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        doTweenAudio = GetComponent<AudioSource>();
        thisTransform = GetComponent<RectTransform>();
        doTweenPosition = thisTransform.position;//用于记录回放位置       

        playerAction = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
    }
    
}
