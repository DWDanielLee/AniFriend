using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private BgmManager bgm;
    private Scene scene;

    private void Awake()
    {
        scene = SceneManager.GetActiveScene();
        bgm = FindObjectOfType<BgmManager>();
        if (scene.name == "5Play")
        {
            Debug.Log("dd");
            //Destroy(bgm.gameObject);
            return;
        }

        if (bgm != null)
        {
            DontDestroyOnLoad(bgm.gameObject);    
        }
        
    }


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
