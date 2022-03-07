using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillCd : MonoBehaviour
{
    public Image bgImage;
    public Image fillImage;
    public int id;
    private int timeWait;
    private float cdTimer;
    private bool isCD=false;

	
	// Update is called once per frame
	void Update ()
	{
        if (isCD)
        {//技能冷却
            cdTimer += Time.deltaTime;
            fillImage.fillAmount = (timeWait - cdTimer) / timeWait;
            if (cdTimer >= timeWait)
            {
                fillImage.fillAmount = 0;
                cdTimer = 0;
                isCD = false;
            }
        }
    }

    //技能已使用标识
    public void SetSkillUse()
    {
        isCD = true;
    }

    //设置技能图标及ID
    public void InitSkill(int id)
    {
        this.id = id;
        SkillInfo info = SkillInfoManager._instance.GetSkillInfoById(id);
        bgImage.sprite =  Resources.Load("bagObject\\" + info.icon_name, typeof(Sprite)) as Sprite;
        timeWait = info.timeWait;
    }
}
