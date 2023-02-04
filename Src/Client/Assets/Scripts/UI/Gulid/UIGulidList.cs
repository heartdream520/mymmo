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

public class UIGulidList : UIWindow
{
    public ListView ListView;
    public GameObject UIGulidListItemPrefab;
    public UIGulidInfo UiGulidInfo;
    [HideInInspector]
    public UIGulidListItem selected_Item;


    private void Start()
    {
        UiGulidInfo.gameObject.SetActive(false);
        GulidManager.Instance.OnGulidListAction += this.RefreshUI;
        GulidManager.Instance.OnNGulidInfoAction += this.to_Close;
        GulidService.Instance.SendGulidList();
        this.ListView.onItemSelected += this.OnSelected;
    }

    private void to_Close(NGulidInfo info)
    {
        if(info!=null)
        {
            this.OnClick_Close();
            UIManager.Instance.Show<UIGulid>();
        }
    }

    private  void OnSelected(ListView.ListViewItem arg0)
    {
        UiGulidInfo.gameObject.SetActive(arg0 == null ? false : true);
        this.selected_Item = (UIGulidListItem)arg0;
        this.UiGulidInfo.SetInfo(selected_Item.info);
    }
    private void OnDestroy()
    {
        GulidManager.Instance.OnGulidListAction -= this.RefreshUI;
        this.ListView.onItemSelected -= this.OnSelected;
        GulidManager.Instance.OnNGulidInfoAction -= this.to_Close;
    }
    private void RefreshUI()
    {
        this.ClearAllUI();
        this.InitUI();
    }
    private void ClearAllUI()
    {
        this.ListView.RemoveAll();
    }
    private void InitUI()
    {
        foreach (var m in GulidManager.Instance.GulidList)
        {
            var go = GameObject.Instantiate(this.UIGulidListItemPrefab, this.ListView.transform, false);

            var x = go.GetComponent<UIGulidListItem>();

            x.InitItem(m);
            this.ListView.AddItem(x);
        }
    }
    public void OnChickJoinGulidButton()
    {
        if(this.selected_Item==null)
        {
            MessageBox.Show("请选择要加入的公会", "公会");
            return;
        }
        GulidService.Instance.SendJoinGulidRequest(User.Instance.CurrentCharacter.Id,selected_Item.info.Id);
    }
}