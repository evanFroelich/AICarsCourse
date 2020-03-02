using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    private float slideTraction = 1.0f;
    [Range(0, 1)]
    [SerializeField]
    private float accelerationTraction = 1.0f;
    [Range(0, 1)]
    [SerializeField]
    private float brakeTraction = 1.0f;
    [Range(0, 1)]
    [SerializeField]
    private float steeringTraction = 1.0f;

    public float GetSlideTraction()
    {
        return slideTraction;
    }
    public float GetAccelerationTraction()
    {
        return accelerationTraction;
    }
    public float GetBrakeTraction()
    {
        return brakeTraction;
    }
    public float GetSteeringTraction()
    {
        return steeringTraction;
    }
}
