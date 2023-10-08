using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject playerRef;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] private float bullySpeed = 4f;
    [SerializeField] private int maxCandyToLoose = 5;
    [SerializeField] private float radius = 15f;
    [Range(0, 360)] [SerializeField] private float angle = 360f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float attackRate = 5f;
    [SerializeField] private float rangeOfAttack = 1f;
    [SerializeField] private float patrolDistThreshhold = 0.9f;
    [SerializeField] private float waitAtPatrolPoint = 3f;
    [SerializeField] private float knockbackForce = 50f;
    [SerializeField] private float obstacleOvoidanceRadius = 1f;
    [SerializeField] private float obstacleOvoidanceDistance = 2f; 
    [SerializeField] private AudioClip takeCandy; 
     
    [Header("Layers Mask")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstructionLayerMask;
    [SerializeField] private LayerMask obstacleToAvoid;

    private Animator anim; 
    private int currentPatrolPointIndex;
    private bool canSeePlayer;
    private bool shouldFollowPlayer = true;
    private Transform player;
    private Rigidbody rb;
    private float distanceToPlayer;
    private Vector3 directionToPlayer;
    private PlayerController playerController;
    private float lastAttackTime = 0f;

    private EnemyState bullyState;

    private void Start()
    {
        if (playerRef)
        {
            player = playerRef.transform;
            playerController = playerRef.GetComponent<PlayerController>();
        }

        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (anim == null) anim = GetComponent<Animator>();  
        if (rb == null) rb = GetComponent<Rigidbody>();
        canSeePlayer = false;

        SwitchState(EnemyState.PATROL);
      
        InvokeRepeating("FieldOfViewCheck", 0f, 0.2f); // Check FOV every 0.2 seconds
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        directionToPlayer = (player.position - transform.position).normalized;
       
        HandleStateMachine();
        FieldOfViewCheck();
        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, canSeePlayer ? Color.green : Color.red);
        Debug.Log("EnemyState: " + bullyState);
    }

    public void SwitchState(EnemyState newState)
    {
        bullyState = newState;
    }

    // Bully StateMachine
    public void HandleStateMachine()
    {
        switch (bullyState)
        {
            case EnemyState.IDLE:
                Idle();
                break;

            case EnemyState.PATROL:
                Patrol();
                break;

            case EnemyState.CHASE:
                ChasePlayer();
                break;

            case EnemyState.ATTACK:
                break;
        }
    }

    // Checks if player is in field of view
    private void FieldOfViewCheck()
    {
        // Check for colitions in the radius of sphere cast
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerLayerMask);

        // If there is at least 1 collition; player is in field of view
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;

            // If target is in the sector of field of view
            if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
            {
                RaycastHit hit;

                //LayerMask layerMask = (bullyState == EnemyState.CHASE) ? default : obstructionLayerMask;

                if (distanceToPlayer <= maxDistance && !Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer, obstructionLayerMask))
                {
                    canSeePlayer = true;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else
            canSeePlayer = false;
    }

    // Idle State
    private void Idle()
    {
        if (canSeePlayer)
        {
            SwitchState(EnemyState.CHASE);
            anim.SetBool("Moving", true);
        }
        else 
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("Moving", false);
        }
    }

    // Chase State
    private void ChasePlayer()
    {
        if (canSeePlayer && shouldFollowPlayer)
        {
            // Movement direction
            Vector3 movementDirection = directionToPlayer * bullySpeed;
            movementDirection.y = 0;  
            // Flip the sprite renderer based on the movement direction
            FlipSprite(movementDirection);

            // Check if the player is within the field of view
            if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
            {
                RaycastHit obstacleHit;
                if (!Physics.SphereCast(transform.position, obstacleOvoidanceRadius, directionToPlayer, out obstacleHit, obstacleOvoidanceDistance, obstacleToAvoid))
                {
                    // Continue chasing the player as long as they are within the field of view
                    rb.velocity = movementDirection; 

                    // Bully Gets candy from player
                    if (distanceToPlayer <= rangeOfAttack)
                    {
                        if (Time.time - lastAttackTime >= attackRate)
                        {
                            int candyToLoose = Random.Range(1, maxCandyToLoose);
                            // Player loose canddy
                            playerController.LoseCandy(candyToLoose);
                            AudioManager.Instance.PlayAudioSFX(takeCandy); 
                            anim.SetTrigger("Eating");
                            // Player knockback
                            Rigidbody playerRigidbody = playerController.GetComponent<Rigidbody>();
                            Vector3 knockbackDirection = (playerController.transform.position - transform.position).normalized;
                            playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                            lastAttackTime = Time.time;

                            // Stop following the player after stealing candy
                            shouldFollowPlayer = false;
                            StartCoroutine(ResumeFollowingPlayer());
                        }
                    }
                }
                else
                {
                    // Obstacle detected, calculate a new direction to avoid it
                    Vector3 hitNormal = obstacleHit.normal;
                    Vector3 newDirection = Vector3.Cross(hitNormal, Vector3.up);
                    rb.velocity = newDirection.normalized * bullySpeed;
                }
            }
            else
            {
                SwitchState(EnemyState.PATROL);
            }
        }
        else
        {
            SwitchState(EnemyState.PATROL);
        }
    }

    // Coroutine to resume following the player after a delay
    private IEnumerator ResumeFollowingPlayer()
    {
        yield return new WaitForSeconds(attackRate);
        shouldFollowPlayer = true; // Resume following the player
    }

    // Patrol State
    private void Patrol()
    {
        if (!canSeePlayer) 
        {
            // Direction and distance to the current patrol point
            anim.SetBool("Moving", true); 
            Vector3 targetDirection = (patrolPoints[currentPatrolPointIndex].position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position);

            //  If enemy reached the current patrol point switch to next patrol point
            if (distanceToTarget <= patrolDistThreshhold)
            {
                // Start waiting at the current patrol point
                StartCoroutine(WaitAtPatrolPoint());

                // Move to the next patrol point
                currentPatrolPointIndex++;
                currentPatrolPointIndex %= patrolPoints.Length;
                SwitchState(EnemyState.IDLE);

                // Reset the SpriteRenderer flip to the default state when patrolling
                spriteRenderer.flipX = false;
            }
            else
            {
                rb.velocity = targetDirection * bullySpeed;
                FlipSprite(targetDirection);
            }
        }
        if (canSeePlayer) 
        {
            if (bullyState != EnemyState.CHASE)
            {
                SwitchState(EnemyState.CHASE);
            }
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        yield return new WaitForSeconds(waitAtPatrolPoint);

        // Resume patrolling
        if (canSeePlayer)
        {
            SwitchState(EnemyState.CHASE);
        }
        else
            SwitchState(EnemyState.PATROL);
    }

    // Flip the SpriteRenderer based on the movement direction
    private void FlipSprite(Vector3 direction)
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public enum EnemyState
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
