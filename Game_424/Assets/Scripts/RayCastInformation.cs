using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class RayCastInformation : MonoBehaviour
{
   

    // Public variables for customization
    public Camera playerCamera;
    public Transform laserOrigin;
    
    public float x = 0.5f, y = 0.5f, z = 0.5f;



    LineRenderer laserLine;







    private void Awake()
    {
        
        laserLine = GetComponent<LineRenderer>();
   
        
        

    }

    private void Update()
    {
       
        
        
    }

    
   

   











}
