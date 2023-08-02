using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNController : MonoBehaviour
{

    public GameObject NeuralNetwork;
    public GameObject[][] Layers;
    
    
    void Start()
    {
        NeuralNetwork = GameObject.FindWithTag("NeuralNetwork");
        Layers = new GameObject[NeuralNetwork.transform.childCount][];
        for (int i = 0; i < NeuralNetwork.transform.childCount; i++)
        {
            Layers[i] = new GameObject[NeuralNetwork.transform.GetChild(i).childCount];
            for (int j = 0; j < NeuralNetwork.transform.GetChild(i).childCount; j++)
            {
                Layers[i][j] = NeuralNetwork.transform.GetChild(i).GetChild(j).gameObject;
            }
        }
        Debug.Log(Layers[0]);
    }

    
    void Update()
    {
        
    }
}
