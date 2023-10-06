using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private Queue<AudioSource> twoDAudioPool;

    [SerializeField] private AudioSource twoDTemplate;
    [SerializeField] private int numberOfPool = 15;
    [SerializeField] private AudioMixer mixer;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        twoDAudioPool = new Queue<AudioSource>();

        for (int i = 0; i < numberOfPool; i++)
        {
            AudioSource twoD = Instantiate(twoDTemplate, transform);
            twoDAudioPool.Enqueue(twoD);
        }
    }
    public AudioSource GetTwoDimensionalSource()
    {
        AudioSource source = twoDAudioPool.Dequeue();
        twoDAudioPool.Enqueue(source);
        return source;
    }

    public void PlayAudioSFX(AudioClip clip)
    {
        AudioSource source = GetTwoDimensionalSource();
        source.volume = 1f;
        source.clip = clip;
        source.Play();
    }

    public void PlayAudioSFXAtVol(AudioClip clip, float vol)
    {
        AudioSource source = GetTwoDimensionalSource();
        source.volume = (vol > 1f) ? 1f : vol;
        source.clip = clip;
        source.Play();
    }

    public void SetMixerVolume(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
        mixer.SetFloat(name, Mathf.Log10(value) * 20);
    }
}
