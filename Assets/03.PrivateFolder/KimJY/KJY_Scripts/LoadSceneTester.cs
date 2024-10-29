using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LoadSceneTester : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadSceneController.LoadScene("03.LobbyScene");
        }
        
    }
}
