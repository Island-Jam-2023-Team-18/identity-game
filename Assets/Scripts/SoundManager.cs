using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource fxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource backgroundSource;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip passClip;
    [SerializeField] private AudioClip denyClip;
    [SerializeField] private AudioClip rulesShowClip;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip exitClip;
    [SerializeField] private AudioClip validClip;
    [SerializeField] private AudioClip notValidClip;
    [SerializeField] private AudioClip trustSlideClip;
    [SerializeField] private AudioClip hiScoreClip;
    [SerializeField] private AudioClip backGroundClip;

    public void StartBackgroundNoise()
    {
        backgroundSource.clip = backGroundClip;
        backgroundSource.Play();
    }

    public void StopBackgroundNoise()
    {
        backgroundSource.Stop();
    }

    public void TrustSlide()
    {
        fxSource.clip = trustSlideClip;
        fxSource.Play();
    }

    public void HiScore()
    {
        fxSource.clip = hiScoreClip;
        fxSource.Play();
    }


    public void ShowRules()
    {
        fxSource.clip = rulesShowClip;
        fxSource.Play();
    }

    public void Pause()
    {
        fxSource.clip = pauseClip;
        fxSource.Play();
    }

    public void Exit()
    {
        fxSource.clip = exitClip;
        fxSource.Play();
    }

    public void Valid()
    {
        fxSource.clip = validClip;
        fxSource.Play();
    }

    public void NotValid()
    {
        fxSource.clip = notValidClip;
        fxSource.Play();
    }

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
