using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazardBase : MonoBehaviour
{
    [SerializeField] public int candyCost = 0;
    [SerializeField] public float coolDown = 0;
    [SerializeField] public float timeAtHit = 0;

    public virtual void Awake()
    {
        Debug.Log("hazard Awake : " + gameObject.name);
        timeAtHit = -coolDown;
    }

    public virtual void OnPlayerHit(PlayerController player)
    {
        if (!player) return;

        if (Time.time > timeAtHit + coolDown)
        {
            Debug.Log("Taking " + candyCost +  " candy from player via " + gameObject.name);
            player.LoseCandy(candyCost);
        }
    }

    public virtual void OnHazardSpawn()
    {
        Debug.Log("hazard has been spawned : " + gameObject.name);
    }

    public virtual void OnHazardDestroy()
    {
        Debug.Log("hazard has been destroyed : " + gameObject.name);
    }
}
