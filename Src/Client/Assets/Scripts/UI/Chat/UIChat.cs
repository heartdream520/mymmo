using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Candlelight.UI;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : UIWindow {

    public HyperText textAres;
    public TabView tabView;
    public InputField chatText;
    //聊天目标
    public Text chatTarger;

    public Dropdown channelSelected_DropDown;

    public RunTobotton runTobotton;
    public GameObject[] gureen_Dot;

    private void Start()
    {
        this.tabView.OnTabSelect += this.OnDisPlayChannelSelect;
        ChatManager.Instance.OnChatAction += this.RefreshUI;

        InputManager.Instance.inputFields.Add(this.chatText);
    }


    private void OnDestroy()
    {
        this.tabView.OnTabSelect -= this.OnDisPlayChannelSelect;
        ChatManager.Instance.OnChatAction -= this.RefreshUI;

        InputManager.Instance.inputFields.Add(this.chatText);

    }
    private void OnDisPlayChannelSelect(int selected)
    {
        ChatManager.Instance.disPlayChannel = (ChatManager.LocalChannel)selected;
        this.RefreshUI();
    }

    public void RefreshUI()
    {
        this.tabView.SelectTab((int)ChatManager.Instance.disPlayChannel,false);
        this.textAres.text = ChatManager.Instance.GetCurrentMessages();
        this.channelSelected_DropDown.value = (int)ChatManager.Instance.sendChannel - 1;
        if(ChatManager.Instance.disPlayChannel==ChatManager.LocalChannel.Private)
        {
            this.chatTarger.transform.parent.gameObject.SetActive(true);
            if(ChatManager.Instance.privateId!=0)
            {
                this.chatTarger.text = ChatManager.Instance.privateName + ":";

            }
            else this.chatTarger.text = ChatManager.Instance.privateName + "<无>:";
        }
        else this.chatTarger.transform.parent.gameObject.SetActive(false);

        runTobotton.runTobotton();
        ChatManager.Instance.hasNewMessage[(int)ChatManager.Instance.disPlayChannel] = false;
        for(int i=0;i<6;i++)
        {
            this.gureen_Dot[i].SetActive(ChatManager.Instance.hasNewMessage[i]);
        }
    }
    public void OnchickChatLink(HyperText text,HyperText.LinkInfo link)
    {
        Debug.LogError(link.Name);
        if (string.IsNullOrEmpty(link.Name))
            return;
        if(link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            var menu = UIManager.Instance.Show<UIPopCharMenu>();
            menu.targerId = int.Parse(strs[1]);
            menu.targerName = strs[2];
            menu.transform.SetParent(this.transform.GetChild(0),false);
            menu.Root = this.gameObject;
            ChatManager.Instance.privateId= int.Parse(strs[1]);
            ChatManager.Instance.privateName= strs[2];

            //ChatManager.Instance.sendChannel = ChatManager.LocalChannel.Private;
            //this.channelSelected_DropDown.value = (int)(ChatManager.Instance.sendChannel - 1);

        }
    }
    public void OnchickSend()
    {
        this.OnEndInput(this.chatText.text);
    }

    public void OnEndInput(string text)
    {
        //Trim 去除首尾空格
        if (!string.IsNullOrEmpty(text.Trim()))
            this.SendChat(text);
        
        this.chatText.text = "";
        
    }

    private void SendChat(string text)
    {
        ChatManager.Instance.SendChat(text);
    }


    /// <summary>
    /// 发送频道改变
    /// </summary>
    /// <param name="idx"></param>
    public void OnSendChannelChange(int idx)
    {
        idx = channelSelected_DropDown.value;
        if((int)ChatManager.Instance.sendChannel == idx + 1)
        {
            return;
        }
        //改变失败在该回去
        if (!ChatManager.Instance.SetSendChannel(idx + 1))
        {
            this.channelSelected_DropDown.value = (int)(ChatManager.Instance.sendChannel - 1);
        }
        else
            //改变成功刷新UI
            this.RefreshUI();

    }
}
