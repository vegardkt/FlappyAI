using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;
    public float fitness;

    //Constructor
    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for(int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
    }

    //Deep copy
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for(int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for(int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    //Initialize Neurons;
    private void InitNeurons()
    {
        List<float[]> neuronList = new List<float[]>();

        for(int i = 0; i < layers.Length; i++)
        {
            neuronList.Add(new float[layers[i]]);
        }
        neurons = neuronList.ToArray();
    }

    //Create Weights Matrix
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        for(int i = 1; i< layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];

            for (int j = 0; j < neurons[i].Length; j++ )
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];

                for(int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    //Feed forward this neural network with a given input array.
    public float[] FeedForward(float[] inputs)
    {
        for(int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for(int i = 1; i < layers.Length; i++)
        {
            for(int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f;
                for(int k = 0; k< neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k]* neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }
        return neurons[neurons.Length-1];
    }

    public void Mutate()
    {
        for(int i = 0; i< weights.Length; i++)
        {
            for(int j = 0; j < weights[i].Length; j++)
            {
                for(int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    //Mutate this weight value
                    float randomNumber = UnityEngine.Random.Range(0, 1000f);

                    if(randomNumber <= 2f)
                    {
                        weight *= -1f;
                    }
                    else if(randomNumber <= 4f)
                    {
                        weight *= UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    {
                        weight *= UnityEngine.Random.Range(0f, 0.5f) + 1f;
                    }
                    else if (randomNumber <= 8f)
                    {
                        weight *= UnityEngine.Random.Range(0f, 1f);
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void AddFitness(float fit)
    {
        fitness += fit;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public float GetFitness()
    {
        return fitness;
    }

    //Compare neural network and sort based on fitness
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        if(fitness > other.fitness)
        {
            return 1;
        }
        else if (fitness < other.fitness)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
