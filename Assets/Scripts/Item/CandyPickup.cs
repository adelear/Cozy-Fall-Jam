using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    [SerializeField] private int minNum;
    [SerializeField] private int maxNum;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>(); 
            int randomNumber = Random.Range(minNum, maxNum);
            player.AddCandy(randomNumber);
            Debug.Log(player.GetCurrentCandy() + " Candy Picked Up"); 
        } 
        Destroy(gameObject);  
    }
}
