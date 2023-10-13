using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHazard : LevelHazardBase
{
    [SerializeField] float moveSpeed;
    [SerializeField] float distThreshold;

    [Tooltip("The starting position of the car. Car Obj snaps to this pos on begin play.")]
    [SerializeField] Transform StartPos;
    [SerializeField] Transform endPos;

    [SerializeField] AudioClip candyDrop;
    [SerializeField] AudioClip carHit;

    private float moveDuration;
    private float maxMoveDuration;

    private Rigidbody rb;

    private void Start()
    {
        OnHazardSpawn();
        transform.position = StartPos.position; 
    }

    private void FixedUpdate()
    {
        if (!StartPos || !endPos) return;

        moveDuration += Time.deltaTime;

        if (AtFinalPos(distThreshold))
        {
            Debug.Log("at final pos - swapping move tForms");
            Transform tempTform = StartPos;
            StartPos = endPos;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;  
            endPos = tempTform;
            moveDuration = 0;
        }

        Vector3 NewPos = Vector3.Lerp(StartPos.position, endPos.position, moveDuration / maxMoveDuration);

        //transform.LookAt(endPos);
        rb.MovePosition(NewPos);
    }
  

    public override void OnPlayerHit(PlayerController player)
    {
        AudioManager.Instance.PlayAudioSFX(carHit); 
        if (Time.time > timeAtHit + coolDown)
        {
            Debug.Log("hit player - taking candy");

            timeAtHit = Time.time;

            player.LoseCandy(candyCost);
            AudioManager.Instance.PlayAudioSFX(candyDrop); 
        }
    }

    public override void OnHazardSpawn()
    {
        if (!rb)
            rb = transform.GetComponent<Rigidbody>();

        transform.position = StartPos.position;

        maxMoveDuration = Vector3.Distance(StartPos.position, endPos.position) / moveSpeed;

        timeAtHit = 0;
    }

    public override void OnHazardDestroy()
    {
        base.OnHazardDestroy();
    }

    //returns true if we are in distance(distThreshold)
    private bool AtFinalPos(float distThreshold)
    {
        return Vector3.Distance(rb.position, endPos.position) < distThreshold;
    }

    //private void OnCollisionEnter(Collision c)
    //{
    //    if (c.gameObject.tag != "Player") return;

    //    PlayerController pc = c.transform.GetComponent<PlayerController>();

    //    if (pc)
    //        OnPlayerHit(pc);

    //}

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Player") return;

        PlayerController pc = c.transform.GetComponent<PlayerController>();

        if (pc)
            OnPlayerHit(pc);

    }

}
