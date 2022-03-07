using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public enum PlayerActionState { Idle,Run,NormalAttack, SingleSkillAttack, MultiSkillAttack, PassiveSkill,BuffSkill, Death }
public class PlayerAction: MonoBehaviour
{
    public PlayerActionState playerState;
 //Animi
    private string Idle;
    private string Run;
    private string Attack1;
    private string Attack2;
    private string TakeDamage2;
    private string Death;


    //Run相关
    private LayerMask mask ;
    public GameObject effect_click_prefab;//点击特效
    private CharacterController playerController;
    private Animation anim;

    private float distance = 0;
    private bool isMoving = false;//用于记录是否移动
    private bool isInstan_effect_click_prefab=false;//用于记录是否实例化点击特效

    public Vector3 movePosition;//移动的目标位置

    //Attack相关
    private int rangeAttack;//保存伤害数值
    public Transform hitTransform;

    public float timerAttack;
    private float timerWait_Attack1 = 0.8f;
    private float timerWait_Attack2 = 0.9f;
    private float timerWait_Attack;
    private bool isAttack = false;
    //SkillAttack
   

    public bool IsUseSkill{ get { return isUseSkill; }}
    private bool isUseSkill = false ;
    private bool isSkillAttack = false;
    private SkillInfo skillInfo;
    private string skillEfxName;
    private string skillAnimationName;
    private float timerWait_Skill;
    private float timerSkill;
    private bool isSkill = false;
    //Buff
    private Transform parent;
    public GameObject buffItemPrefab;
    public List<Transform> buffItemList;
    //takeDamage
    private float timerTakeDamage;
    private float timerTakeDamageWait = 0.4f;

    public bool IsSkillAttack { get { return isSkillAttack; } }
    // Use this for initialization
    void Awake ()
    {
        playerController = GetComponent<CharacterController>();
        movePosition = transform.position;
        anim = GetComponent<Animation>();
        playerState = PlayerActionState.Idle;
        mask = ~(1 << 12);//Enemy活动区域层不检测
        parent = GameObject.Find("BuffIcon").transform;
    }

    void Start()
    {
        if (PlayerStatus._instance.HeroType == HeroType.Magician)
        {
            Idle = "Idle";
            Run = "Run";
            Attack1 = "Attack1";
            Attack2 = "Attack2";
            TakeDamage2 = "TakeDamage2";
            Death = "Death";
        }
        else
        {
            Idle = "Sword-Idle";
            Run = "Sword-Run";
            Attack1 = "Sword-Attack1";
            Attack2 = "Sword-Attack2";
            TakeDamage2 = "Sword-TakeDamage2";
            Death = "Sword-Death";
        }
    
    }
    // Update is called once per frame
    void Update () {

        switch (playerState)
        {
            case PlayerActionState.Idle:
                if (timerTakeDamage > 0)
                {//收到伤害，停顿以下
                    timerTakeDamage -= Time.deltaTime;
                }
                else {anim.CrossFade(Idle); }            
                break;
            case PlayerActionState.Run:
                if (timerTakeDamage > 0)
                { //收到伤害，停顿以下
                    timerTakeDamage -= Time.deltaTime;
                }
                else
                {
                    anim.CrossFade(Run);
                    MoveToPosition(movePosition);
                }
                PlayerStateRun();//点击地面，确定移动的位置
                break;
            case PlayerActionState.NormalAttack:
                NormalAttack();
                break;
            case PlayerActionState.SingleSkillAttack:
                SingleSkillAttack();
                break;
            case PlayerActionState.MultiSkillAttack:
                MultiSkillAttack();
                break;
            case PlayerActionState.BuffSkill:
                SkillItemUse(transform.position);
                break;
            case PlayerActionState.Death:
                anim.CrossFade(Death);
                break;
        }

        if (isSkillAttack&&Input.GetMouseButtonDown(1))
        {
            CancelSkill();
        }

        if (playerState!=PlayerActionState.Death&&Input.GetMouseButtonDown(0) && !PublicFunctionManager.Instance.IsPointerOverUIObject())
	    {
            //表示屏幕点击的Point转换成的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //表示是否碰撞物体（射线是否检测到目标，检测到的目标存储于hitInfor中）
            RaycastHit hitInfo;//记录射线检测目标

            Physics.Raycast(ray, out hitInfo,100, mask.value);//Ray表示射线，hitInfor表示碰撞信息

            if (!isSkillAttack)
            {//不是技能，正常行走
                if (hitInfo.collider.tag == TagsManager.ground)//射线检测到标签为地面的物体
                {
                    movePosition = hitInfo.point;
                    playerState = PlayerActionState.Run;
                    isUseSkill = false;
                }
                else if (hitInfo.collider.tag == TagsManager.enemy)
                {
                    timerAttack = PlayerStatus._instance.AttackTimeWait;
                    hitTransform = hitInfo.transform;
                    playerState = PlayerActionState.NormalAttack;
                    isUseSkill = false;
                }
            }
            else
            {//是技能时，点击各部件
                if (skillInfo.applyType == ApplyType.MultiTarget)
                {
                    if (hitInfo.collider.tag == TagsManager.ground|| hitInfo.collider.tag == TagsManager.enemy)
                    {
                        movePosition = hitInfo.point;
                        playerState = PlayerActionState.MultiSkillAttack;
                        CancelSkill();
                    }
                }
                else if (skillInfo.applyType == ApplyType.SingleTarget)
                {
                    if (hitInfo.collider.tag == TagsManager.enemy)
                    {
                        hitTransform = hitInfo.transform;
                        playerState = PlayerActionState.SingleSkillAttack;
                        CancelSkill();
                    }
                }
            }
        }
    }

