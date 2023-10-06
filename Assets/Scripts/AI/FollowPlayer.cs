using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    [SerializeField] private bool canSeePlayer;
    private Transform player;
    private Rigidbody rb;
    private float distanceToPlayer;

    [Header("Settings")]
    [SerializeField] private float bullySpeed;
    [SerializeField] private float radius;
    [Range(0, 360)] [SerializeField] private float angle;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstructionLayerMask;
    [SerializeField] private float maxDistance = 20f;

    // [SerializeField] private float attackrate = 1.5f;
    // [SerializeField] private float rangeOfAttack;


    void Start()
    {
        if (playerRef)
            player = playerRef.transform;

        rb = GetComponent<Rigidbody>();
        canSeePlayer = false;

        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        if (canSeePlayer) 
        {
            Vector3 velocity = (player.position - transform.position) * bullySpeed;

            rb.velocity = velocity;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }

    // Runs FieldOfViewCheck() for performance 
    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
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
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // If target is in the sector of field of view
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                distanceToPlayer = Vector3.Distance(transform.position, target.position);

                if (distanceToPlayer <= maxDistance && !Physics.Raycast(transform.position, directionToTarget, distanceToPlayer, obstructionLayerMask))
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
