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


public class UIGulid :UIWindow
{ 

    public ListView ListView;
    public NGulidInfo info;
    public NGulidMemberInfo mumber_info;
    public GameObject[] Leader_buttons;
    public GameObject[] Vice_Leader_buttons;
    public GameObject[] mumber_buttons;
    public GameObject mumber_Item_Prefabs;

    public UIGulidInfo UiGulidInfo;
    public Text ApplyNumText;

    [HideInInspector]
    public UIGulidCItem selected_item;
    private void Start()
    {
        GulidManager.Instance.OnNGulidInfoAction += this.SetInfo;
        GulidService.Instance.SendGulidInfo();
        this.ListView.onItemSelected += this.OnSelected;
        SetInfo(GulidManager.Instance.Gulid_Info);
    }

    private void OnSelected(ListView.ListViewItem arg0)
    {
        this.selected_item = (UIGulidCItem)arg0;
    }

    private void OnDestroy()
    {
        GulidManager.Instance.OnNGulidInfoAction -= this.SetInfo;
        this.ListView.onItemSelected -= this.OnSelected;
    }
    private void SetInfo(NGulidInfo info)
    {
        this.info = info;
        this.mumber_info = GulidManager.Instance.mumber_info;
        this.RefreshUI();
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
        if (this.info == null)
        {
            this.OnClick_Close();
            return;
        }
            
        this.UiGulidInfo.SetInfo(this.info);
        var title = this.mumber_info.Title;
        switch (title)
        {
            case GulidTitle.None:
                this.SetGameObgects(this.Leader_buttons, false);
                this.SetGameObgects(this.Vice_Leader_buttons, false);
                this.SetGameObgects(this.mumber_buttons, true);
                break;
            case GulidTitle.President:
                this.SetGameObgects(this.Vice_Leader_buttons, false);
                this.SetGameObgects(this.mumber_buttons, false);
                this.SetGameObgects(this.Leader_buttons, true);
                break;
            case GulidTitle.VicePresident:
                this.SetGameObgects(this.Leader_buttons, false);
                this.SetGameObgects(this.mumber_buttons, false);
                this.SetGameObgects(this.Vice_Leader_buttons, true);
                break;
            default:
                break;
        }
        ApplyNumText.gameObject.SetActive(true);
        ApplyNumText.transform.parent.gameObject.SetActive(true);
        if (this.ApplyNumText.gameObject.activeInHierarchy)
        {
            if (this.info.Applies.Count == 0)
            {
                ApplyNumText.transform.parent.gameObject.SetActive(false);
            }
            else this.ApplyNumText.text = this.info.Applies.Count.ToString();
        }
        SetMemberUIByTitle(GulidTitle.President);
        SetMemberUIByTitle(GulidTitle.VicePresident);
        SetMemberUIByTitle(GulidTitle.None);

    }
    private void SetMemberUIByTitle(GulidTitle title)
    {
        foreach (var m in this.info.Members)
        {
            if (m.Title != title) continue;
            var go = GameObject.Instantiate(this.mumber_Item_Prefabs, this.ListView.transform, false);

            var x = go.GetComponent<UIGulidCItem>();

            x.InitItem(m);
            this.ListView.AddItem(x);
        }
    }

    private void SetGameObgects(GameObject[] buttons, bool v)
    {
        foreach(var button in buttons)
            button.SetActive(v);
    }
    public void Onchick_转让()
    {
        if(this.selected_item==null)
        {
            MessageBox.Show("请选择要转让会长的成员","公会");
            return;
        }
        if (selected_item.info.characterId == User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("不能对自己进行此操作！", "公会");
            return;
        }
        var box = MessageBox.Show(string.Format("确定转让公会给[{0}]吗" ,selected_item.info.Characterinfo.Name), "公会", MessageBoxType.Confirm, "确定", "取消");
        
        box.OnYes = () =>
        {
            GulidService.Instance.SendGulidAdmin(GulidAdminCommand.Transfer, selected_item.info.characterId);
        };
    }
    public void Onchick_晋升()
    {
        if (this.selected_item == null)
        {
            MessageBox.Show("请选择要晋升为副会长的成员", "公会");
            return;
        }
        if (selected_item.info.characterId == User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("不能对自己进行此操作！", "公会");
            return;
        }
        if(selected_item.info.Title==GulidTitle.VicePresident)
        {
            MessageBox.Show("对方已经是副会长了", "公会");
            return;
        }
        var box = MessageBox.Show(string.Format("确定要晋升[{0}]为副会长吗?",selected_item.info.Characterinfo.Name), "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
        {
            GulidService.Instance.SendGulidAdmin(GulidAdminCommand.Promote, selected_item.info.characterId);
        };
    }
    public void Onchick_解散()
    {
        var box = MessageBox.Show(string.Format("确定要解散公会吗?"), "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
        {
            GulidService.Instance.SendGulidAdmin(GulidAdminCommand.Dispand, 0);
        };
    }
    public void Onchick_踢出()
    {
        if (this.selected_item == null)
        {
            MessageBox.Show("请选择要踢出的成员", "公会");
            return;
        }
        if (selected_item.info.characterId == User.Instance.CurrentCharacter.Id)
        {
            MessageBox.Show("不能对自己进行此操作！", "公会");
            return;
        }
        if (selected_item.info.Title == GulidTitle.President)
        {
            MessageBox.Show("会长不是你能动的！", "公会");
            return;
        }
        if (selected_item.info.Title == GulidTitle.VicePresident&&GulidManager.Instance.mumber_info.Title==GulidTitle.VicePresident)
        {
            MessageBox.Show("对方地位与你相同！", "公会");
            return;
        }
        var box = MessageBox.Show(string.Format("确定要将[{0}]踢出公会吗?", selected_item.info.Characterinfo.Name), "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
        {
            GulidService.Instance.SendGulidAdmin(GulidAdminCommand.Kickout, selected_item.info.characterId);
        };
    }
    public void Onchick_申请列表()
    {
        var x= UIManager.Instance.Show<UIGulidApplies>();
        x.UIGulid = this;
    }
    public void Onchick_退出()
    {
        var box=MessageBox.Show("真的要退出公会吗?", "公会", MessageBoxType.Confirm, "确定", "取消");
        box.OnYes = () =>
          {
              GulidService.Instance.SendGulidLeave();
          };
        
    }

}
