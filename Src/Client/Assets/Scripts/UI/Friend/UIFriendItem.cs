using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem :ListView.ListViewItem
{
    public Text name;
    public Text level;
    public Text @class;
    public Text status;
    public Image backGround;

    private Sprite normalBg;
    public Sprite selectedBg;
    
    public Color normalColor;

    public Color disApplerColor;
    
    
    private bool bg_is_app;
    [HideInInspector]
    public NFriendInfo friendInfo;
    private void Start()
    {
        this.normalBg = backGround.sprite;
        //this.normalColor = this.backGround.color;
    }
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? this.selectedBg : this.normalBg;

        if (selected)
            this.backGround.color = normalColor;
        else
            this.backGround.color = bg_is_app ? normalColor : disApplerColor;
        
    }
    public void SetFriend(NFriendInfo info,ListView listView, bool bg_Is_App)
    {
        this.bg_is_app = bg_Is_App;
        if (!bg_Is_App)
        {
            this.backGround.color = disApplerColor;
        }

        this.friendInfo = info;
        listView.AddItem(this);
        if (this.name != null) this.name.text = info.friendInfo.Name;
        if (this.level != null) this.level.text = info.friendInfo.Level.ToString();
        if(this.@class!=null)
        {
            this.@class.text = CharacterDefine.CharacterClass_Dic[info.friendInfo.Class];
        }
        if(this.status!=null)
        {
            if (info.Status == 0) this.status.text = "离线";
            else this.status.text = "在线";
        }
    }

}
