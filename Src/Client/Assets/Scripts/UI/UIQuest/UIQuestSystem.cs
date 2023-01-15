using Assets.Scripts.Managers;
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
using UnityEngine.Events;
public class UIQuestSystem : UIWindow
{ 
    public ListView questList;
    public GameObject UIQuestItemPrefab;
    
    [Header("任务属性脚本")]
    public UIQuestAttribute QuestAttributeUI;

    private int listType;

    private void Start()
    {
        this.QuestAttributeUI.gameObject.SetActive(false);
        questList.onItemSelected += this.OnQuestSelected;
        QuestManager.Instance.OnQuestStatusChangeAction += this.RefreshUI;
        this.listType = 0;
        RefreshUI();
    }
    private void OnDestroy()
    {
        questList.onItemSelected -= this.OnQuestSelected;
        QuestManager.Instance.OnQuestStatusChangeAction -= this.RefreshUI;
    }
    public void OnchickQuestListButton(int i)
    {
        this.listType = i;
        this.RefreshUI();
    }
    private void RefreshUI(Quest quest=null)
    {
        ClearAllUI();
        InitQuestList(listType);
    }



    private void ClearAllUI()
    {
        questList.RemoveAll();
    }
    private void InitQuestList(int type)
    {
        foreach (Quest quest in QuestManager.Instance.allQuests.Values)
        {
            if (type == 0)
            {
                if (quest.Info == null)
                    continue;
                if (quest.Info.Status == QuestStatus.Finished) continue;
            }
            else if (type == 1)
            {
                if (quest.Info != null)
                    continue;
            }
            GameObject go = GameObject.Instantiate(UIQuestItemPrefab, questList.transform, false);
            var ui = go.GetComponent<UIQuestItem>();
            ui.SetQuest(this.questList, quest);

        }

    }
    private void OnQuestSelected(ListView.ListViewItem arg0)
    {
        UIQuestItem item = (UIQuestItem)arg0;
        this.QuestAttributeUI.gameObject.SetActive(true);
        this.QuestAttributeUI.SetQuest(item.quest);
    }
}