    //移动
    void PlayerStateRun()
    {
        if (Input.GetMouseButton(0) && !PublicFunctionManager.Instance.IsPointerOverUIObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //表示是否碰撞物体（射线是否检测到目标，检测到的目标存储于hitInfor中）
            RaycastHit hitInfo;//记录射线检测目标
            Physics.Raycast(ray, out hitInfo, 100, mask.value);//Ray表示射线，hitInfor表示碰撞信息

            //实例化点击特效
            if (hitInfo.collider.tag == TagsManager.ground)//射线检测到标签为地面的物体
            {
                movePosition = hitInfo.point;
                if (!isInstan_effect_click_prefab)//是否实例化点击特效，实例化过就不执行
                {
                    Instantiate(effect_click_prefab, hitInfo.point + new Vector3(0, 0.3f, 0), Quaternion.identity);
                    isInstan_effect_click_prefab = true;
                    //0.5秒后，更改为未实例化点击特效。
                    //目的：0.5秒实例化一次特效。
                    Invoke("IsInstan_effect_click_prefabToFalse", 0.5f);
                }
            }
        }
    }
    //普通攻击
    void NormalAttack()
    {
        PublicFunctionManager.Instance.LookAtHorizontal(hitTransform.position,transform);
        if (hitTransform.transform.GetComponent<Wolf>().Hp == 0)
        {
            playerState = PlayerActionState.Idle;
        }
        else if ((hitTransform.position - transform.position).magnitude > PlayerStatus._instance.AttackRange)
        {
            anim.CrossFade(Run);
            playerController.SimpleMove(transform.forward * PlayerStatus._instance.MoveSpeed);
        }
        else
        {
            timerAttack += Time.deltaTime;//计算攻击间隔
            if (timerAttack >= timerWait_Attack)
            {
                if (isAttack)
                {
                    hitTransform.GetComponent<Wolf>().TakeDamage(rangeAttack);
                    rangeAttack = 0;
                    isAttack = false;
                    if (PlayerStatus._instance.HeroType == HeroType.Magician)
                    {
                        AudioManager._instance.OnPlayAudioSource("attackMagician");
                    }
                    else
                    {
                        AudioManager._instance.OnPlayAudioSource("attackSword");
                    }
                }
                anim.CrossFade(Idle);
            }
            if (timerAttack >= PlayerStatus._instance.AttackTimeWait)
            {
                if (Random.Range(0f, 1f) < 0.8)
                {
                    rangeAttack = Random.Range(PlayerStatus._instance.AttackSum - 5, PlayerStatus._instance.AttackSum);
                    anim.CrossFade(Attack1);
                    timerWait_Attack = timerWait_Attack1;
                    if (timerWait_Attack> PlayerStatus._instance.AttackTimeWait)
                    {
                        timerWait_Attack = PlayerStatus._instance.AttackTimeWait;
                    }
                }
                else
                {
                    rangeAttack = Random.Range(PlayerStatus._instance.AttackSum, PlayerStatus._instance.AttackSum + 5);
                    anim.CrossFade(Attack2);
                    timerWait_Attack = timerWait_Attack2;
                    if (timerWait_Attack > PlayerStatus._instance.AttackTimeWait)
                    {
                        timerWait_Attack = PlayerStatus._instance.AttackTimeWait;
                    }
                }
                timerAttack = 0;
                isAttack = true;
            }
            
        }
    }

