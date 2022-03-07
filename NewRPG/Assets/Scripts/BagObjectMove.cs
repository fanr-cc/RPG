using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagObjectMove : MonoBehaviour
{//保存新建的物体，并且控制背包物体的移动
    private Transform parent;//背包物体标志
    public Transform bagGrid=null;//记录原bagObject的父物体
    private bool isMoving_BagObject=false; //记录是否移动

    private Text countText; //显示物品数量

    public Image objectImage;//不知道为什么，预制体的Image组建不能通过GetComponent获取；
                              
    public int id = 0;//记录ID
    public ObjectInfo info;
    public int count = 1;//记录数量

  
    // Use this for initialization
    void Awake ()
    {
        parent = BagManager._instance.transform.parent; //设置父物体
        countText = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //鼠标按下
        {
            StartMoveObject(); //射线检测是否可以移动，并开始移动
        }
        if (Input.GetMouseButtonUp(0)) //鼠标抬起
        {
            if (isMoving_BagObject) //如果物品移动了，才执行
            {
                FinishMoveObject(); //射线检测放下的位置信息
            }
        }

        //移动过程，物品和鼠标位置保持一致
        if (isMoving_BagObject)
        {
            transform.position = Input.mousePosition;
        }
       }

    //根据ID设置本物体的ID，并更换对应图片
    public void SetNewObjectById(int id)
    {
        this.id = id;
        //通过ID查找物体信息
        info = ObjectInfoManager._instance.GetObjectInfoById(id);
        //在文件夹中查找物体图片
        objectImage.sprite = Resources.Load("bagObject\\"+ info.icon_name , typeof(Sprite))as Sprite;
    }

        //增加物品数量，并更新Text
    public void ChangeCount(int count)
    {
        this.count += count;
        countText.text = this.count.ToString();
    }


//拖放物体开始
    void StartMoveObject()
    {//射线检测
        List<RaycastResult> raycastObject = PublicFunctionManager.Instance.RaycastResultUIObject();
        //遍历检测结果，如果检测到鼠标点击自身，则物体开始移动
        foreach (RaycastResult temp in raycastObject)
        {
            if (temp.gameObject ==this.gameObject)
            {
                bagGrid = transform.parent;//记录原父物体信息

                transform.parent = parent;//将物体移动到高层物体中，使图层位于上方，不被别的物体遮盖

                transform.position = Input.mousePosition;

                isMoving_BagObject = true;
                break;
            }
        }
    }

    //拖放物体结束
    void FinishMoveObject()
    {
        bool isCanMoving_BagObject = true; //记录是否可以移动
        bool isSell_BagObject = false;
        List<RaycastResult> raycastObject = PublicFunctionManager.Instance.RaycastResultUIObject(); //存储射线检测到的目标       
        foreach (RaycastResult temp in raycastObject)
        {
            if (temp.gameObject.tag == TagsManager.bagObject && temp.gameObject != this.gameObject)
            {//如果碰到物体且不是自身，则交换
                isCanMoving_BagObject = false;//表示移动过了，不需要再移动了

                transform.parent = temp.gameObject.transform.parent;
                transform.localPosition = Vector2.zero;

                temp.gameObject.transform.parent = bagGrid;
                temp.gameObject.transform.localPosition = Vector2.zero;

                break;
            }
            else if (temp.gameObject.tag == TagsManager.bagGrid || temp.gameObject.tag == TagsManager.shortCutGrid) //如果碰到空格，则存储
            {
                isCanMoving_BagObject = false;  //表示移动过了，不需要再移动了

                transform.parent = temp.gameObject.transform;
                transform.localPosition = Vector2.zero;
                if (temp.gameObject.tag == TagsManager.shortCutGrid)
                {
                    SkillCd skillCd = temp.gameObject.GetComponentInChildren<SkillCd>();
                    if (skillCd!=null)
                    {
                        SkillManager._instance.skillCdList[temp.gameObject.GetComponent<ShortCut>().i]= null;
                        Destroy(skillCd.gameObject);
                    }
                }
                break;
            }
            else if (temp.gameObject.tag == TagsManager.storePanel)
            {
                isSell_BagObject = true;
            }
        }
      
        if (isCanMoving_BagObject)//如果没检测到物品或空格，原物品移动到原处
        {
            if (isSell_BagObject)
            {
                ErrorManager._instance.IsSell(gameObject);
            }
            else
            {
                ErrorManager._instance.IsDiscardObject(gameObject);
            }
            transform.parent = bagGrid;
            transform.localPosition = Vector2.zero;
        }

        //初始化
        isMoving_BagObject = false;
    }
}
