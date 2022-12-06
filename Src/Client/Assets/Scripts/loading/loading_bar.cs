using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loading_bar :MonoSingleton<loading_bar>
{ 
    [Header("进度条")]
    public Image loading_bar_image;

    [Header("进度Text")]
    public Text progress_text;

    private int progress_num;
    /// <summary>
    /// 进度数字属性
    /// </summary>
    public int Progress_Num
    {
        get { return progress_num; }
        set
        {
            progress_num = value;
            set_progress_text();
            
        }
    }
    public float Progress_Num_Float
    {
        set
        {
            loading_bar_image.fillAmount = value;
            Progress_Num = (int)(value * 100f);
        }
    }

    private void set_progress_text()
    {
        progress_text.text = progress_num.ToString()+"%";
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            loading_bar_image.fillAmount += 0.1f;
            Progress_Num = (int)(loading_bar_image.fillAmount * 100f);
        }
        else if(Input.GetKeyDown(KeyCode.L)||Input.GetKey(KeyCode.L))
        {
            loading_bar_image.fillAmount -= 0.01f;
            Progress_Num = (int)(loading_bar_image.fillAmount * 100f);
        }

	}
}
