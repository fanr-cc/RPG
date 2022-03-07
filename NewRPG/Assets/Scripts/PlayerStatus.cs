using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HeroType { Magician, Swordman }

public class PlayerStatus : MonoBehaviour
{//记录角色状态的各种信息，除了背包物品。
    public static PlayerStatus _instance;
    private PlayerAction playerAction;
    private HeroType heroType;
    public GameObject magicianPrefab;
    public GameObject swordmanPrefab;
    public Transform playerPosition;
    private string playerName = "丧心病狂小三爷";
    private List<ObjectInfo> equipList=new List<ObjectInfo>();

    private int coin = 1000;//金币数量
    private int killCount;
    private int level = 10;
    private int exp;
    private int expMax = 190;
    private int hp = 100;
    private int hpMax=100;
    private int mp = 100;
    private int mpMax = 100;

    private int point_Remain;
    private int attack = 10;
    private int defense = 2;
    private int speed;
    private int attack_Plus;
    private int defense_Plus;
    private int speed_Plus;
    
    private int attack_Range;
  


    public HeroType HeroType { get { return heroType; } }
    public string PlayerName { get { return playerName; } }


    public int Coin { get { return coin; } }
    public int KillCount { get { return killCount; } }
    public int Level { get { return level; } }
    public int Exp { get { return exp; } }
    public int ExpMax { get { return expMax; } }
    public int Hp { get { return hp; } }
    public int HpMax{get { return hpMax; }}
    public int Mp { get { return mp; } }
    public int MpMax{get { return mpMax; } }
 

