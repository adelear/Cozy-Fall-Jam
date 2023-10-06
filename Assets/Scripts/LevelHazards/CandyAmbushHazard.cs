using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyAmbushHazard : LevelHazardBase
{
    [SerializeField] float spawnRadius;
    [SerializeField] float spawnCount;
    [SerializeField] float groundPos;
    
    [SerializeField] GameObject bullyGameObject;

    public override void OnHazardSpawn()
    {
        base.OnHazardSpawn();
    }

    public override void OnHazardDestroy()
    {
        base.OnHazardDestroy();
    }

    public override void OnPlayerHit(PlayerController player)
    {
        if (!player) return;

        if (Time.time > timeAtHit + coolDown)
        {
            timeAtHit = Time.time;
            SpawnBullysInRadius();
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag != "Player") return;

        PlayerController pc = c.transform.GetComponent<PlayerController>();

        OnPlayerHit(pc);
    }

    private void SpawnBullysInRadius()
    {
        if (!bullyGameObject) return;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject tempBully = Instantiate(bullyGameObject, transform.position, Quaternion.identity);

            Vector3 newPos = Random.insideUnitSphere * spawnRadius;

            tempBully.transform.position = new Vector3(newPos.x, groundPos, newPos.z);
        }
    }

}
