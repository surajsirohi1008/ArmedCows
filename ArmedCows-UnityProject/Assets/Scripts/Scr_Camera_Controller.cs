using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Camera_Controller : MonoBehaviour
{
    //References
    public GameObject player;
    private Scr_Movement_Controller scr_Movement_Controller;
    private float driftPercent;
    private float input;
    //my vars
    private Vector3 lookAtTarget;
    private Vector3 targetPos;
    private Vector3 offset;

    void Start()
    {
        offset = player.transform.position - transform.position;
        scr_Movement_Controller = player.GetComponent<Scr_Movement_Controller>();
    }

    void LateUpdate()
    {
        //Get variables
        driftPercent = scr_Movement_Controller.driftPercentRead;
        input = scr_Movement_Controller.input;
        //Set Position
        targetPos = player.transform.TransformPoint(-new Vector3(Mathf.Abs(offset.x), offset.y, Mathf.Abs(offset.z)));
        transform.position = targetPos;
        //Set Rotation
        if (driftPercent > 0) 
        {   lookAtTarget = Vector3.Lerp(lookAtTarget, player.transform.TransformPoint(new Vector3(3 * driftPercent * Mathf.Sign(input), 2, 0)), Time.deltaTime * 10f); }
        else { lookAtTarget = Vector3.Lerp(lookAtTarget, player.transform.TransformPoint(0, 2, 5), Time.deltaTime*10f); }
        transform.LookAt(lookAtTarget);
    }
}
