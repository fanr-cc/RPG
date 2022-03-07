using UnityEngine;
using System.Collections;

public class ShortCut : MonoBehaviour
{  
    public KeyCode shortCutKey;
    public int i;//用于标志该快捷键
    private SkillCd skillCd=null;
    public GameObject skillShortCutPrefab;
    private PlayerAction player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.player).GetComponent<PlayerAction>();
    }

    void Update()
    {
        if (Input.GetKeyDown(shortCutKey))
        {
            UseShortCut_BagObjectOrSkill();
        }       
    }

    
   //在本物体下创建技能快捷方式
    public void CreatSkillItemShortCut(int id)
    {
        if (skillCd==null)
        {
            GameObject go = Instantiate(skillShortCutPrefab);
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector2.zero;
            skillCd = go.GetComponent<SkillCd>();//获取技能上的脚本，以便使用技能 
            SkillManager._instance.skillCdList[i] = skillCd;
        }
        skillCd.InitSkill(id);//初始化该快捷方式   
    }


    //检测快捷栏下是技能还是物品
    void UseShortCut_BagObjectOrSkill()
    {
        Transform[] go = GetComponentsInChildren<Transform>();

        foreach (Transform temp  in go)
        {
            if (temp.tag==TagsManager.skillShortCut && !player.IsUseSkill)
            {
                SkillManager._instance.ReadySkillItemUse(skillCd.id);
                return;
            }
            else if (temp.tag==TagsManager.bagObject)
            {   
                temp.GetComponent<BagObjectUse>().ObjectUse();
                return;
            }
        }
    }
}
