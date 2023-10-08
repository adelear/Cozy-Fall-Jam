using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyHazard : LevelHazardBase
{
    /// <summary>
    /// If true - we will add candy to player instead of taking.
    /// </summary>
    [SerializeField] bool isGoodHazard = false;
    [SerializeField] SpriteRenderer sprite;

    public override void Awake()
    {
        sprite = transform.GetComponent<SpriteRenderer>();

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
        

        if (!player) return;

        if (Time.time > timeAtHit + coolDown)
        {
            Debug.Log("Taking " + candyCost + " candy from player via " + gameObject.name);

            int candyValue = UnityEngine.Random.Range(2, candyCost);

            timeAtHit = Time.time;

            if (isGoodHazard)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("PopupText");
                PopupTextManager ptm = temp.GetComponent<PopupTextManager>();
                if (ptm) ptm.DisplayCandyPopupAtLocation(new Vector3(transform.position.x, 4.25f, transform.position.z), candyValue);

                player.AddCandy(candyValue);
            }
            else
                player.LoseCandy(candyValue);

            StartCoroutine(lifeTimeCoroutine());
        }
    }

    IEnumerator lifeTimeCoroutine()
    {
        //gameObject.SetActive(false);

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        yield return new WaitForSeconds(coolDown);

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);

        //gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag != "Player") return;

        PlayerController pc = c.transform.GetComponent<PlayerController>();

        OnPlayerHit(pc);
    }
}
