using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DistanceSensor : Sensor
{
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    public GameObject wallBall;
    private RaycastHit hit;
    private bool lastHit;
    [SerializeField]
    Color lineColor;
    [SerializeField]
    float curScore;

    public override float Score()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistance, LayerMask.GetMask("Wall")))
        {
            Vector3 temp=this.GetComponent<LineRenderer>().GetPosition(1);
            temp.z = (hit.distance / maxDistance) * maxDistance;
            this.GetComponent<LineRenderer>().SetPosition(1, temp);
            //wallBall.transform.position = transform.position + (hit.distance * transform.forward);
            wallBall.transform.position = hit.point;
            

            if (hit.distance / maxDistance < .05)
            {
                this.GetComponent<LineRenderer>().SetPosition(1, temp);
                this.GetComponent<LineRenderer>().startColor = Color.red;
                this.GetComponent<LineRenderer>().endColor = Color.red;
                wallBall.GetComponent<Renderer>().material.color = Color.red;
            }
            else if (hit.distance / maxDistance < .2)
            {
                this.GetComponent<LineRenderer>().SetPosition(1, temp);
                this.GetComponent<LineRenderer>().startColor = Color.yellow;
                this.GetComponent<LineRenderer>().endColor = Color.yellow;
                wallBall.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (hit.distance / maxDistance < 1)
            {
                this.GetComponent<LineRenderer>().SetPosition(1, temp);
                this.GetComponent<LineRenderer>().startColor = Color.green;
                this.GetComponent<LineRenderer>().endColor = Color.green;
                wallBall.GetComponent<Renderer>().material.color = Color.green;
            }

            lastHit = true;
            curScore = hit.distance / maxDistance;
            return hit.distance / maxDistance;
        }
        else
        {
            wallBall.transform.position = transform.position;
            Vector3 temp = this.GetComponent<LineRenderer>().GetPosition(1);
            temp.z = maxDistance;
            this.GetComponent<LineRenderer>().SetPosition(1, temp);
            this.GetComponent<LineRenderer>().startColor = Color.cyan;
            this.GetComponent<LineRenderer>().endColor = Color.cyan;
            lineColor = this.GetComponent<LineRenderer>().startColor;
            lastHit = false;
            curScore = 1;
            return 1;
        }
    }
    public void Update()
    {
        Score();
    }

    //public void OnDrawGizmos()
    //{
    //    if (lastHit == false)
    //    {
    //        Gizmos.color = Color.cyan;
    //    }
    //    else
    //    {
    //        if (hit.distance / maxDistance < .2)
    //        {
    //            Gizmos.color = Color.red;
    //        }
    //        else if (hit.distance / maxDistance < .5)
    //        {
    //            Gizmos.color = Color.yellow;
    //        }
    //        else if(hit.distance / maxDistance < 1)
    //        {
    //            Gizmos.color = Color.green;
    //        }
    //    }
    //    //Gizmos.color = Color.red;
    //    if (lastHit)
    //    {
    //        float lineDist = (hit.distance / maxDistance) * maxDistance;
    //        Gizmos.DrawRay(transform.position, transform.forward * lineDist);
    //    }
    //    else
    //    {
    //        Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
    //    }
    //    if (lastHit)
    //        Gizmos.DrawSphere(hit.point, 1.0f);
    //}
}
