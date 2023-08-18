using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public new Transform transform;
    public enum EnemyType
    {
        X,
        Y,
        Z
    }

    public EnemyType enemyType;
    public float moveSpeedMin = 1f;
    public float moveSpeedMax = 10f;
    private float moveSpeed;
    public float maxHealth = 1000;
    private float currentHealth;
    private bool isStopped = false;
    public GameObject playerCharacter;
    public Transform player;
    private Rigidbody rb;
    Animator anim;

    private GameObject HealthBar;

    public static List<EnemyController> allEnemies = new List<EnemyController>();

    public float stunDuration = 2.0f; // The duration of the stun in seconds
    private bool isStunned = false; // Flag to indicate if the enemy is currently stunned

    private float timer = 0f;
  
    private void Start()
    {
        var canvas = GetComponentInChildren<Canvas>().gameObject;
        HealthBar = canvas.transform.GetChild(0).gameObject;
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        playerCharacter = GameObject.FindWithTag("Player");
        player = playerCharacter.transform;
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
        allEnemies.Add(this);
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isStunned && player && !isStopped)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;
            rb.MoveRotation(Quaternion.LookRotation(moveDirection));
        }
        HealthBar.transform.LookAt(Camera.main.transform);
        HealthBar.GetComponent<Slider>().value = currentHealth / maxHealth;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (rb.velocity.magnitude > 0.1f)
        {
            anim.SetBool("Walk Forward", true);
        }
        else
        {
            anim.SetBool("Walk Forward", false);
        }
    }

    public bool TakeDamage(float damage)
    {
        currentHealth -= damage;
        anim.SetBool("Take Damage", true);
         // Enemy is stunned when taking damage
        if(!isStunned)
        {
            isStopped = true;
            isStunned = true;
            StartCoroutine(TimerToMove());
        }
        
        
        if (currentHealth <= 0)
        {
            allEnemies.Remove(this);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    IEnumerator TimerToMove()
    {
       
        yield return new WaitForSeconds(stunDuration);
        isStopped = false;
        isStunned = false; // Reset the stun flag after the stun duration is over
    }
}
