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
  [SerializeField] private AudioClip rulesHideClip;
  [SerializeField] private AudioClip pauseClip;
  [SerializeField] private AudioClip exitClip;
  [SerializeField] private AudioClip validClip;
  [SerializeField] private AudioClip notValidClip;
  [SerializeField] private AudioClip trustSlideClip;
  [SerializeField] private AudioClip hiScoreClip;
  [SerializeField] private AudioClip backGroundClip;
  [SerializeField] private AudioClip gameMusicClip;
  [SerializeField] private AudioClip endDayPositiveClip;
  [SerializeField] private AudioClip endDayNegativeClip;
  [SerializeField] private AudioClip menuMusicClip;
  [SerializeField] private AudioClip endGameMusic;

  public void PlayEndGameMusic()
  {
    musicSource.enabled = true;
    musicSource.clip = endGameMusic;
    musicSource.loop = false;
    musicSource.Play();
  }

  public void PlayGameMusic()
  {
    musicSource.enabled = true;
    musicSource.clip = gameMusicClip;
    musicSource.loop = true;
    musicSource.Play();
  }

  public void PlayEndDayPositiveMusic()
  {
    musicSource.enabled = true;
    musicSource.clip = endDayPositiveClip;
    musicSource.loop = false;
    musicSource.Play();
  }

  public void PlayEndDayNegativeMusic()
  {
    musicSource.enabled = true;
    musicSource.clip = endDayNegativeClip;
    musicSource.loop = false;
    musicSource.Play();
  }

  public void PlayMenuMusic()
  {
    musicSource.enabled = true;
    musicSource.clip = menuMusicClip;
    musicSource.loop = true;
    musicSource.Play();
  }

  public void StopMusic()
  {
    musicSource.enabled = true;
    musicSource.Stop();
  }

  public void StartBackgroundNoise()
  {
    backgroundSource.enabled = true;
    backgroundSource.clip = backGroundClip;
    backgroundSource.loop = true;
    backgroundSource.Play();
  }

  public void RulesHide()
  {
    fxSource.enabled = true;
    fxSource.clip = rulesHideClip;
    fxSource.Play();
  }

  public void StopBackgroundNoise()
  {
    backgroundSource.enabled = true;
    backgroundSource.Stop();
  }

  public void TrustSlide()
  {
    fxSource.enabled = true;
    fxSource.clip = trustSlideClip;
    fxSource.Play();
  }

  public void HiScore()
  {
    fxSource.enabled = true;
    fxSource.clip = hiScoreClip;
    fxSource.Play();
  }


  public void ShowRules()
  {
    fxSource.enabled = true;
    fxSource.clip = rulesShowClip;
    fxSource.Play();
  }

  public void Pause()
  {
    fxSource.enabled = true;
    fxSource.clip = pauseClip;
    fxSource.Play();
  }

  public void Exit()
  {
    fxSource.enabled = true;
    fxSource.clip = exitClip;
    fxSource.Play();
  }

  public void Valid()
  {
    fxSource.enabled = true;
    fxSource.clip = validClip;
    fxSource.Play();
  }

  public void NotValid()
  {
    fxSource.enabled = true;
    fxSource.clip = notValidClip;
    fxSource.Play();
  }

  public void Play()
  {
    fxSource.enabled = true;
    fxSource.clip = playClip;
    fxSource.Play();
  }

  public void StartDay()
  {
    fxSource.enabled = true;
    fxSource.clip = startClip;
    fxSource.Play();
  }

  public void Pass()
  {
    fxSource.enabled = true;
    fxSource.clip = passClip;
    fxSource.Play();
  }

  public void Deny()
  {
    fxSource.enabled = true;
    fxSource.clip = denyClip;
    fxSource.Play();
  }
}
