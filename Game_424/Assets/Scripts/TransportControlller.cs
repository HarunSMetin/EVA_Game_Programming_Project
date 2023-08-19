using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportControlller : MonoBehaviour
{
    GameObject corruptedNeuron;
    GameObject Player;
    bool isHolding = false;
    // Start is called before the first frame update
    void Awake()
    {
        corruptedNeuron = GameObject.FindWithTag("CorruptedNeuron");
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(corruptedNeuron.transform.position, Player.transform.position);
        if(distance < 5f)
        {
            if (Input.GetButtonUp("Hold"))
            {
                if (isHolding)
                {
                    corruptedNeuron.transform.parent = null;
                    corruptedNeuron.GetComponent<Rigidbody>().isKinematic = false;
                    isHolding = false;
                }
                else
                {
                    corruptedNeuron.transform.parent = Player.transform;
                    corruptedNeuron.GetComponent<Rigidbody>().isKinematic = true;
                    isHolding = true;
                }
            }
        }
    }
}
