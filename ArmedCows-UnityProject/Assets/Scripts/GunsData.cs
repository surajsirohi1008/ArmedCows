using UnityEngine;

class GunsData : MonoBehaviour
{

    public float gunADamage, gunBDamage, gunCDamage;

    public float gunASpeed, gunBSpeed, gunCSpeed;

    public float gunASize, gunBSize, gunCSize;

    public static GunsData instance;

    private void Start()
    {
        instance = this;
    }
}
