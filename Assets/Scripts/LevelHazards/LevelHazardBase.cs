using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazardBase : MonoBehaviour
{
    [SerializeField] public int candyCost = 0;
    [SerializeField] public float coolDown = 0;

    public virtual void Awake()
    {
        Debug.Log("hazard Awake : " + gameObject.name);
    }

    public virtual void OnPlayerHit()
    {
        Debug.Log("we hit the player - taking candy of value : " + candyCost);
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
