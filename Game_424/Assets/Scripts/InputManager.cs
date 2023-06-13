using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //Singleton class
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("/Input Manager");
                if (go == null)
                {
                    return null;
                }

                instance = go.GetComponent<InputManager>();
                if (instance == null)
                {
                    return null;
                }
            }
            return instance;
        }
    }

    public bool OK = false;      
    public bool lockKey = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.KeypadEnter))
            OK = true; 
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
            OK = false;


        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.L))
                lockKey = true;
        }
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyUp(KeyCode.L))
                lockKey = false;
        }
    }
}
