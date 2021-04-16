using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public enum guns { gunA, gunB, gunC };

    public guns currentGun;

    [HideInInspector]
    public float damage, speed, size;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetDataCoroutine());
    }


    IEnumerator SetDataCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        switch (currentGun)
        {
            case guns.gunA:
                damage = GunsData.instance.gunADamage;
                speed = GunsData.instance.gunASpeed;
                size = GunsData.instance.gunASize;
                break;
            case guns.gunB:
                damage = GunsData.instance.gunBDamage;
                speed = GunsData.instance.gunBSpeed;
                size = GunsData.instance.gunBSize;
                break;
            case guns.gunC:
                damage = GunsData.instance.gunCDamage;
                speed = GunsData.instance.gunCSpeed;
                size = GunsData.instance.gunCSize;
                break;
        }
    }
}
