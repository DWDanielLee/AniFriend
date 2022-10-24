using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{



    public void GoToStart()
    {
        SceneManager.LoadScene("1Start");
    }
    
    public void GoToSelect()
    {
        
        SceneManager.LoadScene("2Select");
        
    }
    
    public void GoToLobby()
    {
        Scene curScene = SceneManager.GetActiveScene();
        if (curScene.name == "5Play")
        {
            BgmManager.Instance.StartMenuSceneMusic();
        }
        SceneManager.LoadScene("3Lobby");
    }
    
    public void GoToSetting()
    {
        SceneManager.LoadScene("4Setting");
    }
    
    public void GoToPlay()
    {
        
        SceneManager.LoadScene("5Play");
    }
}
