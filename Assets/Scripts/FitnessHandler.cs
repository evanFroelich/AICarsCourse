using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessHandler : MonoBehaviour
{
    public float fitness;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //fitness += .001f;
        //fitness *= 1000;
        //fitness = Mathf.Round(fitness);
        //fitness /= 1000;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //Debug.Log("wall");
        }
        if (collision.gameObject.layer != 9)
        {
            //Debug.Log(gameObject.name + " layer#" + collision.gameObject.layer+" called: "+collision.gameObject.name);
            return;
        }
        if (this.gameObject.GetComponentInChildren<Camera>())
        {
            FindObjectOfType<CarListManager>().ReturnButtonClicked();
        }
        FindObjectOfType<PopulationManager>().GetComponent<PopulationManager>().alivePopulation.Remove(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
