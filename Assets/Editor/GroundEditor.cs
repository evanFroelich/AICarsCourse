using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundEditor
{
    [MenuItem("Tools/Ground Item %g")]
    public static void Ground()
    {
        if (Selection.activeGameObject == null)
            return;

        Ray ray = new Ray(Selection.activeGameObject.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Selection.activeGameObject.transform.position = hit.point;
        }
    }
}
