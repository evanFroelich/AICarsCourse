using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject spawnPoint;
    public GameObject initialGate;

    public string typeOfBrain;
    public int numChildren;
    public float percentMutate;

    public float GateBaseReward;
    public float GateTimedReward;
    public float MaxTimeLimit;
    public float MaxTimeReduction;

    public float baseSpeed;
    public int maxTimePerGen;
    public int popsize;
    public float maxFitness=0;
    public List<GameObject> population;
    public List<GameObject> alivePopulation;
    private int generation = 0;
    StreamWriter logFile;

    // Start is called before the first frame update
    void Start()
    {
        //Physics.IgnoreLayerCollision(2, 2);
        logFile = new StreamWriter(new FileStream("population_log.txt", FileMode.OpenOrCreate));
        initiatePop();
        nextGenInvoker(typeOfBrain);
        checkForDeadPop();
        sortInvoker();
        aliveListSorter();
    }

    public static int sortByFitness(GameObject go1, GameObject go2)
    {
        return go1.gameObject.GetComponent<FitnessHandler>().fitness.CompareTo(go2.gameObject.GetComponent<FitnessHandler>().fitness);
    }

    private void nextGenInvoker(string name)
    {
        switch (name)
        {
            case "simple":
                Debug.Log("simple");
                Invoke("nextGenSimplePop", maxTimePerGen);
                break;
            case "evanV1":
                Debug.Log("evans");
                Invoke("neuralNetworkV1NextGen", maxTimePerGen);
                break;
            default:
                Debug.Log("default");
                Invoke("nextGenSimplePop", maxTimePerGen);
                break;
        }
        
    }

    private void initiatePop()
    {
        population = new List<GameObject>();
        alivePopulation = new List<GameObject>();
        for (int i = 0; i < popsize; i++)
        {
            population.Add(createNewCar(i));
            population[i].GetComponent<SimpleAI>().startBrain();
			List<float> temp=new List<float>();
			temp.Add(2.899261f);
			temp.Add(-0.6421f);
			temp.Add(0.05652734f);
			temp.Add(1.136293f);
			temp.Add(-0.1182122f);
			temp.Add(-16.82977f);
			temp.Add(0.07575151f);
			temp.Add(1.861662f);
			temp.Add(-7.671215f);
			temp.Add(-5.372059f);
			temp.Add(-0.03425169f);
			temp.Add(5.998727f);
			temp.Add(3.951669f);
			temp.Add(0.2731403f);
            
			for(int j=0;j<population[i].GetComponent<SimpleAI>().weights.Length;j++){
				for(int k=0;k<population[i].GetComponent<SimpleAI>().weights[j].Length;k++){
					for(int l=0;l<population[i].GetComponent<SimpleAI>().weights[j][k].Length;l++){
						//population[i].GetComponent<SimpleAI>().weights[j][k][l]=temp[0];
						//temp.RemoveAt(0);
					}
				}
			}
			
            alivePopulation.Add(population[i]);
        }
        
    }

    private void neuralNetworkV1NextGen()
    {
        sortPop();
        //foreach (GameObject go in population) {
        //    print(go.GetComponent<FitnessHandler>().fitness);
        //}
        generation++;
        logDump();
        alivePopulation.Clear();
        List<float[][][]> children = new List<float[][][]>();
        for (int i = 0; i < numChildren; i+=6)
        {
            float[][][] parent1 = population[i].GetComponent<SimpleAI>().weights;
            float[][][] parent2 = population[i+1].GetComponent<SimpleAI>().weights;

            SimpleAI cloneBrain = population[i].GetComponent<SimpleAI>();
            float[][][] child = new float[cloneBrain.weights.Length][][];
            float[][][] child2 = new float[cloneBrain.weights.Length][][];
            float[][][] child3 = new float[cloneBrain.weights.Length][][];
            float[][][] child4 = new float[cloneBrain.weights.Length][][];
            float[][][] child5 = new float[cloneBrain.weights.Length][][];
            float[][][] child6 = new float[cloneBrain.weights.Length][][];

            for (int z = 0; z < cloneBrain.weights.Length; z++)
            {
                child[z] = new float[cloneBrain.weights[z].Length][];
                child2[z] = new float[cloneBrain.weights[z].Length][];
                child3[z] = new float[cloneBrain.weights[z].Length][];
                child4[z] = new float[cloneBrain.weights[z].Length][];
                child5[z] = new float[cloneBrain.weights[z].Length][];
                child6[z] = new float[cloneBrain.weights[z].Length][];
                for (int z2 = 0; z2 < cloneBrain.weights[z].Length; z2++)
                {
                    child[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                    child2[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                    child3[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                    child4[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                    child5[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                    child6[z][z2] = (float[])cloneBrain.weights[z][z2].Clone();
                }
            }
            
            int count = 0;
            for (int j = 0; j < parent1.Length; j++)
            {
                for (int k = 0; k < parent1[j].Length; k++)
                {
                    for (int l = 0; l < parent1[j][k].Length; l++)
                    {
                        count++;
                    }
                }
            }
            int count2 = count;
            for (int j = 0; j < parent1.Length; j++)
            {
                for (int k = 0; k < parent1[j].Length; k++)
                {
                    for (int l = 0; l < parent1[j][k].Length; l++)
                    {
                        count--;
                        if (count < count2 / 2)
                        {
                            //Debug.Log("parent 2");
                            child[j][k][l] = parent2[j][k][l];
                            child3[j][k][l] = parent2[j][k][l];
                            child5[j][k][l] = parent2[j][k][l];

                            child2[j][k][l] = parent1[j][k][l];
                            child4[j][k][l] = parent1[j][k][l];
                            child6[j][k][l] = parent1[j][k][l];
                        }
                        else
                        {
                            //Debug.Log("parent 1");
                            child[j][k][l] = parent1[j][k][l];
                            child3[j][k][l] = parent1[j][k][l];
                            child5[j][k][l] = parent1[j][k][l];

                            child2[j][k][l] = parent2[j][k][l];
                            child4[j][k][l] = parent2[j][k][l];
                            child6[j][k][l] = parent2[j][k][l];
                        }
                    }
                }
            }
            children.Add(child);
            children.Add(child2);
            children.Add(child3);
            children.Add(child4);
            children.Add(child5);
            children.Add(child6);
            //children.Add(parent1);
        }

        for (int i = 0; i < popsize; i++)
        {
            if (popsize - i > numChildren)
            {


                float[][][] tempWeights = population[i].GetComponent<SimpleAI>().weights;
                string brain = brain2String(population[i]);
                //Debug.Log("Initial brain: "+brain);
                Destroy(population[i].gameObject);
                population[i] = createNewCar(i);
                population[i].GetComponent<SimpleAI>().startBrain();
                population[i].GetComponent<SimpleAI>().weights = tempWeights;
                alivePopulation.Add(population[i]);
                brain = brain2String(population[i]);
                //Debug.Log("same new brain: " + brain);
                //Debug.Log("hey");
            }
            else
            {
                //Debug.Log("umm");
                //this line is wrong
                //float[][][] tempWeights = population[i].GetComponent<SimpleAI>().weights;
                float[][][] tempWeights = children[0];
                children.RemoveAt(0);

                

                //mutations
                for (int j = 0; j < tempWeights.Length; j++)
                {
                    for (int k = 0; k < tempWeights[j].Length; k++)
                    {
                        for (int l = 0; l < tempWeights[j][k].Length; l++)
                        {
                            float temp = UnityEngine.Random.Range(0f, 1f);
                            if (temp < percentMutate)
                            {
                                if (children.Count%3==0)
                                {
                                    //Debug.Log("the rest " + children.Count + " " + numChildren);
                                    tempWeights[j][k][l] = UnityEngine.Random.Range(-1f, 1f);
                                }
                                else if (children.Count%3==1)
                                {
                                    //Debug.Log("top 66% " + children.Count + " " + numChildren);
                                    temp = UnityEngine.Random.Range(0, 2);
                                    if (temp > 0)
                                    {
                                        tempWeights[j][k][l] *= .7f;
                                    }
                                    else
                                    {
                                        tempWeights[j][k][l] /= .7f;
                                    }
                                }
                                else
                                {
                                    //Debug.Log("top 33%: " + children.Count + " " + numChildren);
                                    temp = UnityEngine.Random.Range(0, 2);
                                    if (temp > 0)
                                    {
                                        tempWeights[j][k][l] *= .9f;
                                    }
                                    else
                                    {
                                        tempWeights[j][k][l] /= .9f;
                                    }
                                }
                            }
                        }
                    }
                }
                Destroy(population[i].gameObject);
                population[i] = createNewCar(i);
                population[i].GetComponent<SimpleAI>().startBrain();
                
                population[i].GetComponent<SimpleAI>().weights = tempWeights;
                alivePopulation.Add(population[i]);
            }


        }
        Invoke("neuralNetworkV1NextGen", maxTimePerGen);
    }

    private void sortInvoker()
    {
        //Debug.Log("sorting!");
        sortPop();
        Invoke("sortInvoker", 1);
    }

    private void sortPop()
    {
        population.Sort(sortByFitness);
        population.Reverse();
    }

    private void aliveListSorter()
    {
        alivePopulation.Sort(sortByFitness);
        alivePopulation.Reverse();
        Invoke("aliveListSorter",.1f);
    }

    private void nextGenSimplePop()
    {
        sortPop();
        generation++;
        logDump();
        //actually making new things
        alivePopulation.Clear();
        for (int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
            population[i] = createNewCar(i);
            alivePopulation.Add(population[i]);
        }
        Invoke("nextGenSimplePop", maxTimePerGen);
    }

    private GameObject createNewCar(int i)
    {
        GameObject car = Instantiate(carPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        car.gameObject.name = "AI Car " + i.ToString();
        //float temp = UnityEngine.Random.value;
        //temp *= 3f;
        //temp -= 1.5f;
        //temp += baseSpeed;
        float temp = baseSpeed;
        car.GetComponent<SimpleAI>().modi = temp;
        car.GetComponent<PlayerController>().input = car.gameObject.GetComponent<SimpleAI>();
        car.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
        car.GetComponent<SimpleAI>().targetGate = initialGate;
        car.GetComponent<SimpleAI>().BaseGatePoints = GateBaseReward;
        car.GetComponent<SimpleAI>().maxTimedPoints = GateTimedReward;
        car.GetComponent<SimpleAI>().maxGateTime = MaxTimeLimit;
        car.GetComponent<SimpleAI>().maxRewardThreshold = MaxTimeReduction;
        car.GetComponent<SimpleAI>().pointsLostPerSecond = 1;
        return car;
    }

    private void logDump()
    {
        float oldMax = maxFitness;
        //new max fitness check
        maxFitness = population[0].GetComponent<FitnessHandler>().fitness > maxFitness ? population[0].GetComponent<FitnessHandler>().fitness : maxFitness;
        //writing to log if we get a new max fitness
        if (maxFitness > oldMax)
        {
            string brain = brain2String(population[0]);
            logFile.WriteLine("[{0}] Generation: {1}   New Max Fitness: {2}   Brain: {3}", DateTime.Now, generation, maxFitness.ToString(), brain);
            logFile.Close();
            logFile = new StreamWriter(new FileStream("population_log.txt", FileMode.Append));
        }
        else
        {
            //string brain = brain2String(population[0]);
            //logFile.WriteLine("[{0}] Generation: {1}   New Max Fitness: {2}   Brain: {3}", DateTime.Now, generation, maxFitness.ToString(), brain);
            //logFile.WriteLine("[{0}] Generation: {1}   Max Fitness: {2}", DateTime.Now, generation, population[0].GetComponent<FitnessHandler>().fitness.ToString());
            //logFile.Close();
            //logFile = new StreamWriter(new FileStream("population_log.txt", FileMode.Append));
        }
        //Debug.Log("Generation: " + generation);

        FindObjectOfType<CarListManager>().ReturnButtonClicked();
        //dump to log every 100 generations so we can make sure we arent stuck
        if (generation % 100 == 0)
        {
            logFile.WriteLine("[" + DateTime.Now + "]" + " Generation: " + generation);
            logFile.Close();
            logFile = new StreamWriter(new FileStream("population_log.txt", FileMode.Append));
        }
    }

    private string brain2String(GameObject car)
    {
        string temp = "";
        SimpleAI carai = car.GetComponent<SimpleAI>();
        for (int i = 0; i < carai.weights.Length; i++)
        {
            for (int j = 0; j < carai.weights[i].Length; j++)
            {
                for (int k = 0; k < carai.weights[i][j].Length; k++)
                {
                    temp += "\t";
                    temp += carai.weights[i][j][k].ToString("f7");
                    temp += ",";
                }
            }
        }

        return temp;
    }

    private void checkForDeadPop()
    {
        if (alivePopulation.Count==0)
        {
            //CancelInvoke("nextGenSimplePop");
            //nextGenSimplePop();
            CancelInvoke("neuralNetworkV1NextGen");
            neuralNetworkV1NextGen();
            Invoke("checkForDeadPop", .2f);
        }
        else
        {
            Invoke("checkForDeadPop", .2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDestroy()
    {
        logFile.Close();
    }
}
