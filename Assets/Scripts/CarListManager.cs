using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarListManager : MonoBehaviour
{
    public List<GameObject> buttons;
    public GameObject pm;
    public GameObject camera;
    [SerializeField]
    private GameObject carFocus;
    Vector3 camstart;
    Quaternion camrot;
    float camsize;

    // Start is called before the first frame update
    void Start()
    {
        camstart = camera.transform.position;
        camrot = camera.transform.rotation;
        camsize = camera.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        

        for(int i = 0; i < buttons.Count; i++)
        {
            //buttons[i].GetComponentsInChildren<Text>()[0].text = pm.GetComponent<PopulationManager>().population[i].name;
            try
            {
                buttons[i].GetComponentsInChildren<Text>()[0].text = pm.GetComponent<PopulationManager>().alivePopulation[i].name;
            }
            catch
            {
                buttons[i].GetComponentsInChildren<Text>()[0].text = "NONE";
            }
            
        }
    }

    public void ButtonClicked(int index)
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = camstart;
        camera.transform.rotation = camrot;
        camera.GetComponent<Camera>().orthographicSize = camsize;
        camera.transform.parent = null;
        if (carFocus)
        {
            if (carFocus.name != pm.GetComponent<PopulationManager>().alivePopulation[index].name)
            {
                foreach (var item in carFocus.GetComponentsInChildren<LineRenderer>())
                {
                    item.enabled = false;
                    item.gameObject.GetComponent<DistanceSensor>().wallBall.SetActive(false);
                }
            }
        }
        
        carFocus = pm.GetComponent<PopulationManager>().alivePopulation[index];
        try
        {
            
            Vector3 offset = new Vector3(0, 20, 0);
            camera.transform.position = pm.GetComponent<PopulationManager>().alivePopulation[index].transform.position + offset;
            camera.transform.rotation = pm.GetComponent<PopulationManager>().alivePopulation[index].transform.rotation;
            Vector3 tempVec = camera.transform.rotation.eulerAngles;
            tempVec.x = 90;
            camera.transform.rotation = Quaternion.Euler(tempVec);
            tempVec = camera.transform.position;
            tempVec += (pm.GetComponent<PopulationManager>().alivePopulation[index].transform.forward * 50);
            //tempVec.z += 40;
            camera.transform.position = tempVec;
            camera.GetComponent<Camera>().orthographicSize = 100;
            camera.transform.parent = pm.GetComponent<PopulationManager>().alivePopulation[index].transform;
            
            foreach (var item in pm.GetComponent<PopulationManager>().alivePopulation[index].GetComponentsInChildren<LineRenderer>())
            {
                item.enabled = true;
                item.gameObject.GetComponent<DistanceSensor>().wallBall.SetActive(true);
            }
            
        }
        catch
        {
            Debug.Log("cant press the none buton");
        }
        
    }

    public void ButtonClicked(GameObject car)
    {
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = camstart;
        camera.transform.rotation = camrot;
        camera.GetComponent<Camera>().orthographicSize = camsize;
        camera.transform.parent = null;
        if (carFocus)
        {
            if (carFocus.name != car.name)
            {
                foreach (var item in carFocus.GetComponentsInChildren<LineRenderer>())
                {
                    item.enabled = false;
                    item.gameObject.GetComponent<DistanceSensor>().wallBall.SetActive(false);
                }
            }
        }

        carFocus = car;
        try
        {

            Vector3 offset = new Vector3(0, 2, 0);
            offset += (car.transform.forward * -10);
            
            //camera.transform.LookAt(car.transform);
            camera.GetComponent<Camera>().orthographic = false;
            camera.transform.position = car.transform.position + offset;
            camera.transform.rotation = car.transform.rotation;
            Vector3 tempVec = camera.transform.rotation.eulerAngles;
            tempVec.x = 90;
            camera.transform.rotation = Quaternion.Euler(tempVec);
            camera.transform.LookAt(car.transform);
            tempVec = camera.transform.position;
            tempVec += (car.transform.up * 5);
            //tempVec.z += 40;
            camera.transform.position = tempVec;
            camera.GetComponent<Camera>().orthographicSize = 100;
            camera.transform.parent = car.transform;

            foreach (var item in car.GetComponentsInChildren<LineRenderer>())
            {
                item.enabled = true;
                item.gameObject.GetComponent<DistanceSensor>().wallBall.SetActive(true);
            }

        }
        catch
        {
            Debug.Log("cant press the none buton");
        }
    }

    public void ReturnButtonClicked()
    {
        //Debug.Log("return");
        camera.GetComponent<Camera>().orthographic = true;
        camera.transform.position = camstart;
        camera.transform.rotation = camrot;
        camera.GetComponent<Camera>().orthographicSize = camsize;
        camera.transform.parent = null;
        if (carFocus)
        {
            foreach (var item in carFocus.GetComponentsInChildren<LineRenderer>())
            {
                item.enabled = false;
            }
        }
    }
}
