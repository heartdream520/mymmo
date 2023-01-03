using System.Collections;
using UnityEngine;
using Services;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
//using Managers;

public class LoadingManager : MonoBehaviour {

    [Header("抵制不良游戏提示")]
    public GameObject UITips;
    [Header("开始界面")]
    public GameObject Start_Panel;
    [Header("加载界面")]
    public GameObject UILoading;
    [Header("登录界面")]
    public GameObject UILogin;
    /*
    [Header("进度条")]
    public GameObject progressBar;

    [Header("进度text")]
    public Text progressText;
    
    [Header("进度num")]
    public Text progressNumber;
    */
    // Use this for initialization
    IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        UITips.SetActive(true);
        Start_Panel.SetActive(true);
        UILoading.SetActive(true);
        UILogin.SetActive(false);
        yield return new WaitForSeconds(4f);
        Start_Panel.SetActive(false);
        /*
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);
        */

        //加载游戏数据

        yield return DataManager.Instance.LoadData();

        //Init basic services

        MapService.Instance.Init();
        UserService.Instance.Init();

        TestManager.Instance.Init();
        BagService.Instance.Init();
        ShopManager.Instance.init();
        ItemServicer.Instance.Init();
        StatusServicer.Instance.Init();
        //FriendService.Instance.Init();
        //TeamService.Instance.Init();
        //GuildService.Instance.Init();
        //ShopManager.Instance.Init();
        //ChatService.Instance.Init();
        //SoundManager.Instance.PlayMusic(SoundDefine.Music_Login);
        // Fake Loading Simulate
        for (float i = 0; i < 1;)
        {
            i += Random.Range(0.01f, 0.02f);
            //progressBar.value = i;
            if (i > 1f) i = 1f;
            loading_bar.Instance.Progress_Num_Float = i;
            //等待帧结束,等待直到所有的摄像机和GUI被渲染完成后，在该帧显示在屏幕之前执行
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
