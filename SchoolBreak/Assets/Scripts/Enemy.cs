using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    Rigidbody rb;

    public Transform player;
    public float stoppingDistance = 2f;
    private bool isAttacking = false;

    public Player playerScript;
    public ChangeScenes changeScenes;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        agent.stoppingDistance = stoppingDistance;
        
        // Configurações do Rigidbody para manter 
        if (rb != null)
        {
            rb.isKinematic = true; 
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (playerScript.isCollidingObstacle)
        {
            StopAgent();
            anim.SetInteger("transition", 0);
            return;
        }

        if (distance > stoppingDistance)
        {
            isAttacking = false;
            Move();
        }
        else
        {
            if (!isAttacking)
            {
                Attack();
            }
        }
    }

    void Move()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
            if (rb != null) rb.isKinematic = true;
        }
        
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetInteger("transition", 1);
    }

void Attack()
    {
        isAttacking = true;
        
        agent.enabled = false;
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        
        anim.SetInteger("transition", 2);
        StartCoroutine(WaitAnimation());
    }

    void StopAgent()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        
        float finalDistance = Vector3.Distance(transform.position, player.position);
        if (finalDistance <= stoppingDistance + 1f) 
        {
            changeScenes.SceneGameOver();
        }
        else
        {
            isAttacking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            changeScenes.SceneGameOver();
        }
    }
}

