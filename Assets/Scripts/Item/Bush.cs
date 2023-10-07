
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField] private ParticleSystem leaveVFX;
    [SerializeField] private AudioClip bushEnterFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (bushEnterFX != null)
                AudioManager.Instance.PlayAudioSFX(bushEnterFX);

            leaveVFX.Play();
        }
    }
}