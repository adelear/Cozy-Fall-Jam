using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotHoleHazard : LevelHazardBase
{
    public override void Awake()
    {
        base.Awake();
    }

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
        base.OnPlayerHit(player);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag != "Player") return;

        PlayerController pc = c.transform.GetComponent<PlayerController>();

        OnPlayerHit(pc);
    }
}
