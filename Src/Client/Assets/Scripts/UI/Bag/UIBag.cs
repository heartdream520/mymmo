using Assets.Scripts.Managers;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : UIWindow
{
    public Text money;
    [Header("所有的页面")]
    public Transform[] pages;

    [Header("放在背包格子上")]
    public GameObject bagItem;
    List<Image>[] slots;
    int[] cnt = new int[4]; 
    public void Awake()
    {
        User.Instance.Gold_Change_Action += this.Chanage_gold_Text;
    }
    private void OnDisable()
    {
        User.Instance.Gold_Change_Action -= this.Chanage_gold_Text;
    }
    public void Start()
    {
        if (slots == null)
        {
            slots = new List<Image>[4];
            for (int page = 0; page < this.pages.Length; page++)
            {
                slots[page] = new List<Image>();
                slots[page].AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        StartCoroutine(InitBags());
    }

    IEnumerator InitBags()
    {

        for (int i = 0; i < 4; i++) cnt[i] = 0;
        for (int i = 0; i < BagManager.Instance.items.Length; i++)
        {
            var item = BagManager.Instance.items[i];
           

            if (item.ItemId > 0)
            {
                int itemType = (int)DataManager.Instance.Items[item.ItemId].Type - 1;
                GameObject go = Instantiate(bagItem, slots[itemType][cnt[itemType]++].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].itemDefine;
                ui.SetMainIcom(def.Icon, item.Count.ToString());
                go.SetActive(true);
            }
        }
        for(int i=0;i<4;i++)
        {
            for(int j=cnt[i];j<slots[i].Count;j++)
            {
                slots[i][j].color = Color.gray;
            }
        }
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
        yield return null;
    }
    public void setTitle(string title)
    {
        this.money.text = User.Instance.CurrentCharacter.EnityId.ToString();
    }
    private void clear_Bag()
    {
        for(int k=0;k<4;k++)
        {
            for (int i = 0; i < slots[k].Count; i++)
            {
                for (int j = 0; j < slots[k][i].transform.childCount; j++)
                {
                    Destroy(slots[k][i].transform.GetChild(j).gameObject);
                }
            }
        }
        
    }
    public void OnReset()
    {
        BagManager.Instance.Reset();
        this.clear_Bag();
        StartCoroutine(InitBags());
    }
    private void Chanage_gold_Text(long count)
    {
        if (!this.gameObject) return;
        this.money.text = count.ToString();
    }
}
