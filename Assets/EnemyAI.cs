using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] waypoints;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float waitTime = 2f;

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

    int currentWaypoint = 0;
    float waitTimer = 0f;
    float attackTimer = 0f;
    Vector3 lastPlayerPos;

    bool isChasing = false;
    bool isAttacking = false;
    bool isSearching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;

        if (waypoints.Length == 0)
            Debug.LogWarning("No waypoints set for " + name);
    }

    void Update()
    {
        if (player == null) return;

        float playerDist = Vector3.Distance(transform.position, player.position);

        // Check if we can see player
        bool canSeePlayer = CanSeePlayer();

        // State logic
        if (!isChasing && !isSearching && canSeePlayer && playerDist <= detectRange)
        {
            // Start chasing
            isChasing = true;
            lastPlayerPos = player.position;
        }
        else if (isChasing && (playerDist > loseRange || !canSeePlayer))
        {
            // Lost player, start searching
            isChasing = false;
            isSearching = true;
            waitTimer = 3f;
        }
        else if (isChasing && playerDist <= attackRange)
        {
            // Close enough to attack
            isAttacking = true;
        }
        else if (isAttacking && playerDist > attackRange)
        {
            // Player moved away
            isAttacking = false;
        }

        // Movement and actions
        if (isAttacking)
        {
            DoAttack();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else if (isSearching)
        {
            SearchForPlayer();
        }
        else
        {
            Patrol();
        }

        UpdateAnimations();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist < 1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                    currentWaypoint = 0;
            }
        }
        else
        {
            MoveToPosition(target.position, walkSpeed);
        }
    }

    void ChasePlayer()
    {
        MoveToPosition(player.position, runSpeed);
        lastPlayerPos = player.position;
    }

    void DoAttack()
    {
        // Look at player
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0f;

            // Deal damage
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Take_Dmg(attackDamage);
            }

            if (anim != null)
                anim.SetTrigger("Attack");

            Debug.Log(name + " attacked player for " + attackDamage + " damage");
        }
    }

    void SearchForPlayer()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer > 0)
        {
            MoveToPosition(lastPlayerPos, walkSpeed);
        }
        else
        {
            // Give up searching, go back to patrol
            isSearching = false;
            FindClosestWaypoint();
        }
    }

    void MoveToPosition(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), Time.deltaTime * 4f);

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

    void FindClosestWaypoint()
    {
        if (waypoints.Length == 0) return;

        float closestDist = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, waypoints[i].position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        currentWaypoint = closestIndex;
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        bool isMoving = !isAttacking && (isChasing || isSearching ||
            (waypoints.Length > 0 && Vector3.Distance(transform.position, waypoints[currentWaypoint].position) > 1f));

        anim.SetBool("Walking", isMoving && !isChasing);
        anim.SetBool("Running", isMoving && isChasing);
    }

    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Waypoint path
        if (waypoints != null && waypoints.Length > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    Gizmos.DrawSphere(waypoints[i].position, 0.3f);

                    int next = (i + 1) % waypoints.Length;
                    if (waypoints[next] != null)
                        Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
                }
            }
        }
    }
}