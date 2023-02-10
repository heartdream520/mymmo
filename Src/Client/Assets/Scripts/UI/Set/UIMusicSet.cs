using System;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Set;
using UnityEngine;
using UnityEngine.UI;

public class UIMusicSet : UIWindow
{


    public GameObject BgImage;
    public GameObject UIImage;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    private void Start()
    {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
    }
    public override void OnClick_Yes()
    {
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        PlayerPrefs.Save();
        base.OnClick_Yes();
    }
    public void OnBgChange(bool isOn)
    {
        isOn = this.toggleMusic.isOn;
        BgImage.SetActive(!isOn);
        Config.MusicOn = isOn;
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
    }
    public void OnUIChange(bool isOn)
    {
        isOn = this.toggleSound.isOn;
        UIImage.SetActive(!isOn);
        Config.SoundOn = isOn;
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
    }
    public void OnBgMusicValueChange(float value)
    {
        if(!Config.MusicOn)
        {
            this.sliderMusic.value = Config.MusicVolume;
            return;
        }
        Config.MusicVolume = (int)this.sliderMusic.value;
        //this.PlaySound();
    }
    public void OnUIMusicValueChange(float value)
    {
        if(!Config.SoundOn)
        {
            this.sliderSound.value = Config.SoundVolume;
            return;
        }
        Config.SoundVolume = (int)this.sliderSound.value;
        this.PlaySound();
    }
    float lastPlayTime = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlayTime > 0.1f)
        {
            this.lastPlayTime = Time.realtimeSinceStartup;
            SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        }
    }
}
