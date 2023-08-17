using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Brain : MonoBehaviour
{
    public Transform target;

    private Enemy2Shooting enemy2Shooting;
    private Enemy2References enemy2References; // Add reference to the Enemy2References component

    private float shootingDistance = 50f;

    private void Awake()
    {
        enemy2Shooting = GetComponent<Enemy2Shooting>();
        enemy2References = GetComponent<Enemy2References>(); // Get the Enemy2References component
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange)
            {
                enemy2References.animator.SetBool("shooting", true); // Set shooting animation
                enemy2Shooting.Shoot();
            }
            else
            {
                enemy2References.animator.SetBool("shooting", false); // Reset shooting animation
            }
        }
    }
}
