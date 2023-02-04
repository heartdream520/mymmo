using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGulidListItem : ListView.ListViewItem
{

    public Image backGround;
    [HideInInspector]
    public Sprite normalBg;
    public Sprite selectedBg;

    public Text gulid_name;
    public Text leader_name;
    
    public Text mumber_num;
    public Text load_num;

    [HideInInspector]
    public NGulidInfo info;
    private void Start()
    {
        this.normalBg = backGround.sprite;

    }
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectedBg : normalBg;
    }
    public void InitItem(NGulidInfo info)
    {
        this.info = info;
        if (this.gulid_name != null) this.gulid_name.text = info.GulidName;
        if (this.leader_name != null) this.leader_name.text = info.LeaderName;
        if (this.mumber_num != null) this.mumber_num.text = info.memberCount.ToString();
        if (this.load_num != null) load_num.text = info.loadmemberCount.ToString();
    }
}
