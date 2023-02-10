using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MySceneManager : MonoSingleton<MySceneManager>
{
    UnityAction<float> onProgress = null;

    // Use this for initialization
    public override void OnStart()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name,MapDefine map=null)
    {
        GameDataManager.SetString("lodeing_scene",name);
        if(map!=null)
        GameDataManager.SetString("music",map.Music);
        else GameDataManager.SetString("music", "");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Load_Scenes");
        //StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        Debug.Log("LevelLoadCompleted:" + obj.progress);
    }
}
