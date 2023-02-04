using System;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Common.Data;
using Assets.Scripts.Services;

public class UIGulidApplyItem : ListView.ListViewItem{

    public Text name;
    public Text level;
    public Text @class;
    private NGulidApplyInfo info;
    internal void InitItem(NGulidApplyInfo a)
    {
        this.info = a;
        this.name.text = a.Name;
        this.level.text =a.Level.ToString() + "级";
        
        this.@class.text = EnumDic.CharacterClass_Dic[(CharacterClass)a.Class];
    }
    public void Onchick_同意()
    {
        var box = MessageBox.Show(string.Format("是否同意[{0}]的入会申请?", info.Name), "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
          {
              GulidService.Instance.SendGulidJoinResponse(this.info, true);
          };
    }
    public void Onchick_拒绝()
    {
        var box = MessageBox.Show(string.Format("是否拒绝[{0}]的入会申请?", info.Name), "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
        {
            GulidService.Instance.SendGulidJoinResponse(this.info, false);
        };
    }
}
