using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMAnager : BaseManager
{
    public AudioMAnager(GameFacade facade) : base(facade)
    {
        
    }

    public const string Sound_Path = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_moderate = "Bg(moderate)";
    public const string Sound_Bg_fast = "Bg(fast)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";
    private AudioSource bgAudioSource;
    private AudioSource noramAudioSource;

    public override void OnInit()
    {
        GameObject audioSocuce = new GameObject("AudioSocure(GameObject)");
        bgAudioSource = audioSocuce.AddComponent<AudioSource>();
        noramAudioSource = audioSocuce.AddComponent<AudioSource>();
        PlaySound(bgAudioSource, LoadSound(Sound_Bg_moderate), 0.5f,true);
    }

    public void PlayBgSound(string sound,float volume = 0.5f)
    {
        PlaySound(bgAudioSource, LoadSound(sound), volume, true);
    }

    public void PlayNoramSound(string sound,float volume,bool loop)
    {
        PlaySound(noramAudioSource, LoadSound(sound), volume, loop);
    }

    private void PlaySound(AudioSource audioSource,AudioClip clip, float volume,bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private AudioClip LoadSound(string sound)
    {
        return Resources.Load<AudioClip>(Sound_Path + sound);
    }
}
