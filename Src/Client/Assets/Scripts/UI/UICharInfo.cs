using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;
    public SkillBridge.Message.NCharacterInfo Info
    {
        get { return info; }
        set
        {
            info = value;
            init_itself();
        }
    }
   
    [Header("角色名text")]
    public Text name;
    [Header("角色等级text")]
    public Text level;
    [Header("角色logo")]
    public GameObject[] logos;

    [Header("选择button")]
    public Button select_button;
    [Header("创建角色按钮")]
    public GameObject creat_character_button;
    [Header("被选择时背景")]
    public GameObject selected_bg;
    
    private void set_logos(int idx)
    {
        for(int i=0;i<4;i++)
        {
            logos[i].SetActive(i == idx);
        }
    }
    private void init_itself()
    {
        if(info==null)
        {
            name.text = "创建新角色";
            level.text = "";
            select_button.gameObject.SetActive(false);
            creat_character_button.SetActive(true);
            set_logos(0);
        }
        else
        {
            name.text = info.Name;
            level.text = "等级" + info.Level.ToString();
            select_button.gameObject.SetActive(true);
            creat_character_button.SetActive(false);
            set_logos((int)info.Class);
        }
        gameObject.SetActive(true);
    }
}
