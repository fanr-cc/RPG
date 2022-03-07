using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadManager : MonoBehaviour
{

    public static HeadManager _instance;
    public Material SwordmanMaterial;
    public Material MagicianMaterial;

    public Text nameText;
    public Text levelText;
    public Slider HpSlider;
    public Slider MpSlider;
    public Slider ExpSlider;
    public Text HpPercentText;
    public Text MpPercentText;
    public Text ExpPercentText;


    void Awake()
    {
        _instance = this;

    }
	// Use this for initialization
	void Start ()
	{
	    InitName();
	    ChangeLevel();
	    ChangeSliderMaxValue();
        ChangHpSlider();
	    ChangMpSlider();
        ChangeExpSlider();

	}


    public void ChangeSliderMaxValue()
    {
        HpSlider.maxValue = PlayerStatus._instance.HpMax;
        MpSlider.maxValue = PlayerStatus._instance.MpMax;
        ExpSlider.maxValue = PlayerStatus._instance.ExpMax;
        ChangHpSlider();
        ChangMpSlider();
        ChangeExpSlider();
    }
    public void ChangHpSlider()
    {
        HpSlider.value= PlayerStatus._instance.Hp;
        HpPercentText.text= PlayerStatus._instance.Hp +"/"+ PlayerStatus._instance.HpMax;
    }
    public void ChangMpSlider()
    {
        MpSlider.value = PlayerStatus._instance.Mp;
        MpPercentText.text = PlayerStatus._instance.Mp+"/"+PlayerStatus._instance.MpMax;
    }

    public void ChangeExpSlider()
    {
       ExpSlider.value = PlayerStatus._instance.Exp;
       ExpPercentText.text = (PlayerStatus._instance.Exp +"/"+ PlayerStatus._instance.ExpMax);
    }
    public void ChangeLevel()
    {
        levelText.text = "LV:" + PlayerStatus._instance.Level;
    }

     void InitName()
    {
        nameText.text = PlayerStatus._instance.PlayerName;
        if (PlayerStatus._instance.HeroType == HeroType.Swordman)
        {
            GetComponent<Image>().material = SwordmanMaterial;
        }
        else
        {
            GetComponent<Image>().material = MagicianMaterial;
        }
    }
}
