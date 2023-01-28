using Assets.Scripts.Services;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeam :UIWindow {

    public ListView ListView;
    public UITeamItem[] items;

    public Text teamTitle;
	// Use this for initialization
	void Start () {
        if (User.Instance.TeamInfo == null)
            Destroy(this.gameObject);
        for (int i = 0; i < items.Length; i++)
            this.ListView.AddItem(items[i]);
        this.UpdateTeamUI();
	}

    internal void UpdateTeamUI()
    {
        if (User.Instance.TeamInfo == null) return;
        NteamInfo team = User.Instance.TeamInfo;
        this.teamTitle.text = string.Format("我的队伍({0}/5)", team.Members.Count);
        for(int i=0;i<5;i++)
        {
            if (i < team.Members.Count)
            {
                this.items[i].SetItem(i, team.Members[i], team.leaderId == team.Members[i].Id);
                this.items[i].gameObject.SetActive(true);
            }
            else
                this.items[i].gameObject.SetActive(false);
        }
    }
    public void OnChickLeaveTeam()
    {
        MessageBox.Show("确定要离开队伍吗？", "离开队伍", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
          {
              TeamService.Instance.SendTeamLeave();
          };
    }
}
