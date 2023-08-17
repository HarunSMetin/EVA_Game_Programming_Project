using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Transform player;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player);
    }
}
