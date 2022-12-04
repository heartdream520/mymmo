﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using SkillBridge.Message;
using ProtoBuf;
using Services;
//using Managers;

public class LoadingManager : MonoBehaviour {

    [Header("抵制不良游戏提示")]
    public GameObject UITips;
    [Header("加载界面")]
    public GameObject UILoading;
    [Header("登录界面")]
    public GameObject UILogin;
    [Header("进度条")]
    public Slider progressBar;
    [Header("进度text")]
    public Text progressText;
    [Header("进度num")]
    public Text progressNumber;

    // Use this for initialization
    IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        UITips.SetActive(true);
        UILoading.SetActive(false);
        UILogin.SetActive(false);
        yield return new WaitForSeconds(2f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);

        //yield return DataManager.Instance.LoadData();

        //Init basic services

        //MapService.Instance.Init();
        UserService.Instance.Init();
        //StatusService.Instance.Init();
        //FriendService.Instance.Init();
        //TeamService.Instance.Init();
        //GuildService.Instance.Init();
        //ShopManager.Instance.Init();
        //ChatService.Instance.Init();
        //SoundManager.Instance.PlayMusic(SoundDefine.Music_Login);
        // Fake Loading Simulate
        for (float i = 50; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progressBar.value = i;
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
    }


    // Update is called once per frame
    void Update () {

    }
}
