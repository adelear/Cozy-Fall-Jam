using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Child : MonoBehaviour
{
    [SerializeField] float distThreshold = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float randomFlipChance = 0.1f;

    [SerializeField] int pathIndx = 0;

    //[SerializeField] bool GenerateRandom = false;


    public List<Transform> pathNodes = new List<Transform>();

    private void Start()
    {
        if (pathNodes.Count > 0)
        {
            pathIndx = Random.Range(0, pathNodes.Count);
            transform.position = pathNodes[pathIndx].position;
        }
    }

    private void Update()
    {
        DistanceCheck();

        Vector3 moveDir = pathNodes[pathIndx].position - transform.position;
        moveDir.Normalize();

        transform.position = transform.position + (moveDir * moveSpeed) * Time.deltaTime;
    } 

    private void FlipPath(bool resetIndx)
    {
        if (resetIndx)
            pathIndx = 0;
        pathNodes.Reverse();
        
    }

    private void DistanceCheck()
    {
        if (Vector3.Distance(transform.position, pathNodes[pathIndx].position) < distThreshold)
        {
            pathIndx = (pathIndx + 1) % pathNodes.Count;

            if (pathIndx == 0)
            {
                // If you've reached the end of the path, reset the path and flip the order.
                pathNodes.Reverse();
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX; 
                pathIndx = 0;
            }
        }
    } 


    private void TryRandomFlip()
    {
        float randNum = Random.Range(0, 100) / 100;

        if (randNum < randomFlipChance)
        {
            Debug.Log("We hit randomPathFlip");

            FlipPath(false);
        }
    }
}
