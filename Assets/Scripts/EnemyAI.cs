using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float waitTime = 2f;
    public float wanderRadius = 10f;

    [Header("Detection")]
    public float detectRange = 8f;
    public float loseRange = 12f;
    public LayerMask obstacleLayer = -1;

    [Header("Combat")]
    public float attackRange = 2.5f;
    public float attackDamage = 25f;
    public float attackDelay = 1.8f;

    Transform player;
    CharacterController controller;
    Animator anim;

    Vector3 wanderTarget;
    float waitTimer = 0f;
    float attackTimer = 0f;
    Vector3 lastPlayerPos;

    bool isChasing = false;
    bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;

        PickNewWanderPoint();
    }

    void Update()
    {
        if (player == null) return;

        float playerDist = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();

        // State logic
        if (!isChasing && canSeePlayer && playerDist <= detectRange)
        {
            isChasing = true;
            lastPlayerPos = player.position;
        }
        else if (isChasing && (playerDist > loseRange || !canSeePlayer))
        {
            isChasing = false;
            PickNewWanderPoint();
        }

        if (isChasing && playerDist <= attackRange)
            isAttacking = true;
        else
            isAttacking = false;

        // Movement / actions
        if (isAttacking)
            DoAttack();
        else if (isChasing)
            ChasePlayer();
        else
            Wander();

        UpdateAnimations();
    }

    void Wander()
    {
        float dist = Vector3.Distance(transform.position, wanderTarget);

        if (dist < 1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                PickNewWanderPoint();
            }
        }
        else
        {
            MoveToPosition(wanderTarget, walkSpeed);
        }
    }

    void PickNewWanderPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(
            transform.position.x + randomCircle.x,
            transform.position.y,
            transform.position.z + randomCircle.y
        );
    }

    void ChasePlayer()
    {
        MoveToPosition(player.position, runSpeed);
        lastPlayerPos = player.position;
    }

    void DoAttack()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;

            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.Take_Dmg(attackDamage);

            if (anim != null)
                anim.SetTrigger("Attack");

            Debug.Log(name + " attacked player for " + attackDamage + " damage");
        }
    }

    void MoveToPosition(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 4f
            );

            Vector3 movement = direction * speed * Time.deltaTime;

            if (controller != null)
            {
                movement.y = -10f * Time.deltaTime; // gravity
                controller.Move(movement);
            }
            else
            {
                transform.position += movement;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(rayStart, dirToPlayer, out hit, distToPlayer, obstacleLayer))
        {
            if (hit.transform != player)
                return false;
        }

        return true;
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        bool isMoving = !isAttacking && (isChasing || Vector3.Distance(transform.position, wanderTarget) > 1f);
        anim.SetBool("Walking", isMoving && !isChasing);
        anim.SetBool("Running", isMoving && isChasing);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
