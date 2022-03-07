using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public enum WolfState { Idle, Walk, Attack, Death }
public enum WolfType { WolfBaby, WolfNormal, WolfBoss }

public class Wolf : MonoBehaviour
{

    public WolfType wolfType;
    private WolfState wolfStatus;
    private Transform player;
    private PlayerAction playerActionCSharp;
    public CharacterController wolfController;

    //动画相关///Init
    private Animation anim;
    private string Idle;
    private string Walk;
    private string Attack1 ;
    private string Attack2;
    private string TakeDamage1 ;
    private string TakeDamage2 ;
    private string Death;
    //Status
    public int Hp;//Init
    private int HpTotal;//Init

    //Idle
    private float timerNormal;
    private float timeNormalWait=2;//Init攻击速度
    private int moveSpeed = 3;

    //Attack
    private int attack;//Init
    private float attackRange;//Init
    private float timeWait;//Init攻击速度
  
    private float timerAttack;
    private float timerAttackWait;
    private float timerAttack1Wait = 0.6f;
    private float timerAttack2Wait = 0.7f;
    private bool isAttack = false;
    private int rangeAttack;
  
    //TakeDamage
    private Color colorNormal;
    private float timerTakeDamage;
    private float timerTakeDamageWait = 0.3f;
    //Die
    public GameObject boxPrefab;
    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animation>();
        wolfController = GetComponent<CharacterController>();
        wolfStatus = WolfState.Walk;
        timerAttack = timeWait;
    }



    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.player).transform;
        playerActionCSharp = player.GetComponent<PlayerAction>();
        colorNormal = GetComponentInChildren<Renderer>().material.color;


        switch (wolfType)
        {
            case WolfType.WolfBaby:
                Idle = "WolfBaby-Idle";
                Walk = "WolfBaby-Walk";
                Attack1 = "WolfBaby-Attack1";
                Attack2 = "WolfBaby-Attack2";
                TakeDamage1 = "WolfBaby-TakeDamage1";
                TakeDamage2 = "WolfBaby-TakeDamage2";
                Death = "WolfBaby-Death";

                HpTotal = 30;
                Hp = HpTotal;
                attack = 15;
                attackRange = 2f;
                timeWait = 2f;
                break;
            case WolfType.WolfNormal:
                Idle = "Wolf-Idle";
                Walk = "Wolf-Walk";
                Attack1 = "Wolf-Attack1";
                Attack2 = "Wolf-Attack2";
                TakeDamage1 = "Wolf-TakeDamage1";
                TakeDamage2 = "Wolf-TakeDamage2";
                Death = "Wolf-Death";

                HpTotal = 50;
                Hp = HpTotal;
                attack = 10;
                attackRange = 1.5f;
                timeWait = 1.7f;
                break;
            case WolfType.WolfBoss:
                Idle = "WolfBoss-Idle";
                Walk = "WolfBoss-Walk";
                Attack1 = "WolfBoss-Attack1";
                Attack2 = "WolfBoss-Attack2";
                TakeDamage1 = "WolfBoss-TakeDamage1";
                TakeDamage2 = "WolfBoss-TakeDamage2";
                Death = "WolfBoss-Death";

                HpTotal = 500;
                Hp = HpTotal;
                attack = 30;
                attackRange =2.5f;
                timeWait = 1.3f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (wolfStatus)
        {
            case WolfState.Idle:
                anim.CrossFade(Idle);
                IdleWalkTransform();
                break;
            case WolfState.Walk:
                anim.CrossFade(Walk);
                wolfController.SimpleMove(transform.forward * moveSpeed);
                IdleWalkTransform();
                break;
            case WolfState.Attack:
                WolfAttack();
                break;
            case WolfState.Death:
                anim.CrossFade(Death);
                break;
        }
    }

    //小狼巡逻状态切换
    void IdleWalkTransform()
    {
        timerNormal += Time.deltaTime;//计算移动间隔
        if (timerNormal >= timeNormalWait)
        {
            timerNormal = 0;
            if (Random.Range(0, 2) == 0)
            {
                wolfStatus = WolfState.Idle;
            }
            else
            {
                if (wolfStatus == WolfState.Idle)
                {
                    transform.Rotate(transform.up * Random.Range(0, 360));
                }
                wolfStatus = WolfState.Walk;
            }
        }
    }

    //小狼攻击
    void WolfAttack()
    {
        PublicFunctionManager.Instance.LookAtHorizontal(player.position,transform);
        if (PlayerStatus._instance.Hp==0)
        {
            wolfStatus = WolfState.Idle;
            return;
        }
        if (timerTakeDamage > 0)
        {//收到伤害，停顿以下
            timerTakeDamage -= Time.deltaTime;
        }
        else
        {
            if ((player.position - transform.position).magnitude > attackRange)
            {
                anim.CrossFade(Walk);
                wolfController.SimpleMove(transform.forward * moveSpeed);
            }
            else
            {
                timerAttack += Time.deltaTime;//计算攻击间隔
                if (timerAttack >= timeWait)
                {
                    if (Random.Range(0f, 1f) < 0.8)
                    {
                        rangeAttack = Random.Range(attack - 2, attack);
                        anim.CrossFade(Attack1);
                        timerAttackWait = timerAttack1Wait;
                    }
                    else
                    {
                        rangeAttack = Random.Range(attack, attack + 3);
                        anim.CrossFade(Attack2);
                        timerAttackWait = timerAttack2Wait;
                    }
                    timerAttack = 0;
                    isAttack = true;
                }
                else if (timerAttack >= timerAttackWait)
                {
                    if (isAttack)
                    {
                        PlayerStatus._instance.TakeDamage(rangeAttack);
                        isAttack = false;
                    }
                    anim.CrossFade(Idle);
                }
            }
        }
    }

    //小狼受到伤害
    public void TakeDamage(int damage)
    {
        if (wolfStatus == WolfState.Death)
        {
            return;
        }
        else if (wolfStatus == WolfState.Idle || wolfStatus == WolfState.Walk)
        {
            wolfStatus = WolfState.Attack;
            moveSpeed += 2;
        }

        Hp -= damage;
        anim.CrossFade(Random.Range(0f, 1f) < 0.8f ? TakeDamage1 : TakeDamage2);
        StartCoroutine(TakeDamageColorTransform());//变色，表示被击中
        PublicFunctionManager.Instance.SetTakeDamageText(damage, transform);//每位受到伤害，创建一个Text文本用于显示伤害；
        timerTakeDamage = timerTakeDamageWait;//被几种有延迟

        if (Hp <= 0)
        {
            Hp = 0;
            wolfStatus = WolfState.Death;
            WolfDie();
        }
    }

 //控制小狼收到打击时，自身颜色变化
    IEnumerator TakeDamageColorTransform()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        GetComponentInChildren<Renderer>().material.color = colorNormal;
    }

    void WolfDie()
    {
        switch (wolfType)
        {
            case WolfType.WolfBaby:
                EnemyManager._instance.wolfBabyCountChange();
                PlayerStatus._instance.KillWolfBaby();
                break;
            case WolfType.WolfNormal:
                EnemyManager._instance.wolfNormalCountChange();
                PlayerStatus._instance.KillWolfNormal();    
                break;
            case WolfType.WolfBoss:
                EnemyManager._instance.wolfBossCountChange();
                PlayerStatus._instance.KillWolfBoss();
                Instantiate(boxPrefab, transform.position,Quaternion.identity);
                break;
        }
        Destroy(gameObject, 2f);
    }

    //碰到空气墙，小狼回头，控制小狼活动范围
    void OnTriggerEnter(Collider collider)//移动过程中碰到空气墙，移动方向翻转
    {
        if (collider.tag == TagsManager.enemyArea && wolfStatus == WolfState.Walk)
        {
            transform.Rotate(transform.up * 180);
        }
    }
 
    void OnMouseEnter()
    {//改变鼠标图标
        if (!playerActionCSharp.IsSkillAttack)
        {
            CursorManager._instance.SetAttack();
        }
    }
    void OnMouseExit()
    {//改变鼠标图标
        if (!playerActionCSharp.IsSkillAttack)
        {
            CursorManager._instance.SetNormal();
        }
    }

}

