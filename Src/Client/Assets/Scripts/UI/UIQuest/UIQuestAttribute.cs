using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestAttribute : MonoBehaviour {


    [Header("物品预设体")]
    public GameObject itemPrefab;
    public Text questName;
    public Text overview;
    public Text diaLog;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public Text goldText;
    public Text expText;
    public GameObject[] Buttons;
    public void SetQuest(Quest quest)
    {
        QuestDefine define = quest.Define;

        if (this.questName != null)
            this.questName.text = define.Name;
        if (this.overview != null)
            this.overview.text = quest.Define.Overview;
        if (this.diaLog != null)
        {

            if (quest.Info == null)
                this.diaLog.text = quest.Define.Dialog;
            else this.diaLog.text = quest.Define.DialogFinish;
        }
            

        goldText.text = quest.Define.RewardGold.ToString();
        goldText.gameObject.SetActive(quest.Define.RewardGold == 0 ? false : true);
        expText.text = quest.Define.RewardExp.ToString();
        expText.gameObject.SetActive(quest.Define.RewardExp == 0 ? false : true);

        if(quest.Define.RewardItem1==0)
        {
            this.item1.transform.parent.gameObject.SetActive(false);
        }
        else this.item1.transform.parent.gameObject.SetActive(true);
        setItem(item1,define.RewardItem1, define.RewardItem1Count);
        setItem(item2,define.RewardItem2, define.RewardItem1Count);
        setItem(item3,define.RewardItem3, define.RewardItem1Count);

        if (quest.Info == null) setButton(0);  //领取
        else if(quest.Info.Status==SkillBridge.Message.QuestStatus.Complated) setButton(1);
        else
            setButton(-1);

    }
    private void setButton(int id)
    {
        for (int i = 0; i < Buttons.Length; i++)
            Buttons[i].SetActive(i == id);
    }
    private void setItem(GameObject itemBox,int itemId,int num)
    {
        for (int i = 0; i < itemBox.transform.childCount; i++)
            Destroy(itemBox.transform.GetChild(i).gameObject);
        itemBox.SetActive(itemId == 0 ? false : true);
        if (itemId == 0) return;
        ItemDefine item = null;
        if(DataManager.Instance.Items.TryGetValue(itemId,out item))
        {
            GameObject go = GameObject.Instantiate(this.itemPrefab, itemBox.transform, false);
            go.GetComponent<UIIconItem>().SetMainIcom(item.Icon, num.ToString());
        }
        else
        {
            Debug.LogError("任务奖励物品不存在！");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">0 未前往领取  1为前往提交</param>
    public void OnchickButton(int id)
    {

    }
}
