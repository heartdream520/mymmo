using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Services;

public class UIGulidApplies : UIWindow {

    public List<NGulidApplyInfo> applylist;
    public GameObject gulidApplyItemPrefab;
    public ListView ListView;
    [HideInInspector]
    private UIGulid uIGulid;
    public UIGulid UIGulid
    {
        set
        {
            this.uIGulid = value;
            uIGulid.Onclose += this.OnUIGulidClose;
        }
        
    }
    private void Start()
    {
        
        GulidManager.Instance.OnNGulidInfoAction += this.SetApplyList;
        GulidService.Instance.SendGulidInfo();
        
        this.SetApplyList(GulidManager.Instance.Gulid_Info);
    }



    private void OnDestroy()
    {
        GulidManager.Instance.OnNGulidInfoAction -= this.SetApplyList;
        uIGulid.Onclose -= this.OnUIGulidClose;
    }
    private void OnUIGulidClose(UIWindow sender, WindowResult result)
    {
        this.OnClick_Close();
    }
    private void SetApplyList(NGulidInfo GulidInfo)
    {
        this.applylist = GulidInfo.Applies;
        this.RefreshUI();
    }
    private void RefreshUI()
    {
        this.RemoveAllUI();
        this.InitUI();
    }
    private void RemoveAllUI()
    {
        this.ListView.RemoveAll();
    }
    private void InitUI()
    {
        foreach (var a in this.applylist)
        {
            if (a.ApplyResult != ApplyResult.None) return;
            var go = GameObject.Instantiate(this.gulidApplyItemPrefab, ListView.transform, false);
            var x = go.GetComponent<UIGulidApplyItem>();
            x.InitItem(a);
            ListView.AddItem(x);
        }
    }
}
