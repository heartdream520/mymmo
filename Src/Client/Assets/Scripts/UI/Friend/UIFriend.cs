using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIFriend : UIWindow
{
    public ListView ListView;
    public GameObject UIFriendItemPrefab;
    private UIFriendItem selectedItem;
    private void Start()
    {
        this.ListView.onItemSelected += this.OnFriendListViewSelected;
        FriendService.Instance.OnFriendUpdate += this.RefreshUI;
        RefreshUI();
    }
    private void OnDestroy()
    {
        this.ListView.onItemSelected -= this.OnFriendListViewSelected;
        FriendService.Instance.OnFriendUpdate -= this.RefreshUI;
    }
    private void OnFriendListViewSelected(ListView.ListViewItem arg0)
    {
        this.selectedItem = (UIFriendItem)arg0;
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
        int i = 0;
        foreach(var f in FriendManager.Instance.allFriends)
        {
            i++;
            GameObject go = GameObject.Instantiate(this.UIFriendItemPrefab,this.ListView.transform,false);
            var uI = go.GetComponent<UIFriendItem>();
            uI.SetFriend(f, this.ListView, i % 2 == 1);

        }
    }
    public void OnchickRemoveFriendButton()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        var box = MessageBox.Show(string.Format("确定要删除好友 {0} 吗？", selectedItem.friendInfo.friendInfo.Name),
            "删除好友", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes += () =>
          {
              FriendService.Instance.SendFriendRemoveRequest(selectedItem.friendInfo.Id, selectedItem.friendInfo.friendInfo.Id);
          }; 
    }
    public void OnchickAddFriendButton()
    {
        InputBox.Show("要添加的好友Id", "添加好友","确定","取消").Onsumbit += this.OnFriendInputSumbit;
    }
    public void OnchickFriendChatButton()
    {
        MessageBox.Show("功能暂未开放");
    }

    private bool OnFriendInputSumbit(string inputText, out string tips)
    {
        tips = "";
        int friendId = 0;
        if(!int.TryParse(inputText,out friendId))
        {
            tips = "好友Id为数字！";
            return false;
        }
        if(friendId==User.Instance.CurrentCharacter.Id)
        {
            tips = "不能添加自己为好友哦！";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, "");
        return true;
    }
}
