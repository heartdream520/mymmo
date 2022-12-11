using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class lode_scene : MonoBehaviour
{
    //加载的场景名
    private string lodeing_scene;
    private AsyncOperation async = null;

    private void Start()
    {
        lodeing_scene = GameDataManager.GetString("lodeing_scene");
        async = SceneManager.LoadSceneAsync(lodeing_scene);
        async.allowSceneActivation = false;
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 1;)
        {
            i += Random.Range(0.005f, 0.01f);
            //progressBar.value = i;
            if (i > 1f) i = 1f;
            loading_bar.Instance.Progress_Num_Float = i;
            //等待帧结束,等待直到所有的摄像机和GUI被渲染完成后，在该帧显示在屏幕之前执行
            if (i >= 1) yield return null;
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;
        yield return null;

    }
}