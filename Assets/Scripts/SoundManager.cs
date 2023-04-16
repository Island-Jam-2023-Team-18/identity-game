using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource fxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip passClip;
    [SerializeField] private AudioClip denyClip;

    public void Play()
    {
        fxSource.clip = playClip;
        fxSource.Play();
    }

    public void StartDay()
    {
        fxSource.clip = startClip;
        fxSource.Play();
    }

    public void Pass()
    {
        fxSource.clip = passClip;
        fxSource.Play();
    }

    public void Deny()
    {
        fxSource.clip = denyClip;
        fxSource.Play();
    }
}
