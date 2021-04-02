using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;

public class Scr_Shooting_Controller : MonoBehaviour
{
    [SerializeField] private GameObject shooterObj;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform temporaryStorage;
    public float projectileSpeed;

    int currentGun;


    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Shoot(float percent)
    {
        if (percent > 0 && percent < 33.33f)
            Analytics.CustomEvent("Shot Level: " + 1);
        else if (percent > 33.33f && percent < 66.66f)
            Analytics.CustomEvent("Shot Level: " + 2);
        else if (percent > 66.66f)
            Analytics.CustomEvent("Shot Level: " + 3);

        StartCoroutine(ShootACoroutine(percent));


        //switch (currentGun)
        //{
        //    case 0:
        //        ShootA(percent);
        //        break;
        //    case 1:
        //        ShootB(percent);
        //        break;
        //    case 2:
        //        ShootC(percent);
        //        break;
        //}
    }

    //public void ShootA(float percent)
    //{
    //    StartCoroutine(ShootACoroutine(percent));
    //}

    IEnumerator ShootACoroutine(float percent) //3 weak but rapid shots
    {
        int shots = (int)(percent * 3f) + 1;
        while (shots > 0)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, shooterObj.transform.position, shooterObj.transform.rotation, temporaryStorage);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();


            AnalyticsResult result = Analytics.CustomEvent("Bullet Shot");

            // Send event

            if (result == AnalyticsResult.Ok)
            {
                print("Event Sent");
            }
            else
            {
                print("Event Not Sent");
            }

            newProjectile.GetComponent<Projectile>().damage = GunsData.instance.gunADamage;

            newProjectile.transform.localScale = Vector3.one * GunsData.instance.gunASize;

            rb.velocity = newProjectile.transform.forward * GunsData.instance.gunASpeed;

            yield return new WaitForSeconds(.2f);
            shots -= 1;
            yield return null;
        }
        yield return null;


    }

    //void ShootB(float percent)
    //{
    //    GameObject newProjectile = Instantiate(projectilePrefab, shooterObj.transform.position, shooterObj.transform.rotation, temporaryStorage);
    //    Rigidbody rb = newProjectile.GetComponent<Rigidbody>();


    //    newProjectile.GetComponent<Projectile>().damage = GunsData.instance.gunBDamage;

    //    newProjectile.GetComponent<Projectile>().size = GunsData.instance.gunBSize;

    //    rb.velocity = newProjectile.transform.forward * GunsData.instance.gunBSpeed; ;
    //}

    //void ShootC(float percent)
    //{
    //    GameObject newProjectile = Instantiate(projectilePrefab, shooterObj.transform.position, shooterObj.transform.rotation, temporaryStorage);
    //    Rigidbody rb = newProjectile.GetComponent<Rigidbody>();


    //    newProjectile.GetComponent<Projectile>().damage = GunsData.instance.gunCDamage;

    //    newProjectile.GetComponent<Projectile>().size = GunsData.instance.gunCSize;

    //    rb.velocity = newProjectile.transform.forward * GunsData.instance.gunCSpeed; ;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.gameObject.layer == 11)
    //    {
    //        currentGun = (int)other.GetComponent<GunPickup>().currentGun;
    //    }
    //}


    //IEnumerator WaitToShoot(float waitTime, float percent)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    Shoot(percent - .33333f);
    //    yield return null;
    //}
}
