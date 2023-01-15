using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using SkillBridge.Message;
using System;
using Models;

public class MyUICharacterSelect : MonoBehaviour {


    [Header("角色标签UI")]
    public GameObject[] character_tips_UI;
    [Header("角色描述text")]
    public Text dis_text;
    [Header("创建的角色名")]
    public InputField character_name_InputField;

    [Header("按钮image")]
    public Sprite[] characters_button_images;
    [Header("选择角色按钮")]
    public Image[] characters_button;

    [Header("选择角色面板")]
    public GameObject character_Select_panel;
    [Header("角色创建面板")]
    public GameObject character_Creat_panel;
    public UnityAction<int> now_selected_Char_action;
    private CharacterClass now_selected_Char;
    public CharacterClass Now_Selected_Char
    {
        get { return now_selected_Char; }
        set
        {
            now_selected_Char = value;
            if (now_selected_Char_action != null)
                now_selected_Char_action((int)value);
        }
    }



    IEnumerator Start()
    {
        //测试用加载游戏数据
        //yield return DataManager.Instance.LoadData();
        now_selected_Char_action += set_Panel;
        yield return null;
        
    }

    private void OnEnable()
    {
        Now_Selected_Char = CharacterClass.Warrior;
        now_selected_Char = CharacterClass.None;
        characters_button[0].sprite = characters_button_images[1];
        dis_text.text = DataManager.Instance.Characters[1].Description;
        character_name_InputField.text = "";
    }
    private void Awake()
    {
        Services.UserService.Instance.OnCharacterCreate += OnCharacterCreate;
    }
    private void OnDestroy()
    {
        Services.UserService.Instance.OnCharacterCreate -= OnCharacterCreate;
    }
    private void OnCharacterCreate(Result result, string msg)
    {
        

        if(result==Result.Success)
        {
            MessageBox.Show("角色创建成功");
            if (this.character_Select_panel == null) this.character_Select_panel = transform.parent.GetChild(2).gameObject;
            if (this.character_Creat_panel == null) this.character_Creat_panel = transform.parent.GetChild(3).gameObject;
            character_Select_panel.SetActive(true);
            character_Creat_panel.SetActive(false);
        }
    }

    /// <summary>
    /// 当角色变化时设置当前的UI
    /// </summary>
    private void set_Panel(int x)
    {
        for(int i=0;i<3;i++)
        {
            character_tips_UI[i].SetActive(i == (int)x - 1);
            if(i == x - 1)
            {
                characters_button[i].sprite = characters_button_images[4+i];
            }
            else characters_button[i].sprite = characters_button_images[(i + 1)];
        }
        dis_text.text = DataManager.Instance.Characters[x].Description;
    }
    

    public void onchick_select_class_button(int cla)
    {
        Now_Selected_Char =(CharacterClass) cla;
    }
    /// <summary>
    /// 点击创建角色button
    /// </summary>
    public void onchick_creat_button()
    {
        if (Now_Selected_Char == CharacterClass.None)
        {
            MessageBox.Show("请选择职业！");
            return;
        }
        if (string.IsNullOrEmpty(character_name_InputField.text))
        {
            MessageBox.Show("请输入昵称！");
            return;
        }
        Debug.LogFormat("creat character: class:{0}  name:{1}", Now_Selected_Char, character_name_InputField.text);
        Services.UserService.Instance.SendCharacterCreate(Now_Selected_Char,character_name_InputField.text);
        
        

    }
}
