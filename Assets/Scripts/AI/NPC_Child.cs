using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Child : MonoBehaviour
{
    [SerializeField] float distThreshold = 1;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float randomFlipChance = 0.1f;

    [SerializeField] int pathIndx = 0;
    [SerializeField] bool GenerateRandom = false;


    public List<Transform> pathNodes = new List<Transform>();

    private void Start()
    {
        Transform temp = GameObject.FindGameObjectWithTag("PatrolRoute").transform;

        for (int i = 0; i < temp.childCount; i++)
        {
            pathNodes.Add(temp.GetChild(i));
        }

        //transform.position = pathNodes[pathIndx].position;
        SelectRandomNode();
    }

    private void Update()
    {
        DistanceCheck();

        Vector3 moveDir = pathNodes[pathIndx].position - transform.position;
        moveDir.Normalize();

        transform.position = transform.position + (moveDir * moveSpeed) * Time.deltaTime;

        if (GenerateRandom)
            SelectRandomNode();
    }

    private void FlipPath(bool resetIndx)
    {
        if (resetIndx)
            pathIndx = 0;
        
        pathNodes.Reverse();
    }

    private void SelectRandomNode()
    {
        if (GenerateRandom) GenerateRandom = false;

        pathIndx = Random.Range(0, pathNodes.Count - 1);

        transform.position = pathNodes[pathIndx].position;
    }

    private void DistanceCheck()
    {
        if (Vector3.Distance(transform.position, pathNodes[pathIndx].position) < distThreshold )
        {
            pathIndx = (pathIndx + 1) % pathNodes.Count;
            //pathIndx++;

            //TryRandomFlip();

            if (pathIndx >= pathNodes.Count - 1)
                FlipPath(true);
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
