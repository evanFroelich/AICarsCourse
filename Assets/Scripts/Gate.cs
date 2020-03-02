using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Gate : MonoBehaviour
{
    [SerializeField]
    //public Gate nextGate;
    public GameObject nextGate;

    public void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(collider.center, collider.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(collider.center, collider.size);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Vector3.zero, 1.0f);
    }
}
