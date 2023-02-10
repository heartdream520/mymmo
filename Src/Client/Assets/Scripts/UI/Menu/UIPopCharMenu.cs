using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
using Assets.Scripts.UI.Set;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopCharMenu : UIMenu
{
    public int targerId;
    public string targerName;

    public override void VEnable()
    {
        this.transform.localPosition = Input.mousePosition + new Vector3(80, -160, 0);
    }
    public void OnChat()
    {
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        ChatManager.Instance.StartPrivateChat(targerId, targerName);
        this.Root.GetComponent<UIChat>().RefreshUI();
        this.OnClick_Close();
    }
    public void OnAddFriend()
    {
        var box= MessageBox.Show(string.Format("是否向[{0}]发送好友申请",this.targerName), "好友", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
          {
              FriendService.Instance.SendFriendAddRequest(this.targerId, this.targerName);
              this.OnClick_Close();
          };
    }
    public void OnInviteTeam()
    {
        var box = MessageBox.Show(string.Format("是否向[{0}]发送组队申请", this.targerName), "组队", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
        {
            TeamService.Instance.SendTeamInviteRequest(this.targerId, this.targerName);
            this.OnClick_Close();
        };
    }

}
