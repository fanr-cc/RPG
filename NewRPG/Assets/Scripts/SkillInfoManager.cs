using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillInfoManager : MonoBehaviour {
    public static SkillInfoManager _instance;

    //可直接指定文本文件，TextAsset是专门处理文本文件的类
    public TextAsset skillInfoText;
    //创建字典
    private Dictionary<int, SkillInfo> skillInfoDict = new Dictionary<int, SkillInfo>();

    void Awake()
    {
        _instance = this;
        ReadSkillInfoText();
    }

    //查找字典中信息
    public SkillInfo GetSkillInfoById(int id)
    {
        SkillInfo info = null;
        skillInfoDict.TryGetValue(id, out info);
        return info;
    }

    //读取Text文件
    void ReadSkillInfoText()
    {
        string text = skillInfoText.text;
        //将文本信息按行取到数组lineArray中。
        string[] lineArray = text.Split('\n');//按行分数据。
        //遍历数组lineArray
        foreach (string temp in lineArray)
        {
            //按逗号取出单个数据
            string[] valueArray = temp.Split(',');
            //创建类对象存储每条信息
            SkillInfo info = new SkillInfo();

            info.id = int.Parse(valueArray[0]);
            info.name = valueArray[1];
            info.icon_name = valueArray[2];
            info.skillDes= valueArray[3];
            info.propertyValue = float.Parse(valueArray[6]);
            info.timeEffect = int.Parse(valueArray[7]);
            info.mp = int.Parse(valueArray[8]);
            info.timeWait = int.Parse(valueArray[9]);
            info.skillLevel = int.Parse(valueArray[11]);
            info.skillDistance = float.Parse(valueArray[13]);
            info.efxName = valueArray[14];
            info.animName = valueArray[15];
            info.animTime = float.Parse(valueArray[16]);

 
            switch (valueArray[4])
            {
                case "Passive":
                    info.applyType = ApplyType.Passive;
                    break;
                case "Buff":
                    info.applyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    info.applyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    info.applyType = ApplyType.MultiTarget;
                    break;
            }
            switch (valueArray[5])
             {
                case "Attack":
                    info.propertyType = PropertyType.Attack;
                    break;
                case "Def":
                    info.propertyType = PropertyType.Def;
                    break;
                case "Speed":
                    info.propertyType = PropertyType.Speed;
                    break;
                case "AttackSpeed":
                    info.propertyType = PropertyType.AttackSpeed;
                    break;
                case "HP":
                    info.propertyType = PropertyType.Hp;
                    break;
                case "MP":
                    info.propertyType = PropertyType.Mp;
                    break;
            }
            switch (valueArray[10])
            {
                case "Magician":
                    info.heroType = HeroType.Magician;
                    break;
                case "Swordman":
                    info.heroType = HeroType.Swordman;
                    break;
            }
            switch (valueArray[12])
            {
                case "Self":
                    info.releaseType = ReleaseType.Self;
                    break;
                case "Enemy":
                    info.releaseType = ReleaseType.Enemy;
                    break;
                case "Position":
                    info.releaseType = ReleaseType.Position;
                    break;     
            }    
            //添加到字典
            skillInfoDict.Add(info.id, info);
        }
    }
}


public enum ApplyType { Passive, Buff, SingleTarget, MultiTarget }
public enum PropertyType { Attack, Def, Speed,AttackSpeed,Hp,Mp }
public enum ReleaseType { Self, Enemy, Position }

//用于存储单条信息内容
public class SkillInfo
{//MagicSphere_effect,AttackCritical,0.8
    public int id;
    public string name;
    public string icon_name;
    public string skillDes;
    public ApplyType applyType;
    public PropertyType propertyType;//作用类型，
    public float propertyValue;//作用值
    public int timeEffect;//技能持续时间
    public int mp;//魔法消耗
    public int timeWait;//技能冷却时间
    public HeroType heroType;
    public int skillLevel;
    public ReleaseType releaseType;//技能释放类型
    public float skillDistance;//技能释放距离
    public string efxName;//特效名称
    public string animName;//动画名称
    public float animTime;//动画时间
    
}
