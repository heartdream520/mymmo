using Assets.Scripts.UI.Set;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterView : MonoBehaviour {

    [Header("角色展示")]
    public GameObject[] characters;

    [Header("角色info")]
    public GameObject ui_character_info;

    [Header("角色展示视图")]
    public GameObject characters_content;

    [Header("角色选择脚本")]
    public character_scale character_Scale;

    /// <summary>
    /// 选择的角色idx
    /// </summary>
    private int selectCharacterIdx;
    /// <summary>
    /// 在面板上的角色
    /// </summary>
    private List<GameObject> content_chars_list;

    private int currentCharacter = 0;

    public int CurrectCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            character_Scale.selected_character(value+1);
        }
    }
    private void Start()
    {
        SoundManager.Instance.PlayerMusic(SoundDefine.Music_Select);
    }
    private void OnEnable()
    {
        selectCharacterIdx = -1;
        CurrectCharacter = -1;
        if(content_chars_list==null)
        {
            content_chars_list = new List<GameObject>();
        }
        foreach(var y in content_chars_list)
        {
            Destroy(y);
        }
        content_chars_list.Clear();
        for(int i=0;i<User.Instance.Info.Player.Characters.Count;i++)
        {
            GameObject go = Instantiate(ui_character_info, characters_content.transform, false);
            UICharInfo uICharInfo = go.GetComponent<UICharInfo>();
            uICharInfo.Info = User.Instance.Info.Player.Characters[i];
            Button button = uICharInfo.select_button;
            int idx = i;
            button.onClick.AddListener(() =>
            {
                OnSelect_UI_character(idx);
            });
            content_chars_list.Add(go);
            go.SetActive(true);
        }
        ///创建一个创建角色的UI
        GameObject x = Instantiate(ui_character_info, characters_content.transform, false);
        UICharInfo uIChar = x.GetComponent<UICharInfo>();
        uIChar.Info = null;
        content_chars_list.Add(x);
        x.SetActive(true);

    }
    /// <summary>
    /// 当选择角色时按钮点击事件
    /// </summary>
    /// <param name="idx"></param>
    private void OnSelect_UI_character(int idx)
    {
        if (this.selectCharacterIdx == idx) return;
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        var cha = User.Instance.Info.Player.Characters[idx];
        character_Scale.id= (int)cha.Class - 1;
        CurrectCharacter =(int)cha.Class - 1;
        selectCharacterIdx = idx;
        Debug.LogFormat("Select Character:[{0}]{1}[{2}] ", cha.Id, cha.Name, cha.Class);
        
        for(int i=0;i<content_chars_list.Count-1;i++)
        {
            content_chars_list[i].GetComponent<UICharInfo>().selected_bg.SetActive(i == idx);
        }

    }
    /// <summary>
    /// 进入游戏按钮点击事件
    /// </summary>
    public void OnClickPlay()
    {
       
        if (selectCharacterIdx >= 0)
        {

            var go= MessageBox.Show("进入游戏", "进入游戏", MessageBoxType.Confirm);

            go.OnYes = delegate()
            {
                UserService.Instance.SendCharacterEnter(selectCharacterIdx);
                //Debug.LogErrorFormat("TES函数执行了！");
            };
        }
    }
    public void OnChickButtonPlaySound()
    {
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
    }
}
