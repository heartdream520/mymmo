using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Models;
using SkillBridge.Message;

public class UITeamItem :ListView.ListViewItem
{ 

    public Text name;
    public Text Leavel;
    public Image CIcon;
    public Image LeaderIcon;
    public NCharacterInfo info;
    public Image back_image;
    public Color normal_Color;
    public Color selected_Color;

    [HideInInspector]
    public int idx;

    public override void onSelected(bool selected)
    {
        back_image.color = selected ? selected_Color : normal_Color;
    }
    public void SetItem(int idx,NCharacterInfo info,bool is_Leader)
    {
        this.idx = idx;
        this.info = info;
        this.name.text = info.Name;
        this.Leavel.text = info.Level.ToString();
        this.CIcon.overrideSprite = SpritesManager.Instance.CharacterClassIcons[info.ConfigId - 1];
        this.LeaderIcon.gameObject.SetActive(is_Leader);

    }
}
