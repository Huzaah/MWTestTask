using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public HealthBarScript healthBar;
    public Animator animator;
    public CharacterController controller;
    private Transform playerTransform;
    public Transform lookOrigin;
    public float viewDistance = 10;
    public LayerMask mask;
    public float moveSpeed = 5;
    public float rotateSmoothTime;
    private float rotateSmoothVelocity;
    private Vector3 lastPosition;
    public float attackDistance;
    public Transform anchorL;
    public Transform anchorR;
    public enum EnemyState
    {
        Standby = 0,
        Running = 1,
        Fighting = 2
    }
    public EnemyState enemyState;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = EnemyState.Standby;

        currentHealth = maxHealth;
        healthBar.SetMaxhealth(maxHealth);
    }
    void FixedUpdate()
    {
        CheckDistanceToPlayer();
        CallAnimator();
    }
    void CheckDistanceToPlayer()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance &&
        !animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
        {
            enemyState = EnemyState.Fighting;
            Attack();
            return;
        }

        LookForPlayer();
    }
    void LookForPlayer()
    {
        Ray ray = new Ray(lookOrigin.position, (playerTransform.position - transform.position).normalized);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, viewDistance, mask))
        { // Если противник видит игрока в радиусе
            enemyState = EnemyState.Running;
            Movement();
        }
        else
        { // Если противник не видит игрока в радиусе
            enemyState = EnemyState.Standby;
        }
    }
    void Movement()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSmoothVelocity, rotateSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);

        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        controller.SimpleMove(moveDir.normalized * moveSpeed);
    }
    void Attack()
    {
        animator.SetTrigger("Attacked");
        Debug.Log("Enemy " + transform.GetSiblingIndex() + " has attacked");
    }
    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    void CallAnimator()
    {
        switch (enemyState)
        {
            case EnemyState.Standby:
                {
                    animator.SetBool("Running", false);
                    break;
                }
            case EnemyState.Running:
                {
                    animator.SetBool("Running", true);
                    break;
                }
            case EnemyState.Fighting:
                {
                    animator.SetBool("Running", false);

                    break;
                }

        }
    }

}
