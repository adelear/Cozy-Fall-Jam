using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyPickup : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioClip pickupSFX;
    [SerializeField] private ParticleSystem pickupVFX;


    [Header("Properties")]
    [SerializeField] private int minNum;
    [SerializeField] private int maxNum;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>(); 
            int randomNumber = Random.Range(minNum, maxNum);
            player.AddCandy(randomNumber);

            if (pickupSFX != null)
                AudioManager.Instance.PlayAudioSFX(pickupSFX);

            if (pickupVFX != null)
                Instantiate(pickupVFX, transform.position, Quaternion.identity);
        } 
        Destroy(gameObject);  
    }
}
// Ready