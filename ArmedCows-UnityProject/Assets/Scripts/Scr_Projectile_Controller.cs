using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Projectile_Controller : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
   
    private void OnCollisionEnter(Collision collision)
    {
        //Instantiate particles
        GameObject particleSystem = Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        //Destroy particles and self
        Destroy(particleSystem, 2f);
        Destroy(transform.gameObject);
    }
}
