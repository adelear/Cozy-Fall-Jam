using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerController followTarget;

    [Header("Attributes")]
    [SerializeField] private float distance = 10f;
    [SerializeField] private float lookaheadDistance = 1.5f;
    [SerializeField] private float cameraSmoothTime = 0.3f;
    [SerializeField] private float yOffset = 1.5f;

    private Vector3 followVel;

    private void Awake()
    {
        if (followTarget == null)
            followTarget = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        
        float zPos = 0;

        //Vector3 dirToPlayer = transform.position - followTarget.transform.position;
        //Ray ray = new Ray(followTarget.transform.position, dirToPlayer.normalized);

        zPos = followTarget.transform.position.z - distance;

        //if (Physics.Raycast(ray, out RaycastHit hit, distance))
        //{
        //    zPos = hit.point.z;
        //}

        //else
        //{
        //    //if (followTarget.isTeleporting) return;

            
        //}

        if (followTarget.isTeleporting) return;

        

        Vector3 targetPosition = new Vector3(followTarget.transform.position.x, yOffset, zPos);
        Vector3 lookahead = new Vector3(followTarget.GetInputVector().x * lookaheadDistance, 0, 0);

        Vector3 final = targetPosition + lookahead;

        transform.position = Vector3.SmoothDamp(transform.position, final, ref followVel, cameraSmoothTime);
    }


   
}
