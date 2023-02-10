using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager> {

    public AudioMixer audioMixer;
    public AudioSource  musicAudioSource;
    public AudioSource  SoundAudioSource;

    public const string musicPath = "Music/";
    public const string SoundPath = "Sound/";

    private void Start()
    {
        this.MusicMute(Config.MusicOn);
        this.SoundMute(Config.SoundOn);
        this.MusicSetV(Config.MusicVolume);
        this.SoundSetV(Config.SoundVolume);
    }
    private bool musicOn;
    public bool MusicOn
    {
        get
        {
            return this.musicOn;
        }
        set
        {
            this.musicOn = value;
            this.MusicMute(value);
        }
    }
    private bool soundOn;
    public bool SoundOn
    {
        get
        {
            return this.soundOn;
        }
        set
        {
            this.soundOn = value;
            this.SoundMute(value);
        }
    }
    private int musicVolume;
    public int MusicVolume
    {
        get
        {
            return this.musicVolume;
        }
        set
        {
            this.musicVolume = value;
            this.MusicSetV(value);
        }
    }
    private int soundVolume;
    public int SoundVolume
    {
        get
        {
            return this.soundVolume;
        }
        set
        {
            this.soundVolume = value;
            this.SoundSetV(value);
        }
    }
    public void MusicMute(bool on)
    {
        this.MusicSetV("MusicV", on ? this.musicVolume : 0);
    }
    public void SoundMute(bool on)
    {
        this.MusicSetV("SoundV", on ? this.musicVolume : 0);
    }
    public void MusicSetV(int value)
    {
        this.MusicSetV("MusicV", value);
    }
    public void SoundSetV(int value)
    {
        this.MusicSetV("SoundV", value);
    }
    private void MusicSetV(string link, int v)
    {
        float V = v * 0.5f - 50f;
        this.audioMixer.SetFloat(link, V);

    }
    public void PlayerMusic(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(musicPath + name);
        if (clip == null)
        {
            Debug.LogErrorFormat("AudioClip:{0} not exist", musicPath + name);
            return;
        }
        if (this.musicAudioSource.isPlaying)
        {
            this.musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    public void PlayerSound(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(SoundPath + name);
        if(clip==null)
        {
            Debug.LogErrorFormat("AudioClip:{0} not exist", SoundPath + name);
            return;
        }
        if(this.SoundAudioSource.isPlaying)
        {
            this.SoundAudioSource.Stop();
        }
        SoundAudioSource.PlayOneShot(clip);
    }
}
