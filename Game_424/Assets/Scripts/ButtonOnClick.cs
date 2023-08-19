using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{
    public int sphereTag;
    public BoxMovement boxMovement;
    

    //if pressed "E" Boxmoevement script's OnButtonPressed method will be called (index = sphereTag)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Button " + sphereTag + " pressed");
                boxMovement.OnButtonPressed(sphereTag);
            }
        }
    }
}
