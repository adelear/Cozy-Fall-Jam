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

    private Vector3 followVel;

    private void Awake()
    {
        if (followTarget == null)
            followTarget = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -distance);
        Vector3 lookahead = followTarget.GetInputVector() * lookaheadDistance;

        Vector3 final = targetPosition + lookahead;

        transform.position = Vector3.SmoothDamp(transform.position, final, ref followVel, cameraSmoothTime);
    }

}
