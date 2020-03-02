using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour
{
    [SerializeField]
    private float length;
    [SerializeField]
    private float strength;
    [SerializeField]
    private float expansionDampen;
    [SerializeField]
    private float compressionDampen;

    [SerializeField]
    private Transform attachmentPoint;

    [SerializeField]
    private GameObject skidObject;

    private float compression;
    private float lastCompression;
    private Rigidbody rigidbody;
    private RaycastHit hit;

    private bool grounded;
    private Surface surface;

    public void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();    
    }

    public void FixedUpdate()
    {
        Physics.queriesHitTriggers = false;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit, length))
        {
            compression = (length - hit.distance) / length;
            grounded = true;
            surface = hit.collider.gameObject.GetComponent<Surface>();
            EnableSkid(true);
        }
        else
        {
            compression = 0;
            grounded = false;
            EnableSkid(false);
        }
        Physics.queriesHitTriggers = true;

        float damp = (lastCompression - compression);
        if (damp < 0)
            damp *= compressionDampen;
        else
            damp *= expansionDampen;
        float force = strength * compression - damp;
        if (force < 0)
            force = 0;
        rigidbody.AddForceAtPosition(transform.up * force, transform.position);

        attachmentPoint.localPosition = new Vector3(0, -length * (1 - compression));

        lastCompression = compression;
    }

    public Vector3 HitPosition()
    {
        return hit.point;
    }

    public bool IsGrounded()
    {
        return grounded;
    }
    public Surface GetSurface()
    {
        return surface;
    }

    private void EnableSkid(bool val)
    {
        if (skidObject == null)
            return;
        skidObject.GetComponent<TrailRenderer>().emitting = val;
    }

    public void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position - transform.up * length * (1.0f - compression), Color.red);
        Debug.DrawLine(transform.position - transform.up * length * (1.0f - compression), transform.position - transform.up * length, Color.green);
    }
}
