using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NNController : MonoBehaviour
{

    public GameObject NeuralNetwork;
    public GameObject[][][] Layers ;
    public Dictionary<GameObject, int[]> ActivePerceptrons = new Dictionary<GameObject, int[]>();
    public GameObject inputLayer;
    public GameObject outputLayer;
    public GameObject[][] inputPerceptrons;
    public GameObject[][] outputPerceptrons;

    void Start()
    {
        //Hidden Layer
        NeuralNetwork = GameObject.FindWithTag("HiddenLayer");
        GameObject[] layers = GameObject.FindGameObjectsWithTag("Layers");
        Layers = new GameObject[layers.Length][][];
        for (int x =0;x<layers.Length;x++)
        {
            Layers[x] = new GameObject[layers[x].transform.childCount][];
            for(int y = 0; y < layers[x].transform.childCount; y++)
            {
                Layers[x][y] = new GameObject[layers[x].transform.GetChild(y).childCount];
                GameObject row = layers[y].transform.GetChild(y).gameObject;
                for(int z = 0; z < row.transform.childCount; z++)
                {
                    GameObject perceptron = row.transform.GetChild(z).gameObject;
                    Layers[x][y][z] = perceptron;
                }
            }
        }
        
        //Input Layer
        inputLayer = GameObject.FindWithTag("InputLayer");
        inputPerceptrons = new GameObject[inputLayer.transform.childCount][];
        for(int i =0;i<inputLayer.transform.childCount;i++)
        {
            GameObject row = inputLayer.transform.GetChild(i).gameObject;
            inputPerceptrons[i] = new GameObject[row.transform.childCount];
            for(int j = 0; j < row.transform.childCount; j++)
            {
                GameObject perceptron = row.transform.GetChild(j).gameObject;
                inputPerceptrons[i][j] = perceptron;
            }
        }

        //Output Layer
        outputLayer = GameObject.FindWithTag("OutputLayer");
        outputPerceptrons = new GameObject[outputLayer.transform.childCount][];
        for (int i = 0; i < outputLayer.transform.childCount; i++)
        {
            GameObject row = outputLayer.transform.GetChild(i).gameObject;
            outputPerceptrons[i] = new GameObject[row.transform.childCount];
            for (int j = 0; j < row.transform.childCount; j++)
            {
                GameObject perceptron = row.transform.GetChild(j).gameObject;
                outputPerceptrons[i][j] = perceptron;
            }
        }
        Debug.Log("Layers: " + Layers.Length);
        Debug.Log("Input Layer: " + inputLayer.transform.childCount);
        Debug.Log("Output Layer: " + outputLayer.transform.childCount);

        
}


    void Update()
    {
       
    }
}
