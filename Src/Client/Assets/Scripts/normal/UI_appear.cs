using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_appear : MonoBehaviour {
    

    public enum start_move_d
    {
        l,r,no
    }
    [Header("初始移动方向")]
    public start_move_d start_move;
    [Header("左边界")]
    public float l_x;
    [Header("右边界")]
    public float r_x;
    [Header("移动速度")]
    public float v;
    /// <summary>
    /// 淡化淡出的速度  2.5f
    /// </summary>
    [Header("出现速度")]
    public float av;
    [Header("移动距离阈值")]
    public float x;

    private bool flag;

    [HideInInspector]
    public UnityAction after_move_r_action;
    [HideInInspector]
    public UnityAction after_move_l_action;
    [HideInInspector]
    public UnityAction start_move_r_action;
    [HideInInspector]
    public UnityAction start_move_l_action;

    public Transform rect_transform;
    private void OnEnable()
    {
        switch (start_move)
        {
            case start_move_d.l:
                move_to_l();
                break;
            case start_move_d.r:
                move_to_r();
                break;
            case start_move_d.no:
                break;
            default:
                break;
        }
    }
    public void stop_all_Coroutine()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// 向右移动
    /// </summary>
    public void move_to_r()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(move_r());
    }
    public IEnumerator move_r()
    {
        if (start_move_r_action != null)
            start_move_r_action();
        //Debug.Log("向右移动");
        Vector3 vector = new Vector3(r_x, rect_transform.localPosition.y, rect_transform.localPosition.z);

        //Color color = new Color();
        while(Mathf.Abs(rect_transform.localPosition.x-r_x)>x)
        {
            //Debug.Log("正在向右移动");
            rect_transform.localPosition=Vector3.Lerp(rect_transform.localPosition, vector, Time.deltaTime * v);
            //rect_transform.localPosition += Vector3.right * Time.deltaTime * v;

            yield return null;
        }
        if (after_move_r_action != null)
            after_move_r_action();
        yield return null;
    }
    /// <summary>
    /// 向左移动
    /// </summary>
    public void move_to_l()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(move_l());
    }
    public IEnumerator move_l()
    {
        if (start_move_l_action != null)
            start_move_l_action();
        Vector3 vector = new Vector3(l_x, rect_transform.localPosition.y, rect_transform.localPosition.z);

        //Color color = new Color();
        while (Mathf.Abs(rect_transform.localPosition.x - l_x) > x)
        {
            rect_transform.localPosition = Vector3.Lerp(rect_transform.localPosition, vector, Time.deltaTime * v);
            //rect_transform.localPosition -= Vector3.right * Time.deltaTime * v;
            yield return null;

        }
        if (after_move_l_action != null)
            after_move_l_action();
        yield return null;
    }

    public void appear_start()
    {
        StartCoroutine(appear());
    }
    public void dis_appear_start()
    {
        StartCoroutine(dis_appear());
    }
    /// <summary>
    /// 显出函数
    /// </summary>
    /// <returns></returns>
    IEnumerator appear()
    {
        float x = transform.localScale.x;
        while (x < 1.0)
        {
            x = Mathf.Lerp(x, 2f, av * Time.deltaTime);
            x = Mathf.Min(x, 1f);
            transform.localScale = new Vector3(x, x, x);
            
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    /// <summary>
    /// 淡化函数
    /// </summary>
    /// <returns></returns>
    IEnumerator dis_appear()
    {
        float x = transform.localScale.x;
        while (x > 0)
        {
            x = Mathf.Lerp(x, -1f, av * Time.deltaTime);
            x = Mathf.Max(x, 0f);
            transform.localScale = new Vector3(x, x, x);
           
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}