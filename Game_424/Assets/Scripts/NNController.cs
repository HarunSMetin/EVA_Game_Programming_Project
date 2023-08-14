using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NNController : MonoBehaviour
{

    public GameObject NeuralNetwork;
    public GameObject[][][] Layers;
    public Dictionary<GameObject, float> ActivePerceptrons = new Dictionary<GameObject, float>();
    public GameObject inputLayer;
    public GameObject outputLayer;
    public GameObject[][] inputPerceptrons;
    public GameObject[][] outputPerceptrons;
    public int corruptedX=2, corruptedY=1;
    private GameObject blinkingPerceptron;
    private Material originalMaterial;
    public float blinkingDuration = 0.5f;
    public Material glowingMaterial;
    public Material corruptedMaterial;
    public GameObject corruptedPerceptron;
    private int inputLayerSizeX, inputLayerSizeY, outputLayerSizeX, outputLayerSizeY, hiddenLayerSizeX, hiddenLayerSizeY, hiddenLayerSizeZ;
    public AudioSource AudioSource;
    void Awake()
    {
        //Hidden Layer
        NeuralNetwork = GameObject.FindWithTag("HiddenLayer");
        GameObject[] layers = GameObject.FindGameObjectsWithTag("Layers");
        Layers = new GameObject[layers.Length][][];

        for (int x = 0; x < layers.Length; x++)
        {
            Layers[x] = new GameObject[layers[x].transform.childCount][];

            for (int y = 0; y < layers[x].transform.childCount; y++)
            {
                Transform row = layers[x].transform.GetChild(y);
                Layers[x][y] = new GameObject[row.childCount];

                for (int z = 0; z < row.childCount; z++)
                {
                    GameObject perceptron = row.GetChild(z).gameObject;
                    Layers[x][y][z] = perceptron;
                    perceptron.name = "Hidden Perceptron "+z +" "+y+" "+x;
                }
            }
        }
        

        hiddenLayerSizeZ = Layers.Length;
        hiddenLayerSizeY = Layers[0].Length;
        hiddenLayerSizeX = Layers[0][0].Length;
        //Input Layer
        inputLayer = GameObject.FindWithTag("InputLayer");
        inputPerceptrons = new GameObject[inputLayer.transform.childCount][];
        for (int i = 0; i < inputLayer.transform.childCount; i++)
        {
            GameObject row = inputLayer.transform.GetChild(i).gameObject;
            inputPerceptrons[i] = new GameObject[row.transform.childCount];
            for (int j = 0; j < row.transform.childCount; j++)
            {
                GameObject perceptron = row.transform.GetChild(j).gameObject;
                inputPerceptrons[i][j] = perceptron;
                perceptron.name = "Input Perceptron " +i+" "+j;
            }
        }
        inputLayerSizeX = inputPerceptrons.Length;
        inputLayerSizeY = inputPerceptrons[0].Length;
        corruptedPerceptron = inputPerceptrons[corruptedX][corruptedY];
        Material corruptedNeuron = corruptedPerceptron.GetComponent<Renderer>().material;
        corruptedNeuron  = corruptedMaterial;
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
                perceptron.name = "Output Perceptron " +i+" "+j;
            }
        }
        outputLayerSizeX = outputPerceptrons.Length;
        outputLayerSizeY = outputPerceptrons[0].Length;


    }


    void Update()
    {
        //loop active perceptrons
        foreach (KeyValuePair<GameObject, float> entry in ActivePerceptrons.ToList())
        {
            if (entry.Value >= blinkingDuration)
            {
                Renderer rendererLocal = entry.Key.GetComponent<Renderer>();
                rendererLocal.material = (GameObject.ReferenceEquals(entry.Key,corruptedPerceptron))?corruptedMaterial:originalMaterial;
                
                ActivePerceptrons.Remove(entry.Key);
            }
            else if (entry.Value < blinkingDuration)
            {
                ActivePerceptrons[entry.Key] = entry.Value + Time.deltaTime;
            }

        }


        /*if (isBlinking && Time.time >= blinkEndTime)
        {
            Renderer renderer = blinkingPerceptron.GetComponent<Renderer>();
            renderer.material = originalMaterial;
            isBlinking = false;
        }*/
        Material selectedMaterial = glowingMaterial;
        // Blink input perceptron
        if (ActivePerceptrons.Count == 0)
        {
            int randomInputX = Random.Range(0, inputLayerSizeX);
            int randomInputY = Random.Range(0, inputLayerSizeY);
            Debug.Log("Random Input X: " + randomInputX + " Random Input Y: " + randomInputY+" Corrupted X: "+corruptedX+" Corrupted Y: "+corruptedY);
            if (randomInputX == corruptedX && randomInputY == corruptedY)
            {
                Debug.Log("here");
                selectedMaterial = corruptedMaterial;
                AudioSource.Play();
            }
            else
            {
                selectedMaterial = glowingMaterial;
            }
            blinkingPerceptron = inputPerceptrons[randomInputX][randomInputY];
            ActivePerceptrons.Add(blinkingPerceptron, 0.0f);
            originalMaterial = blinkingPerceptron.GetComponent<Renderer>().material;

            Renderer renderer = blinkingPerceptron.GetComponent<Renderer>();
            renderer.material = glowingMaterial; // Apply the glowing material
            renderer.material.EnableKeyword("_EMISSION"); // Enable emission



            // Blink perceptrons in hidden layers
            for (int z = 0; z < hiddenLayerSizeZ; z++)
            {
                
                int randomX = Random.Range(0, hiddenLayerSizeX);
                int randomY = Random.Range(0, hiddenLayerSizeY);
                
                blinkingPerceptron = Layers[z][randomY][randomX];
                ActivePerceptrons.Add(blinkingPerceptron, 0.0f);


                originalMaterial = blinkingPerceptron.GetComponent<Renderer>().material;

                Renderer rendererHidden = blinkingPerceptron.GetComponent<Renderer>();
                rendererHidden.material = glowingMaterial; // Apply the glowing material


            }


            // Blink perceptron in output layer
            int randomOutputX = Random.Range(0, outputLayerSizeX);
            int randomOutputY = Random.Range(0, outputLayerSizeY);
            blinkingPerceptron = outputPerceptrons[randomOutputX][randomOutputY];
            ActivePerceptrons.Add(blinkingPerceptron, 0.0f);
            originalMaterial = blinkingPerceptron.GetComponent<Renderer>().material;

            Renderer rendererOut = blinkingPerceptron.GetComponent<Renderer>();
            rendererOut.material = glowingMaterial; // Apply the glowing material
            rendererOut.material.EnableKeyword("_EMISSION"); // Enable emission


            // Increment the sequence index and wrap around if necessary
        }
    }
}
