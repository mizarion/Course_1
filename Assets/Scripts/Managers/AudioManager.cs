using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
#pragma warning disable 649

    [SerializeField] AudioClip[] Clips;

#pragma warning restore 649

    public AudioSource AudioSource => GetComponent<AudioSource>();

    MyAudioClips CurrentClip;

    private void FixedUpdate()
    {
        if (!AudioSource.isPlaying)
        {
            PlayNextClip();
        }
    }
    
    /// <summary>
    /// Проигрывает следующий клип
    /// </summary>
    public void PlayNextClip()
    {
        AudioSource.clip = Clips[(int)CurrentClip];
        AudioSource.PlayDelayed(1);
        AudioSource.Play();
        CurrentClip = CurrentClip == MyAudioClips.LastClip ? MyAudioClips.StartClip : CurrentClip + 1;
    }

    /// <summary>
    /// Перечисление клипов
    /// </summary>
    public enum MyAudioClips
    {
        StartClip,     // Winds of Winter 
        RedSwan,
        HiroyukiSavano,
        LastClip
    }
}


