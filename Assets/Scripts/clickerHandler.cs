using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickInfo = new RaycastHit(); //= Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out clickInfo);
            bool hit = Physics.Raycast(this.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out clickInfo);
            if (hit)
            {
                if (FindObjectOfType<PopulationManager>().alivePopulation.Contains(clickInfo.transform.gameObject))
                {
                    //Debug.Log("hit! " + clickInfo.transform.gameObject.name);
                    FindObjectOfType<CarListManager>().ButtonClicked(clickInfo.transform.gameObject);
                }
            }
            else
            {
                Debug.Log("didnt hit anything?");
            }
        }
    }
}