    //单体技能
    void SingleSkillAttack()
    {
        if (hitTransform.transform.GetComponent<Wolf>().Hp == 0)
        {
            playerState = PlayerActionState.Idle;
        }
        else
        {
            if ((hitTransform.position - transform.position).magnitude > skillInfo.skillDistance)
            {
                PublicFunctionManager.Instance.LookAtHorizontal(hitTransform.position,transform);
                timerSkill = 0;
                anim.CrossFade(Run);
                playerController.SimpleMove(transform.forward * PlayerStatus._instance.MoveSpeed);
            }
            else
            {
                SkillItemUse(hitTransform.position);
            }
        }
    }


    //群体技能
    void MultiSkillAttack()
    {
        if ((movePosition - transform.position).magnitude > skillInfo.skillDistance)
        {
            PublicFunctionManager.Instance.LookAtHorizontal(movePosition,transform);
            timerSkill = 0;
            anim.CrossFade(Run);
            playerController.SimpleMove(transform.forward * PlayerStatus._instance.MoveSpeed);
        }
        else
        {
            SkillItemUse(movePosition);
        }
    }

    void SkillItemUse(Vector3 position)
    {
        PublicFunctionManager.Instance.LookAtHorizontal(position, transform);
        timerSkill += Time.deltaTime;
        if (timerSkill < timerWait_Skill)
        {
            if (!isUseSkill )
            {
                 isUseSkill = true;
            }
            anim.CrossFade(skillAnimationName);
        }
        else
        {
            timerSkill = 0;
            isUseSkill = false;
            bool isMpEnough = PlayerStatus._instance.IsMpEnough(skillInfo.mp);
            if (isMpEnough)
            {
                PlayerStatus._instance.MpChange(-skillInfo.mp);
                InstEfx(position);//实例化特效
                switch (skillInfo.applyType)
                {                               
                    case ApplyType.Passive:
                        AudioManager._instance.OnPlayAudioSource("Passive");
                        switch (skillInfo.propertyType)
                        { //增益魔法效果
                            case PropertyType.Hp:
                                PlayerStatus._instance.HpChange((int)skillInfo.propertyValue);
                                break;
                            case PropertyType.Mp:
                                PlayerStatus._instance.MpChange((int)skillInfo.propertyValue);
                                break;
                        }
                        break;
                    case ApplyType.Buff:
                        if (PlayerStatus._instance.HeroType == HeroType.Magician)
                        {
                            AudioManager._instance.OnPlayAudioSource("buffMagician");
                        }
                        else
                        {
                            AudioManager._instance.OnPlayAudioSource("buffSword");
                        }
                        bool isInitBuff = false;
                        foreach (Transform temp  in buffItemList)
                        {
                            if (temp.GetComponent<BuffItem>().skillInfo.id==skillInfo.id)
                            {
                                temp.GetComponent<BuffItem>().BuffAgain();
                                isInitBuff = true;
                            }
                        }
                        if (!isInitBuff)
                        {
                            switch (skillInfo.propertyType)
                            {
                                //Buff魔法效果
                                case PropertyType.Attack:
                                    PlayerStatus._instance.AttackPropertyValueChange(skillInfo.propertyValue) ;
                                    CreatBuffItem();
                                    break;
                                case PropertyType.Def:
                                    PlayerStatus._instance.DefensePropertyValueChange(skillInfo.propertyValue);
                                    CreatBuffItem();
                                    break;
                                case PropertyType.AttackSpeed:
                                    PlayerStatus._instance.AttackSpeedPropertyValueChange(skillInfo.propertyValue);
                                    CreatBuffItem();
                                    break;
                            }
                        }
                        break;
                    case ApplyType.SingleTarget:
                        AudioManager._instance.OnPlayAudioSource("singleSkill");
                        hitTransform.GetComponent<Wolf>().TakeDamage((int)(skillInfo.propertyValue * PlayerStatus._instance.AttackSum));
                        break;
                    case ApplyType.MultiTarget:
                        AudioManager._instance.OnPlayAudioSource("MultiSkill");
                        break;
                }
            }
            SkillManager._instance.SkillItemUse(skillInfo.id);
            playerState = PlayerActionState.Idle;
        }
     }

