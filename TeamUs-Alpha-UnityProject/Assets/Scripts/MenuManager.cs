﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayGame()
    {
        SceneManager.LoadScene("AlphaGame");
    }

    void EndGame()
    {
        Application.Quit();
        Debug.Log("You quit");
    }
}
