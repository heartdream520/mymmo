using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
public class UIGulidInfo : MonoBehaviour {

    public NGulidInfo info;
    public Text gulid_Name;
    public Text leader_Name;
    public Text gulid_num;
    public Text notice;
    public Text Load_num;
    internal void SetInfo(NGulidInfo gulidInfo)
    {
        this.info = gulidInfo;
        this.SetInfoUI();
    }

    private void SetInfoUI()
    {
        this.gulid_Name.text = this.info.GulidName;
        this.leader_Name.text = this.info.LeaderName;
        this.gulid_num.text = this.info.memberCount.ToString();
        this.notice.text = this.info.Notice;
        this.Load_num.text = this.info.loadmemberCount.ToString();

    }
}
