using Assets.Scripts.Models;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
using Assets.Scripts.UI.UIQuest;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIQuestItem :ListView.ListViewItem
{
    public Color activeColor;
    public Text QuestName;
    private Color normalColor;
    public Image tabImage;
    [HideInInspector]
    public Quest quest;
    private void Start()
    {
        normalColor = tabImage.color;
    }
    public override void onSelected(bool selected)
    {
        tabImage.color = this.Selected ? activeColor : normalColor;
    }
    public void SetQuest(ListView list,Quest quest)
    {
        owner = list;
        owner.AddItem(this);
        this.quest = quest;
        if (this.QuestName != null)
            this.QuestName.text = quest.Define.Name;
    }
}
