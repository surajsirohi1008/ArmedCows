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

    public void Shoot(float percent)
    {
        if (percent > 0 && percent < 33.33f)
            Analytics.CustomEvent("Shot Level: " + 1);
        else if (percent > 33.33f && percent < 66.66f)
            Analytics.CustomEvent("Shot Level: " + 2);
        else if (percent > 66.66f)
            Analytics.CustomEvent("Shot Level: " + 3);

        StartCoroutine(ShootACoroutine(percent));
    }

    IEnumerator ShootACoroutine(float percent) //3 weak but rapid shots
    {
        //Get shot amount
        int shots = (int)(percent * 3f) + 1;
        while (shots > 0)//Spawn and shoot each projectile
        {
            GameObject newProjectile = Instantiate(projectilePrefab, shooterObj.transform.position, shooterObj.transform.rotation, temporaryStorage);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

            AnalyticsResult result = Analytics.CustomEvent("Bullet Shot");
            // Send event
            if (result == AnalyticsResult.Ok)
            {   print("Event Sent");}
            else
            {  print("Event Not Sent"); }

            //Setup projectile
            newProjectile.GetComponent<Projectile>().damage = GunsData.instance.gunADamage;
            newProjectile.transform.localScale = Vector3.one * GunsData.instance.gunASize;
            rb.velocity = newProjectile.transform.forward * GunsData.instance.gunASpeed;
            //wait
            yield return new WaitForSeconds(.2f);
            shots -= 1;
            yield return null;
        }
        yield return null;
    }
}