    public int Point_Remain { get { return point_Remain; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int Speed { get { return speed; } }
    public int Attack_Plus { get { return attack_Plus; } }
    public int Defense_Plus { get { return defense_Plus; } }
    public int Speed_Plus { get { return speed_Plus; } }
    public int AttackSum { get { return (int)((attack + attack_Plus)* (AttackPropertyValue+1)); } }
    public int DefenseSum { get { return (int)((defense + defense_Plus)*(DefensePropertyValue+1)); } }
    public int SpeedSum { get { return speed+speed_Plus; } }


    public float AttackSpeed { get {
        if (((SpeedSum * 0.02f + 1) * (AttackSpeedPropertyValue + 1))>5)return 5;
        return ((SpeedSum*0.02f+1) * (AttackSpeedPropertyValue+1)); } }
    public float MoveSpeed { get {
        if (SpeedSum * 0.02f + 4>8)return 8;
        return (SpeedSum * 0.02f + 4);} }
    public int AttackRange { get { return attack_Range; } }

    public float AttackTimeWait { get { return 2/AttackSpeed; } }


    private float AttackPropertyValue { get; set; }
    private float DefensePropertyValue { get; set; }
    private float AttackSpeedPropertyValue { get; set; }

    void Awake()
    {
        _instance = this;
        int playerNumber = PlayerPrefs.GetInt("PlayerIndex");
        playerName = PlayerPrefs.GetString("PlayerName");
        if (playerNumber==0)
        {
            heroType = HeroType.Magician;
            attack_Range = 6;
            Instantiate(magicianPrefab, playerPosition.position, Quaternion.identity);
        }
        else
        { heroType = HeroType.Swordman;
            attack_Range = 3;
            Instantiate(swordmanPrefab, playerPosition.position, Quaternion.identity);
        }
    }

    void Start()
    {
        playerAction = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
    }


//任务数量变化
    public void KillCountChange(int count)
    {
        if (QuestManager._instance.IsQuest)
        {
            killCount += count;
            QuestManager._instance.UpdateQuestDes();
        }
    }

    //杀死小狼
    public void KillWolfBaby()
    {
        //小狼增加金钱，经验，爆装备
        int wolfBabyCoin = 50;
        int wolfBabyExp = 20;
        CoinChange(Random.Range(wolfBabyCoin - 5, wolfBabyCoin + 5) );
        ExpChange(wolfBabyExp);

        KillCountChange(1);
    }

    //杀死正常狼
    public void KillWolfNormal()
    {
        int wolfNormalCoin = 100;
        int wolfNormalExp = 30;
        CoinChange(Random.Range(wolfNormalCoin - 5, wolfNormalCoin + 5));
        ExpChange(wolfNormalExp);

        KillCountChange(2);
    }

    //杀死Boss
    public void KillWolfBoss()
    {
        int wolfBossCoin = 500;
        int wolfBossExp = 200;
        CoinChange(Random.Range(wolfBossCoin - 5, wolfBossCoin + 5));
        ExpChange(wolfBossExp);

        KillCountChange(5);
    }

    //穿上装备
    public void AddEquipList(int id)
    {
       ObjectInfo info= ObjectInfoManager._instance.GetObjectInfoById(id);
       equipList.Add(info);
    }
    //卸下装备
    public void RemoveEquipList(int id)
    {
        ObjectInfo info = ObjectInfoManager._instance.GetObjectInfoById(id);
        equipList.Remove(info);
    }

    //更换装备引起的属性变化
    public void EquipChange()//原部位没有装备，穿上装备
    {
        int equipAttack = 0;
        int equipDef = 0;
        int equipSpeed = 0;

        foreach (ObjectInfo info  in equipList)
        {
            equipAttack += info.attack;
            equipDef += info.defense;
            equipSpeed += info.speed;
        }
        attack_Plus=equipAttack ;
        defense_Plus = equipDef;
        speed_Plus = equipSpeed;
        StatusManager._instance.UpdateStatusText();
    }

    //金币变化，买东西，杀狼，完成任务，卖东西
    public bool CoinChange(int count)
    {
        if ( coin+count<0)
        {
            ErrorManager._instance.CoinLack(coin, -count);//提示缺少金币
            return false;
        }
        else
        {
            coin += count;
            BagManager._instance.UpdateCoin();
            return true;
        }
    }
    //3杀怪，1任务引起的经验变化
    public void ExpChange(int count)
    {
        exp += count;
        while (exp >= expMax)
        {
            exp -= expMax;
            Levelup();
        }
        HeadManager._instance.ChangeExpSlider();
    }
    //升级，经验够了，就升级
    public void Levelup()
    {
        level ++;
        expMax = 90 + level*10;
        Point_RemainChange(5);
        if (heroType==HeroType.Magician)
        {
            HpMpMaxChange(10,20);
        }
        else
        {
            HpMpMaxChange(20,10);
        }
        HpChange(hpMax);
        MpChange(mpMax);

        SkillManager._instance.UpdateSkillItem();
        HeadManager._instance.ChangeLevel();
        HeadManager._instance.ChangeSliderMaxValue();
    }

    //收到伤害，减少血量
    public void TakeDamage(int damage)
    {
        int dam = damage*(200 - DefenseSum)/(200 + DefenseSum);//伤害系数
        if (dam < 1) dam = 1;
        HpChange(-dam);
        playerAction.PlayerTakeDamage(dam);
    }
    //血量变化
    public void HpChange(int count)
    {
        if (hp + count < 0)
        {
            hp = 0;
            playerAction.PlayerDie();
            ErrorManager._instance.Die();
        }
        else if (hp + count>hpMax)
        {
            hp = hpMax;
        }
        else
        {
            hp += count;
        }
        HeadManager._instance.ChangHpSlider();
    }
    //判断蓝是不是够
    public bool IsMpEnough(int count)
    {
        if (mp - count < 0)
        {
            ErrorManager._instance.MpLack();
            return false;
            //魔法不足
        }
         return true;     
    }
    //改变蓝量
    public void MpChange(int count)
    {
        if (mp + count < 0)
        {
            ErrorManager._instance.MpLack();
            //魔法不足
        }
        else if (mp + count > mpMax)
        {
            mp = mpMax;
        }
        else
        {
            mp += count;
        }
        HeadManager._instance.ChangMpSlider();
    }
    //升级带来的血量增加
    public void HpMpMaxChange(int hpCount,int mpCount)
    {
        hpMax += hpCount;
        mpMax += mpCount;
    }

    //技能点带来的属性加成
    public void AttackChange(int count)
    {
        attack += count;
        StatusManager._instance.UpdateStatusText();
    }
    public void DefenseChange(int count)
    {
        defense += count;
        StatusManager._instance.UpdateStatusText();
    }
    public void SpeedChange(int count)
    {
        speed += count;
        StatusManager._instance.UpdateStatusText();
    }


    //剩余点数变化
    public void Point_RemainChange(int count)
    {
        point_Remain += count;     
        StatusManager._instance.UpdateStatusText();
 
        if (point_Remain>0)
        {
            StatusManager._instance.ShowAddButton();
        }
        else
        {
            StatusManager._instance.HideAddButton();
        }

        if (count > 0)
        {
            StatusManager._instance.ShowStatus();
        }
    }


    public void AttackPropertyValueChange(float num)
    {
        AttackPropertyValue = num;
        StatusManager._instance.UpdateStatusText();
    }

    public void DefensePropertyValueChange(float num)
    {
        DefensePropertyValue = num;
        StatusManager._instance.UpdateStatusText();
    }

    public void AttackSpeedPropertyValueChange(float num)
    {
        AttackSpeedPropertyValue = num;
        StatusManager._instance.UpdateStatusText();
    }

}
