using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_Scene_Manager : MonoBehaviour
{
    void Update()
    {
        //Reload Scene on input press
        if (Input.GetKeyDown(KeyCode.R))
        {   SceneManager.LoadScene(0); }
    }
}
