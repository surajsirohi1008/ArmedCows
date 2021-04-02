using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class Scr_Movement_Controller : MonoBehaviour
{
    public static bool freeze = false;
    private enum State { Driving, Steering, Drifting, KnockedAway };
    [SerializeField] private State MyState;

    [Header("General Settings")]
    public int health = 5;
    [SerializeField] private string leftKey;
    [SerializeField] private string rightKey;
    private KeyCode leftKeyCode, rightKeyCode;
    [SerializeField] private float drivingVelocity;

    [Header("Rotation Settings")]
    [SerializeField] [Range(0f, 10f)] private float timeToMaxDrift;
    [SerializeField] [Range(0f, 1f)] private float driftPercentTreshold;
    [Space(15)]
    [SerializeField] private float maxRadius;
    [SerializeField] private AnimationCurve radiusCurve;
    [Space(15)]
    [SerializeField] [Range(0f, 100f)] private float maxDriftingVelocity;
    [SerializeField] private AnimationCurve driftingVelocityCurve;


    [Header("Components")]
    [SerializeField] private TrailRenderer trailRenderer;
    //[SerializeField] private GameObject body;
    private Scr_Shooting_Controller scr_Shooting_Controller;
    private Rigidbody rb;

    //other parameters
    private float driftPercent;
    private float rotationStartTime;
    [HideInInspector] public float input;
    private float driftPower;
    private int driftingDirection;
    //private Quaternion lastDriftBodyRotation;
    private Vector3 momentum = Vector3.zero;
    private Vector3 knockedAwayDir;
    private float timeOfCollision;
    public GameObject winTextUI, loseTextUI;
    [SerializeField] private GameObject otherPlayer;



    //shared paramaters
    [HideInInspector] public float driftPercentRead, driftPercentTresholdRead;//read by other scripts

    //[Header("Debug")]
    // public float velocity;
    // public float actualRadius;
    // public float actualRotationsPerSecond;
    // public float percentTest1, percentTest2;

    private void Awake()
    {
        driftPercentTresholdRead = driftPercentTreshold;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scr_Shooting_Controller = GetComponent<Scr_Shooting_Controller>();
        trailRenderer.emitting = false;
        rb.maxAngularVelocity = 100f;
        driftPercent = 0;
        leftKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), leftKey);
        rightKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), rightKey);

    }
    void Update()
    {
        // input = Input.GetAxisRaw("Horizontal");//Get Input

        input = Input.GetKey(leftKeyCode) ? -1 : Input.GetKey(rightKeyCode) ? 1 : 0;
        driftPercentRead = driftPercent;
        // velocity = rb.velocity.magnitude;//Debug velocity
        //actualRotationsPerSecond = rb.angularVelocity.magnitude / (2 * Mathf.PI);//Debug rotations per second
        // actualRadius = MyState != State.Driving ? rb.velocity.magnitude / rb.angularVelocity.magnitude : 0;//Debug radius

        if (!freeze)
        {
            switch (MyState)
            {
                case State.Driving:
                    if (input != 0) { DrivingToSteering(); break; }//Switch to Drifting
                    Driving();
                    break;
                case State.Steering:
                    if (input == 0 && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) { AnyToDriving(); break; }//Switch to Driving
                    if (driftPercent >= driftPercentTreshold) { SteeringToDrifting(); break; }//Switch to Driving
                    Rotating();
                    break;
                case State.Drifting:
                    if (input == 0 && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow)) { TriggerShot(); AnyToDriving(); break; }//Switch to Driving
                    if (driftPercent < driftPercentTreshold) { DriftingToSteering(); break; }
                    Rotating();
                    break;
                case State.KnockedAway:
                    KnockedAway();
                    break;
            }

            if (health <= 0 && !freeze)
            {
                StartCoroutine(EndGame());
            }
        }

    }


    private IEnumerator EndGame()
    {
        freeze = true;
        if (!winTextUI.activeSelf)
        {
            loseTextUI.SetActive(true);
        }
        Scr_Movement_Controller otherPlayerScript = otherPlayer.GetComponent<Scr_Movement_Controller>();
        otherPlayerScript.winTextUI.SetActive(true);
        yield return new WaitForSeconds(2f);

        //loseTextUI.SetActive(false);
        //winTextUI.SetActive(false);
        //otherPlayerScript.winTextUI.SetActive(true);
        //otherPlayerScript.loseTextUI.SetActive(true);
        freeze = false;
        SceneManager.LoadScene(0);


    }
    private void KnockedAway()
    {
        if (Time.time > timeOfCollision + .5f)
        {
            MyState = State.Driving;
        }
    }

    private void Driving()
    {
        if (momentum.magnitude > .1f)
        { momentum -= momentum.normalized * 10 * Time.deltaTime; }
        else { momentum = Vector3.zero; }
        float velocityTresholdToDrift = driftPercentTreshold * (maxDriftingVelocity - drivingVelocity) + drivingVelocity;
        if (momentum.magnitude < velocityTresholdToDrift && trailRenderer.emitting)
        { trailRenderer.emitting = false; }

        rb.velocity = transform.forward * drivingVelocity + momentum;
    }
    private void Rotating()//Steering and Drifting
    {
        //Handle Drift direction
        if (driftingDirection != (int)input && input != 0)
        {
            driftingDirection = (int)input;//change direction
            if (driftPercent >= driftPercentTreshold)
            {
                momentum = rb.velocity;
                rotationStartTime = Time.time - timeToMaxDrift * driftPercentTreshold;//Setback drift percent
            }
        }
        if (momentum.magnitude > .1f)
        { momentum -= momentum.normalized * 5 * Time.deltaTime; }
        else { momentum = Vector3.zero; }

        //Lerp angular velocity and velocity values
        float rotationLifeTime = Time.time - rotationStartTime;
        driftPercent = Mathf.Clamp(rotationLifeTime / timeToMaxDrift, 0f, 1f);
        driftPower = Mathf.Clamp((rotationLifeTime - timeToMaxDrift * driftPercentTreshold) / (timeToMaxDrift - timeToMaxDrift * driftPercentTreshold), 0, 1);



        rb.velocity = transform.forward * Mathf.Lerp(drivingVelocity, maxDriftingVelocity, driftingVelocityCurve.Evaluate(driftPercent)) + momentum;
        float angularFrequency = rb.velocity.magnitude / (radiusCurve.Evaluate(driftPercent) * maxRadius);
        rb.angularVelocity = transform.up * angularFrequency * driftingDirection;


    }
    private void DrivingToSteering()//Transition to Steering 
    {//Set up variables
        driftingDirection = (int)input;
        rotationStartTime = Time.time;
        //rotationStartTime = Time.time - timeToMaxDrift * ((rb.velocity.magnitude - drivingVelocity) / (maxDriftingVelocity - drivingVelocity));//Setback drift percent
        MyState = State.Steering;
    }
    private void DriftingToSteering()//Transition to Steering 
    {//Set up variables
        //trailRenderer.emitting = false;
        MyState = State.Steering;
    }
    private void SteeringToDrifting()//Transition to Drifting 
    {//Set up variables

        trailRenderer.emitting = true;
        MyState = State.Drifting;
    }
    private void AnyToDriving()//Transition to Driving 
    {//Set up variables
        momentum = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0, maxDriftingVelocity);
        driftPercent = 0;
        driftPower = 0;
        trailRenderer.emitting = false;
        MyState = State.Driving;
    }
    private void TriggerShot()
    {
        scr_Shooting_Controller.Shoot(driftPower);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            timeOfCollision = Time.time;
            MyState = State.KnockedAway;
            ContactPoint contact = collision.contacts[0];
            Vector3 myDir = transform.TransformDirection(transform.forward);
            knockedAwayDir = Vector3.Reflect(myDir, contact.normal).normalized;
            rb.velocity = Vector3.zero;
            momentum = Vector3.zero;
            rb.AddForce(knockedAwayDir * 20f, ForceMode.Impulse);

            Analytics.CustomEvent("Hit Obstacle");
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
        {
            health -= 1;
            //Destroy(collision.gameObject);
            AnalyticsResult result = Analytics.CustomEvent("Got Shot");

            // Send event

            if (result == AnalyticsResult.Ok)
            {
                print("Event Sent");
            }
            else
            {
                print("Event Not Sent");
            }
        }
    }
}
