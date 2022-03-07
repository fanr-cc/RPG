using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuffItem : MonoBehaviour
{

    public SkillInfo skillInfo;
    public Image bgImage;
    public Image fillImage;
    private int timeWait;
    private float cdTimer;
    private bool isCD = false;


    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {//技能冷却
            cdTimer += Time.deltaTime;
            fillImage.fillAmount = (timeWait - cdTimer) / timeWait;
            if (cdTimer >= timeWait)
            {
                Destroy(this.gameObject);
            }
        }
    }


    //设置技能图标及ID
    public void InitBuff(SkillInfo info)
    {
        skillInfo = info;
        bgImage.sprite = Resources.Load("bagObject\\" + info.icon_name, typeof(Sprite)) as Sprite;
        timeWait = info.timeEffect;
        isCD = true;
    }

    public void BuffAgain()
    {
        cdTimer = 0;
    } 

    void OnDestroy()
    {
        GameObject go = GameObject.FindGameObjectWithTag(TagsManager.player);
        if (go != null)
        {
            go.GetComponent<PlayerAction>().buffItemList.Remove(transform);
        }
        if (skillInfo.applyType==ApplyType.Buff)
        {
            switch (skillInfo.propertyType)
            {
                case PropertyType.Attack:
                    PlayerStatus._instance.AttackPropertyValueChange(0);
                    break;
                case PropertyType.Def:
                    PlayerStatus._instance.DefensePropertyValueChange(0);
                    break;
                case PropertyType.AttackSpeed:
                    PlayerStatus._instance.AttackSpeedPropertyValueChange(0);
                    break;
            }
        }
    }
}
