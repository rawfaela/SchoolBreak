using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;

    public Transform player;
    public float stoppingDistance = 2f;
    private bool isAttacking = false;

    public Player playerScript;
    public ChangeScenes changeScenes;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (playerScript.isCollidingObstacle)
        {
            agent.isStopped = true;
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
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetInteger("transition", 1);
    }

    void Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        anim.SetInteger("transition", 2);
        StartCoroutine(WaitAnimation());
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f); // espera a animação
        changeScenes.SceneGameOver();
    }
}
