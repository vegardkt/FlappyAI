using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject BirdPrefab;

    private int populationSize = 10;
    private int generationNumbr;
    private int[] layers = new int[] { 2, 10, 10, 1 }; //Set size of neural net layers
    public GameObject[] pipes;
    public GameObject pipe;
    private List<NeuralNetwork> nets;
    private List<BirdController> birdsList = null;
    private int birdsAlive;
    private float maxFitness = 0;
    

    private void Start()
    {
        generationNumbr = 0;
    }

    private void Update()
    {
        if(birdsAlive <= 0)
        {
            if(generationNumbr == 0)
            {
                InitBirdNeuralNetwork(); //Initializes the system with all nets
            }
            else //Keep best birds and mutate, reset fitness
            {
                nets.Sort();
                if (nets[populationSize - 1].fitness > maxFitness)
                {
                    maxFitness = nets[populationSize - 1].fitness;
                }
                Debug.Log("cF: " + nets[populationSize - 1].fitness + " | mF: " + maxFitness + " | gNr: " + generationNumbr);
                for (int i = 0;i < populationSize /2; i++)
                {
                    nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                    nets[i].Mutate();

                    nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f); //reset fitness
                }
            }

            generationNumbr = generationNumbr + 1;
            DestroyAllPipes();
            CreateBirdBodies();

        }
    }

    public int getNoOfInputs()
    {
        return layers[0];
    }

    public void BirdKilled()
    {
        birdsAlive-=1;
        //Debug.Log(birdsAlive);
    }

    private void DestroyAllPipes()
    {
        pipes = GameObject.FindGameObjectsWithTag("PipeTag");
        foreach(var pipe in pipes)
        {
            GameObject.Destroy(pipe);
        }
    }

    private void CreateBirdBodies()
    {
        if(birdsList != null)
        {
            for (int i = 0; i< birdsList.Count; i++) //Destroy potential remaining birds
            {
                GameObject.Destroy(birdsList[i].gameObject);
            }
        }

        birdsList = new List<BirdController>();

        for(int i = 0; i< populationSize; i++)
        {
            //1. Instantiate bird prefab
            GameObject temp = ((GameObject)Instantiate(BirdPrefab, new Vector3(-3f, 2f, 0), BirdPrefab.transform.rotation));
            temp.layer = i+8;
            BirdController bird = temp.GetComponent<BirdController>();

            bird.Init(nets[i]);
            birdsList.Add(bird);
        }
        birdsAlive = populationSize;
    }

    void InitBirdNeuralNetwork()
    {
        if (populationSize % 2 != 0)
        {
            populationSize++;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }

}
