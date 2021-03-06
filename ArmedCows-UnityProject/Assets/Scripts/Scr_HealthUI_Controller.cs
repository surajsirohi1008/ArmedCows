using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_HealthUI_Controller : MonoBehaviour
{
    [SerializeField] private Image box, fill;
    [SerializeField] private float widthInPixels = 200;
    [SerializeField] private float heightInPixels = 50;
    [SerializeField] private Scr_Movement_Controller scr_Movement_Controller;
    private float maxHealth;
    private float health;
    void Start()
    {
        //Set UI Box size a
        box.rectTransform.sizeDelta = new Vector2(widthInPixels, heightInPixels);
        fill.rectTransform.sizeDelta = new Vector2(widthInPixels, heightInPixels);
        maxHealth = scr_Movement_Controller.health;
    }

    void Update()
    {
        //Update Health Bat
        health = scr_Movement_Controller.health;//Get player's health
        fill.rectTransform.sizeDelta = new Vector2(widthInPixels * (health / maxHealth), heightInPixels);//Set Fill
    }
}
