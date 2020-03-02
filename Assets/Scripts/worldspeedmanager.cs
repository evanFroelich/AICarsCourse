using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldspeedmanager : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setWorldSpeed(float i)
    {
        //Debug.Log("change " + i);
        speed = i;
        Time.timeScale = speed;
    }
}
