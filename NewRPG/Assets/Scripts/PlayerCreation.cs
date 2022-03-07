using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerCreation : MonoBehaviour
{


    public GameObject[] playerPrefabs; //用于引用预制
    private GameObject[] playerGameObjects;//用于存储实例化的角色
    private int playerIndex=0;
    public InputField nameInput;
	// Use this for initialization
	void Start ()
	{
        //创建数组长度
	    playerGameObjects = new GameObject[playerPrefabs.Length];
        //实例化角色，并存储于playerGameObjects中，并将实例化的物体设置为不可见
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            playerGameObjects[i] =  GameObject.Instantiate(playerPrefabs[i], transform.position, transform.rotation) as GameObject;
            playerGameObjects[i].SetActive(false);

        }
        //将第一个角色设置为可见
        playerGameObjects[0].SetActive(true);
	}


//角色选择
    public void OnChangeChoose()
    {
        //循环选择，增加索引，将所有的角色设置为不可见，将索引角色设置为可见
        //也可以将上一个角色设置为不可见
        playerIndex++;
        if (playerIndex >= playerGameObjects.Length)
        {
            playerIndex = 0;
            //PlayerSetActiveToFalse();
            playerGameObjects[playerGameObjects.Length-1].SetActive(false);
            playerGameObjects[playerIndex].SetActive(true);
        }
        else
        {
            //PlayerSetActiveToFalse();
            playerGameObjects[playerIndex-1].SetActive(false);
            playerGameObjects[playerIndex].SetActive(true);
        }
    }

    public void OnOK()
    {
        PlayerPrefs.SetInt("PlayerIndex", playerIndex);//存储选择的角色
        PlayerPrefs.SetString("PlayerName", nameInput.text);//存储角色的名字
        SceneManager.LoadScene(2);
    }

    //将所有角色设置为不可见
    public void PlayerSetActiveToFalse()
    {
        for (int i = 0; i < playerGameObjects.Length; i++)
        {
            playerPrefabs[i].SetActive(false);
        }
    }
}
