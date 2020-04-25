using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioClip[] Clips;

    AudioSource _audioSource;

    MyAudioClips CurrentClip;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayClips(MyAudioClips.StartClip);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            //PlayClips(MyAudioClips.secondClip);
            PlayNextClip();
        }
        //if (!_audioSource.isPlaying)
        //{
        //    PlayNextClip();
        //}
    }

    private void PlayNextClip()
    {
        // Todo: Добавить ... логики?
        if (CurrentClip == MyAudioClips.LastClip)
        {
            CurrentClip = MyAudioClips.StartClip;
        }
        else
        {
            CurrentClip++;
        }
        _audioSource.clip = Clips[(int)CurrentClip];
        _audioSource.PlayDelayed(1);

    }

    public void PlayClips(MyAudioClips audioClip)
    {
        int i = (int)audioClip;
        //if (_audioSource.isPlaying)
        //{
        //_audioSource.PlayDelayed(10);
        Debug.Log("play");
        //}
        //else
        //{
        // Играет не зависимо от чего (использовать для единоразовых звуков)
        _audioSource.PlayOneShot(Clips[i]);
        //}
    }


    public enum MyAudioClips
    {
        StartClip,
        secondClip,
        LastClip
    }
}


