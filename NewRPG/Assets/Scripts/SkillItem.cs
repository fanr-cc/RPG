using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{ 
    public int id;
    private SkillInfo info;
    private int level;
    public Image skillIcon;
    public Image skillIconBlack;
    public Text nameText;
    public Text desText;
    public Text mpText;
    public Text levelText;
    public Text applyText;

    private int timeWait;
    private float cdTimer;
    private bool isCD = false;

    public bool    IsCD{get{return isCD; }}
    void Update()
    {
        if (isCD)
        {//技能冷却
            cdTimer += Time.deltaTime;
            if (cdTimer >= timeWait)
            {
                cdTimer = 0;
                isCD = false;
            }
        }
    }


    public void SkillItemCD()
    {
        isCD = true;
    }

    public void UpdateSkillEnable()    
    {
        if (PlayerStatus._instance.Level >=level)
        {
            skillIconBlack.gameObject.SetActive(false);
            skillIcon.GetComponent<SkillItemIconMove>().enabled = true;
        }
        else
        {
            skillIconBlack.gameObject.SetActive(true);
            skillIcon.GetComponent<SkillItemIconMove>().enabled = false;
        }
    }


    public void SetNewSkillById(int id) //创建新技能，设置其图片
    {
        this.id = id;
        //通过ID查找物体信息    
        info = SkillInfoManager._instance.GetSkillInfoById(id);
        level = info.skillLevel;
        //在文件夹中查找物体图片
        skillIcon.sprite = Resources.Load("bagObject\\" + info.icon_name, typeof(Sprite)) as Sprite;
        timeWait = info.timeWait;

        nameText.text = info.name;
        desText.text = info.skillDes;
        mpText.text = "MP:"+info.mp;
        levelText.text = "LV:" + info.skillLevel;

        switch (info.applyType)
        {
            case ApplyType.Passive:
                applyText.text = "增益";
                break;
            case ApplyType.Buff:
                applyText.text = "BUFF";
                break;
            case ApplyType.SingleTarget:
                applyText.text = "单体攻击";
                break;
            case ApplyType.MultiTarget:
                applyText.text = "群体攻击";
                break;
        }
    }
}
