using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameService
{
    AudioSource _audioSource;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip) 
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void OnEnable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Register(this);
    }

    private void OnDisable()
    {
        if (ServiceLocator.Current != null)
            ServiceLocator.Current.Unregister(this);
    }
}
