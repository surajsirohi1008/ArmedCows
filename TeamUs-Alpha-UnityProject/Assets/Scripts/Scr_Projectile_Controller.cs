using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Projectile_Controller : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject particleSystem = Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(particleSystem, 2f);
        Destroy(transform.gameObject);
    }
}
