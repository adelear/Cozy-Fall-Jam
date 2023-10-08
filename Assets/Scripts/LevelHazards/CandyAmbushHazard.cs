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

            StartCoroutine(CooldownLifetime());
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

            tempBully.transform.position += new Vector3(newPos.x, 0, newPos.z);

            FollowPlayer bully = tempBully.GetComponent<FollowPlayer>();

            bully.SetPatrolRotues(GeneratePatrolRoute());

        }

         
    }
    IEnumerator CooldownLifetime()
    {
        SpriteRenderer tempSprite = transform.GetComponent<SpriteRenderer>();

        if (tempSprite) tempSprite.color = new Color(tempSprite.color.r, tempSprite.color.g, tempSprite.color.b, 0);

        yield return new WaitForSeconds(coolDown);

        if (tempSprite) tempSprite.color = new Color(tempSprite.color.r, tempSprite.color.g, tempSprite.color.b, 1);
    }

    private List<Transform> GeneratePatrolRoute()
    {
        List<Transform> newRoute = new List<Transform>(4);

        for (int i =0; i < transform.childCount; i++)
        {
            newRoute.Add(transform.GetChild(0));
        }

        return newRoute;
    }

}
