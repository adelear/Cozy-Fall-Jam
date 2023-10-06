using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
   
    [Header("Settings")]
    [SerializeField] private float bullySpeed;
    [SerializeField] private int maxCandyToLoose = 5;
    [SerializeField] private float radius;
    [Range(0, 360)] [SerializeField] private float angle;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstructionLayerMask;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float attackRate = 5f;
    [SerializeField] private float rangeOfAttack = 1f;


    private bool canSeePlayer;
    private bool shouldFollowPlayer = true;
    private Transform player;
    private Rigidbody rb;
    private float distanceToPlayer;
    private Vector3 directionToPlayer;
    private PlayerController playerController;
    private float lastAttackTime = 0f;


    void Start()
    {
        if (playerRef)
        {
            player = playerRef.transform;
            playerController = playerRef.GetComponent<PlayerController>();
        }

        rb = GetComponent<Rigidbody>();
        canSeePlayer = false;

        InvokeRepeating("FieldOfViewCheck", 0f, 0.2f); // Check FOV every 0.2 seconds
    }

    void Update()
    {

        if (!player || !playerController) { return; }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        directionToPlayer = (player.position - transform.position).normalized;

        if (canSeePlayer && shouldFollowPlayer) 
        {
            rb.velocity = directionToPlayer * bullySpeed;

            // Bully Gets candy from player
            if (distanceToPlayer <= rangeOfAttack)
            {
                if (Time.time - lastAttackTime >= attackRate)
                {
                    int candyToLoose = Random.Range(1, maxCandyToLoose);
                    playerController.LoseCandy(candyToLoose);

                    lastAttackTime = Time.time;

                    // Stop following the player after steling candy
                    shouldFollowPlayer = false;
                    StartCoroutine(ResumeFollowingPlayer());
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
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
            //Vector3 directionToTarget = (target.position - transform.position).normalized;

            // If target is in the sector of field of view
            if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
            {
                //float distancetoTarget = Vector3.Distance(transform.position, target.position);

                if (distanceToPlayer <= maxDistance && !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayerMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else
            canSeePlayer = false;
    }

    // Coroutine to resume following the player after a delay
    private IEnumerator ResumeFollowingPlayer()
    {
        yield return new WaitForSeconds(attackRate); // Wait for attackRate seconds
        shouldFollowPlayer = true; // Resume following the player
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
