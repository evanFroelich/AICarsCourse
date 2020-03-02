using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MAX_WHEEL_ROTATION = 30.0f;

    private Rigidbody rigidbody;

    [SerializeField]
    private int playerIndex = 0;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float breakStrength = 1.0f;
    [SerializeField]
    private float turnSpeed = 1.0f;
    [SerializeField]
    private float turnRatio = 10.0f;
    [SerializeField]
    private float airTurnRate = 1.0f;
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float traction = 0.25f;
    [SerializeField]
    private float jumpStrength = 5.0f;

    [SerializeField]
    private Transform centerOfMass;
    [SerializeField]
    private Transform accerationPoint;

    [SerializeField]
    private Suspension suspensionFR;
    [SerializeField]
    private Suspension suspensionFL;
    [SerializeField]
    private Suspension suspensionBR;
    [SerializeField]
    private Suspension suspensionBL;

    [SerializeField]
    private Transform wheelFR;
    [SerializeField]
    private Transform wheelFL;
    [SerializeField]
    private Transform wheelBR;
    [SerializeField]
    private Transform wheelBL;

    [SerializeField]
    private GameObject misslePrefab;
    [SerializeField]
    private Transform missleSpawnLocation;

    private Vector3 resetPosition;

    [SerializeField]
    public InputController input;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        resetPosition = transform.position;
        //name = "Player " + playerIndex.ToString();
    }

    public void FixedUpdate()
    {
        rigidbody.centerOfMass = centerOfMass.localPosition;
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            rigidbody.AddForceAtPosition(new Vector3(0, 100, 0), new Vector3(0.1f, 0, 0.1f));
        }

        float forwardVelocity = Vector3.Dot(rigidbody.velocity, transform.forward);
        float steering = input.GetSteering();
        float vertical = 0;//Input.GetAxis("Vertical" + GetPlayerSuffix());
        
        if (IsGrounded())
        {
            
            float surfaceSlideTraction = 0;
            try
            {
                surfaceSlideTraction += suspensionFL.GetSurface().GetSlideTraction();
                surfaceSlideTraction += suspensionFR.GetSurface().GetSlideTraction();
                surfaceSlideTraction += suspensionBL.GetSurface().GetSlideTraction();
                surfaceSlideTraction += suspensionBR.GetSurface().GetSlideTraction();
                surfaceSlideTraction /= 4;
            }
            catch
            {
                Debug.Log("Blame Andrew");
            }
            float surfaceAccerationTraction = 0;
            try
            {
                surfaceAccerationTraction += suspensionFL.GetSurface().GetAccelerationTraction();
                surfaceAccerationTraction += suspensionFR.GetSurface().GetAccelerationTraction();
                surfaceAccerationTraction += suspensionBL.GetSurface().GetAccelerationTraction();
                surfaceAccerationTraction += suspensionBR.GetSurface().GetAccelerationTraction();
                surfaceAccerationTraction /= 4;
            }
            catch
            {
                Debug.Log("Blame Andrew");
            }
            

            
            

            float surfaceBrakeTraction = 0;
            try
            {
                surfaceBrakeTraction += suspensionFL.GetSurface().GetBrakeTraction();
                surfaceBrakeTraction += suspensionFR.GetSurface().GetBrakeTraction();
                surfaceBrakeTraction += suspensionBL.GetSurface().GetBrakeTraction();
                surfaceBrakeTraction += suspensionBR.GetSurface().GetBrakeTraction();
                surfaceBrakeTraction /= 4;
            }
            catch
            {
                Debug.Log("Blame Andrew");
            }
            

            float surfaceSteeringTraction = 0;
            try
            {
                surfaceSteeringTraction += suspensionFL.GetSurface().GetSteeringTraction();
                surfaceSteeringTraction += suspensionFR.GetSurface().GetSteeringTraction();
                surfaceSteeringTraction += suspensionBL.GetSurface().GetSteeringTraction();
                surfaceSteeringTraction += suspensionBR.GetSurface().GetSteeringTraction();
                surfaceSteeringTraction /= 4;
            }
            catch
            {
                Debug.Log("Blame Andrew");
            }
            

            //Wheel spin
            wheelBL.transform.Rotate(new Vector3(forwardVelocity, 0, 0));
            wheelBR.transform.Rotate(new Vector3(forwardVelocity, 0, 0));
            wheelFL.transform.Rotate(new Vector3(forwardVelocity, 0, 0));
            wheelFR.transform.Rotate(new Vector3(forwardVelocity, 0, 0));

            //Wheel rotation
            Vector3 rotation;
            rotation = suspensionFR.transform.localEulerAngles;
            rotation.y = MAX_WHEEL_ROTATION * steering;
            suspensionFR.transform.localEulerAngles = rotation;
            rotation = suspensionFL.transform.localEulerAngles;
            rotation.y = MAX_WHEEL_ROTATION * steering;
            suspensionFL.transform.localEulerAngles = rotation;

            float gas = -input.GetAcceration();
            gas = Mathf.Clamp01(gas);
            Vector3 forward = (suspensionFL.HitPosition() - suspensionBL.HitPosition()).normalized;
            rigidbody.AddForceAtPosition(forward * speed * gas * surfaceAccerationTraction, accerationPoint.position);
            //Debug
            //Debug.DrawRay(accerationPoint.position, forward * 5.0f, Color.cyan);

            //Breaking
            float breaks = input.GetAcceration();
            breaks = Mathf.Clamp01(breaks);
            if (forwardVelocity < 1 && forwardVelocity > -1)
                breaks *= Mathf.Abs(forwardVelocity);
            rigidbody.AddForceAtPosition(-forward * breakStrength * breaks * Mathf.Sign(forwardVelocity) * surfaceBrakeTraction, accerationPoint.position);

            //Steering
            float steeringForce = steering * turnSpeed * Mathf.Clamp01(Mathf.Abs(forwardVelocity) / turnRatio);
            rigidbody.AddTorque(new Vector3(0, steeringForce * surfaceSteeringTraction * Mathf.Sign(forwardVelocity), 0));

            //Tracktion
            Vector3 slideVector = Vector3.Dot(rigidbody.velocity, transform.right) * transform.right;
            rigidbody.AddForce(-slideVector * traction * surfaceSlideTraction);
            
            rigidbody.AddTorque(-rigidbody.angularVelocity * surfaceSteeringTraction);

            
            //Debug
            //Debug.DrawRay(transform.position, -slideVector * traction * 10.0f, Color.yellow);

            //Sorry no jumping
            //if (Input.GetButton("Jump" + GetPlayerSuffix()))
            //{
            //    rigidbody.AddForce(transform.up * jumpStrength);
            //}
        }
        else
        {
            rigidbody.AddTorque(transform.up * steering * airTurnRate);
            rigidbody.AddTorque(transform.right * vertical * airTurnRate);
        }
    }

    private bool IsGrounded()
    {
        return suspensionBL.IsGrounded() && suspensionBR.IsGrounded() && suspensionFL.IsGrounded() && suspensionFR.IsGrounded();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + GetComponent<Rigidbody>().centerOfMass, 0.25f);
    }

    public void ResetPosition()
    {
        transform.position = resetPosition;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void SetPlayerIndex(int i)
    {
        playerIndex = i;
    }
}
