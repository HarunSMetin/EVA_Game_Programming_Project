using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Controller : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject stateController;
    // Update is called once per frame
    void Update()
    {
            if (Input.GetButtonDown("SkipLevel"))
            {

                PlayerPrefs.Save();

            }

    }
}
