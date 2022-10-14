using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        BgmManager[] bgm = FindObjectsOfType<BgmManager>();
        if(bgm.Length > 1)
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
