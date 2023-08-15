using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReferences : MonoBehaviour
{
    // Start is called before the first frame update


    public NavMeshAgent navMeshAgent;
    public Animator animator;

    private void Awake (){
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

}
