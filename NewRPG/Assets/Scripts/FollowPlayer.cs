using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{


    private Transform player;
    private Vector3 cameraToPlayer;//相机与角色的相对向量
    private float distance;//相机与角色的距离
    private float rotateSpeed = 5;
    private bool isRotate = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag(TagsManager.player).transform;
        transform.LookAt(player.position);
        cameraToPlayer = this.transform.position-player.position;
        distance = cameraToPlayer.magnitude;
    }

    void Update ()
    {//相机跟随
        //transform.LookAt(player.position);
        this.transform.position = player.position + cameraToPlayer;
        //视野拉近拉远
        ViewSizeChange();
        //视野旋转
        ViewRotate();
    }



    //改变视野大小方法（视野拉近拉远）
    void ViewSizeChange()
    {//只是更改其长度
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && cameraToPlayer.magnitude > 0.5f*distance)
        {//缩小视野    
            cameraToPlayer += 0.5f *cameraToPlayer.normalized* distance * Input.GetAxis("Mouse ScrollWheel");
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && cameraToPlayer.magnitude < 1.5f * distance)
        {//放大视野
            cameraToPlayer += 0.5f * cameraToPlayer.normalized * distance * Input.GetAxis("Mouse ScrollWheel");
        }
    }



    //改变视野角度（旋转视野）
    void ViewRotate()
    {//Input.GetAxis("Mouse X")//鼠标在X轴方向的滑动
     //Input.GetAxis("Mouse Y")//鼠标在Y轴方向的滑动

        if (Input.GetMouseButtonDown(1)&&!PublicFunctionManager.Instance.IsPointerOverUIObject())
        {//鼠标右键按下，视野开始旋转      
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))
        {//点击结束
            isRotate = false;
        }
        if (isRotate)
        { 
            transform.RotateAround(player.position,Vector3.up, rotateSpeed*Input.GetAxis("Mouse X"));

            if (Input.GetAxis("Mouse Y")>0&&transform.eulerAngles.x>15)
            {
                transform.RotateAround(player.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            }
            if (Input.GetAxis("Mouse Y") < 0 && transform.eulerAngles.x < 75)
            {
                transform.RotateAround(player.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            }
            cameraToPlayer = this.transform.position - player.position;
        }
      }
}