    void InstEfx(Vector3 position)
    {
        GameObject goPrafab = PublicFunctionManager.Instance.GetEfxByName(skillEfxName);
        GameObject go=Instantiate(goPrafab, position + Vector3.up, transform.rotation)as GameObject;
        MultiSkillCollider CSharp = go.GetComponent<MultiSkillCollider>();
        if (CSharp!=null)
        {
            CSharp.attack = (int)(PlayerStatus._instance.AttackSum*skillInfo.propertyValue);
        }
    }


    public void ReadySkillItemUse(int id)
    { 
        skillInfo = SkillInfoManager._instance.GetSkillInfoById(id);
        timerWait_Skill = skillInfo.animTime;
        skillAnimationName = skillInfo.animName;
        skillEfxName = skillInfo.efxName;

        bool isMpEnough = PlayerStatus._instance.IsMpEnough(skillInfo.mp);
        if (isMpEnough)
        {
            if (skillInfo.applyType == ApplyType.Buff || skillInfo.applyType == ApplyType.Passive)
            {
                playerState = PlayerActionState.BuffSkill;
            }
            else
            {
                isSkillAttack = true;
                CursorManager._instance.SetLockTarget();
            }
        }
    }


    void CancelSkill()
    {
        isSkillAttack = false;
        CursorManager._instance.SetNormal();
    }


    public void PlayerTakeDamage(int damage)
    {
        timerTakeDamage = timerTakeDamageWait;
        anim.CrossFade(TakeDamage2);
        PublicFunctionManager.Instance.SetTakeDamageText(damage, transform);//每位受到伤害，创建一个Text文本用于显示伤害；
    }

    public void PlayerDie()
    {
        playerState = PlayerActionState.Death;
    }
    public void PlayerLive()
    {
        transform.position = PlayerStatus._instance.playerPosition.position;
        playerState = PlayerActionState.Idle;
    }


    //更改为未实例化点击特效
    void IsInstan_effect_click_prefabToFalse()
	{
        isInstan_effect_click_prefab = false;
    }


    //移动到指定点
    void MoveToPosition(Vector3 position)
    {
        PublicFunctionManager.Instance.LookAtHorizontal(position,transform);
    //角色移动
        distance = DistanceFromPlayerToEndPosition(transform.position, position);
        if (distance > 0.5f)
        {
            //利用角色控制器组件移动
            playerController.SimpleMove(transform.forward * PlayerStatus._instance.MoveSpeed);
            timerSkill = 0;
            timerAttack = 0;
        }
        else
        {
            playerState = PlayerActionState.Idle;
        }
    }
    //计算俩三维坐标的X,Z平面上的距离
    float DistanceFromPlayerToEndPosition ( Vector3 v1, Vector3 v2)
    {
        Vector2 v11 = new Vector2(v1.x, v1.z);
        Vector2 v22 = new Vector2(v2.x, v2.z);
        float dis=Vector2.Distance(v11,v22);
        return dis;
    }

    
    void CreatBuffItem()
    {
        GameObject go = Instantiate(buffItemPrefab);
        go.transform.parent = parent;
        go.transform.localPosition = Vector2.zero;
        go.GetComponent<BuffItem>().InitBuff(skillInfo);
        buffItemList.Add(go.transform);
    }
}
