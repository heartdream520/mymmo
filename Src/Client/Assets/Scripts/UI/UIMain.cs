using Assets.Scripts.Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;
    public Sprite[] avatars;
    public Image avatar_image;

    public override void OnAwake()
    {
        User.Instance.CurrentCharacter_Set_Action += UpdateAvatar;
    }
    // Use this for initialization
    public override void OnStart(){

        UpdateAvatar();
    }

    void UpdateAvatar()
    {
        if (User.Instance.CurrentCharacter == null) return;
        if (this.avatarName != null)
            this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        if (this.avatarLevel != null)
            this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
        if (this.avatar_image != null)
            this.avatar_image.sprite = avatars[User.Instance.CurrentCharacter.ConfigId - 1];

    }


    public void onchick_back_character_select()
    {
        MapService.Instance.CurrentMapId = 0;
        MouseManager.Instance.ToShowCursor();
        UserService.Instance.SendCharacterLeave();
        MySceneManager.Instance.LoadScene("CharSelect");
    }
    public void onchick_test_button(string title="标题")
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.transform.SetParent(this.transform, false);
        test.gameObject.name = test.Type.Name;
        if (!String.IsNullOrEmpty(title))
            test.text.text = title;
        test.Onclose += this.On_UIText_Close;
    }

    private void On_UIText_Close(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("你点击了test的"+result.ToString());
    }
    public void OnchickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
    public void OnchickEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }
    public void OnchickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnchickFriend()
    {
        UIManager.Instance.Show<UIFriend>();
    }
    public void OnchickGulid()
    {
        GulidManager.Instance.ShowGulid();
    }
    internal void ShowTeamUI(bool show)
    {
        var go = UIManager.Instance.UIResources[typeof(UITeam)].Instance;
        if (show)
        {
            if (go != null)
                go.GetComponent<UITeam>().UpdateTeamUI();
            else UIManager.Instance.Show<UITeam>();
        }
        else
        {
            
            if (go != null)
                go.GetComponent<UITeam>().OnClick_Close();
        }


    }

}
