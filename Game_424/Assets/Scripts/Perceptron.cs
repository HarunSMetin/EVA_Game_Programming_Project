using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron : MonoBehaviour
{
    Material material;

    public float emissionIntensity = 0.23f;

    public void ChangeColor(Color color)
    {
        // Set the emission color to the desired color with the specified intensity
        if(color==Color.red)
            material.SetColor("_EmissionColor", color * emissionIntensity);
        else
        {
            material.color = color;
        }
    }

    void Start()
    {
        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION"); // Enable emission for the material
    }


    void Update()
    {
       
    }
}
