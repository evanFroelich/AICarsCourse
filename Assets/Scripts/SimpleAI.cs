using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : InputController
{
    







    float acceleration = -0.05f;
    float steering;

    public override float GetAcceration()
    {
        return acceleration;
    }
    public override float GetSteering()
    {
        return steering;
    }

    [SerializeField]
    private DistanceSensor leftDistanceSensor;
    [SerializeField]
    private DistanceSensor rightDistanceSensor;
    [SerializeField]
    private DistanceSensor left45DistanceSensor;
    [SerializeField]
    private DistanceSensor right45DistanceSensor;
    [SerializeField]
    private DistanceSensor left15DistanceSensor;
    [SerializeField]
    private DistanceSensor right15DistanceSensor;
    [SerializeField]
    private DistanceSensor frontDistanceSensor;

    [SerializeField]
    private List<float> rewards = new List<float>();

    [SerializeField]
    //private Gate targetGate;
    public GameObject targetGate;
    private const float MAX_GATE_POINTS = 5000;
    private const float GATE_POINTS_LOST_PER_SECOND = 1.0f;
    [SerializeField]
    public float BaseGatePoints;
    public float maxTimedPoints;
    [SerializeField]
    public float pointsLostPerSecond;//dont think im using this?
    [SerializeField]
    private float time;
    [SerializeField]
    public float maxGateTime = 10;
    [SerializeField]
    public float maxRewardThreshold=.8f;

    public float modi;
    
    public void FixedUpdate()
    {
        //simpleAIV1();

        NeuralNetworkV1();
    }
    public void simpleAIV1()
    {
        float diff = rightDistanceSensor.Score() - leftDistanceSensor.Score();
        float diff45 = right45DistanceSensor.Score() - left45DistanceSensor.Score();
        float diff15 = right15DistanceSensor.Score() - left15DistanceSensor.Score();

        float sign = Mathf.Sign(diff);
        diff = Mathf.Pow(diff, 2);
        diff *= sign;

        steering += diff * 5.0f;
        steering += diff45 * 2.5f;
        steering += diff15 * 1.5f;
        sign = Mathf.Sign(steering);

        steering *= (1 - frontDistanceSensor.Score());
        steering = Mathf.Clamp(steering, -1, 1);

        //Debug.Log("90: " + diff.ToString());
        //Debug.Log("45: " + diff45.ToString());
        //Debug.Log("15: " + diff15.ToString());
        //Debug.Log(steering);

        acceleration = -0.15f * (frontDistanceSensor.Score() * modi);
        //Debug.Log(modi);

        //time += Time.fixedDeltaTime;
        time = time > maxGateTime ? maxGateTime : time += Time.fixedDeltaTime;
    }

    

    
    public void Update()
    {
        if (time == maxGateTime)
        {
            if (this.gameObject.GetComponentInChildren<Camera>())
            {
                FindObjectOfType<CarListManager>().ReturnButtonClicked();
            }
            FindObjectOfType<PopulationManager>().GetComponent<PopulationManager>().alivePopulation.Remove(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    private float GateReward()
    {
        //float points = BaseGatePoints + (maxTimedPoints - ((time / maxGateTime) * maxRewardThreshold*maxTimedPoints));
		float points = BaseGatePoints;
        this.gameObject.GetComponent<FitnessHandler>().fitness += points;
        //float points = MAX_GATE_POINTS - GATE_POINTS_LOST_PER_SECOND * time;
        //Points to be awarded

        time = 0;
        return points;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Gate"))
        {
            //Debug.Log("GATE?");
            //Debug.Log(targetGate.gameObject.name + " != " + other.gameObject.name);
            if (other.gameObject.name == targetGate.gameObject.name)
            {
                //Debug.Log("GATE");
                //Reward
                float temp=GateReward();
                rewards.Add(temp);
                //Debug.Log("going to "+other.gameObject.GetComponent<Gate>().nextGate.gameObject.name);
                targetGate = other.GetComponent<Gate>().nextGate;
                //targetGate = targetGate.nextGate;
            }
        }
    }


    
    public float bias;
    //public List<int> neuronsPerLayer = new List<int>();
    [SerializeField]
    private List<int> layers;
    private float[][] neurons;
    public float[][][] weights;

    public List<float> brainReadout;
	
	public List<float> speedReadout=new List<float>();

    public void Start()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    brainReadout.Add(weights[i][j][k]);
                }
            }
        }
    }

    public void startBrain()
    {
        weights = new float[layers.Capacity][][];
        initNeurons();
        initWeights();
    }

    private void initNeurons()
    {
        List<float[]> neuronList = new List<float[]>();
        for (int i = 0; i < layers.Capacity; i++)
        {
            neuronList.Add(new float[layers[i]]);
        }
        neurons = neuronList.ToArray();
    }

    public void initWeights()
    {
        List<float[][]> weightslist = new List<float[][]>();

        for (int i = 1; i < layers.Capacity; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();

            int neuronsInPrevLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPrevLayer];

                for (int k = 0; k < neuronsInPrevLayer; k++)
                {
                    //give random weights
                    neuronWeights[k] = UnityEngine.Random.Range(-1f, 1f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightslist.Add(layerWeightsList.ToArray());
        }
        weights = weightslist.ToArray();
    }

    public void NeuralNetworkV1()
    {
        neuralNetV1Setup();
        for (int i = 1; i < layers.Capacity; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float val = bias;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    val += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = sigmoid(val);
                if (j == 0)//speed
                {
                    neurons[i][j] = neurons[i][j] * -1;
                }
                else//steering
                {
                    neurons[i][j] = (neurons[i][j] * 2) - 1;
                }
            }
        }
        acceleration = neurons[neurons.Length - 1][0];
		speedReadout.Add(acceleration);
        steering= neurons[neurons.Length - 1][1];
        time = time > maxGateTime ? maxGateTime : time += Time.fixedDeltaTime;
        //Debug.Log("car: "+this.name+"   speed: " + neurons[neurons.Length - 1][0] + "  turn: " + neurons[neurons.Length - 1][1]);
    }

    private float sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-1*x));
    }

    private void neuralNetV1Setup()
    {
        neurons[0][0] = leftDistanceSensor.Score();
        neurons[0][1] = left45DistanceSensor.Score();
        neurons[0][2] = left15DistanceSensor.Score();
        neurons[0][3] = frontDistanceSensor.Score();
        neurons[0][4] = right15DistanceSensor.Score();
        neurons[0][5] = right45DistanceSensor.Score();
        neurons[0][6] = rightDistanceSensor.Score();
    }
}
