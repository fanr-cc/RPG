using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour
{
    public Camera miniMapCamera;
    private Transform player;
    private Vector3 miniMapCameraToPlayer;//相机与角色的相对向量
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.player).transform;
        miniMapCameraToPlayer = miniMapCamera.transform.position - player.position;
    }
    void Update()
    {//相机跟随
        miniMapCamera.transform.position = player.position + miniMapCameraToPlayer;
    }
    public void AddMiniMapClick()
    {
        if (miniMapCamera.orthographicSize > 4 )
        {
            miniMapCamera.orthographicSize -= 2;
        }
    }
    public void ReduceMiniMapClick()
    {
        if (miniMapCamera.orthographicSize < 9)
        {
            miniMapCamera.orthographicSize += 2;
        }
    }
}
