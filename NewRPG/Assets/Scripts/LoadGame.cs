using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
    

    public void OnNewGame()
    {
        SceneManager.LoadScene(1);
    }

}
