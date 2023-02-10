using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dis_itself : MonoBehaviour {

    [Header("销毁时间")]
    public float dis_time;
    [Header("停顿时间")]
    public float stop_time;
    public Image Image;
    IEnumerator Start () {
        if (Image == null)
        {
            yield return new WaitForSeconds(dis_time);
            gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(stop_time);
            while (Image.color.a > 0f)
            {
                Color color = Image.color;
                float x = 1.0f / dis_time;
                color.a -= Mathf.Min(x * Time.deltaTime, color.a);
                Image.color = color;
                yield return null;
            }
          

            gameObject.SetActive(false);
        }
	}
	
}
