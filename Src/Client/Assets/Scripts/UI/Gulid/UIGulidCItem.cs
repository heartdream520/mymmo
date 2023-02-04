using Common.Data;
using Common.Utils;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGulidCItem :ListView.ListViewItem
{

    public Image backGround;
    [HideInInspector]
    public Sprite normalBg;
    public Sprite selectedBg;
    public Text name;
    public Text level;
    public Text @class;
    /// <summary>
    /// 职位
    /// </summary>
    public Text posts;
    public Text join_time;
    public Text last_load_time;
    [HideInInspector]
    public NGulidMemberInfo info;
    private void Start()
    {
        this.normalBg = backGround.sprite;
    }
    public override void onSelected(bool selected)
    {
        this.backGround.overrideSprite = selected ? selectedBg : normalBg;
    }
    public void InitItem(NGulidMemberInfo info)
    {
        this.info = info;
        if (this.name != null) this.name.text = info.Characterinfo.Name;
        if (this.level != null) this.level.text = info.Characterinfo.Level.ToString();
        if (this.@class != null) this.@class.text = CharacterDefine.CharacterClass_Dic[info.Characterinfo.Class];
        if (this.posts != null) this.posts.text = EnumDic.Gulid_Posts_Dic[info.Title];

        if (this.join_time != null) this.join_time.text = TimeUtil.GetTime(info.joinTime).ToString();
        if (this.last_load_time != null)
        {
            if (info.Status == 1) this.last_load_time.text = "在线";
            else
                this.last_load_time.text =TimeUtil.GetTime(info.lastLoadTime).ToString();
        }
    }
}
