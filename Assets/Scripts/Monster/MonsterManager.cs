using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private int monsterTreats = 0;
    [SerializeField] private PlayerController playerController;
    private bool playerInCollider;
    private bool playerHasCandy; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            playerInCollider = true; 
            Debug.Log("Standing over monster!"); 
            if (player.GetCurrentCandy() > 0)
            {
                playerHasCandy = true; 
                Debug.Log("You have candy!"); 
            }
            else
            {
                Debug.Log("You have no candy!");
                playerHasCandy = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInCollider = false;
            Debug.Log("Leaving monster area!");
        }
    } 

    public int GetMonsterTreats()
    {
        return monsterTreats; 
    }

    private void Update()
    {
        if (playerInCollider && playerHasCandy) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Feeding the monster!");
                int treatsGiven = playerController.GetCurrentCandy();
                monsterTreats += treatsGiven;
                playerController.LoseCandy(treatsGiven);
            }
        }
    }
}
