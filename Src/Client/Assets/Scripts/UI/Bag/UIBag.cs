using Assets.Scripts.Managers;
using Models;
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
    List<Image> slots;
    public void Start()
    {
        if (slots == null)
        {
            slots = new List<Image>();
            for (int page = 0; page < this.pages.Length; page++)
            {
                slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        StartCoroutine(InitBags());
    }

    IEnumerator InitBags()
    {
        for(int i=0;i<slots.Count;i++)
        {
            for (int j = 0; j < slots[i].transform.childCount; j++)
            {
                Destroy(slots[i].transform.GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < BagManager.Instance.items.Length; i++)
        {
            var item = BagManager.Instance.items[i];
            if (item.ItemId > 0)
            {
                GameObject go = Instantiate(bagItem, slots[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].define;
                ui.SetMainIcom(def.Icon, item.Count.ToString());
                go.SetActive(true);
            }
        }
        for (int i = BagManager.Instance.items.Length; i < slots.Count; i++)
        {
            slots[i].color = Color.gray;
        }

        yield return null;
    }
    public void setTitle(string title)
    {
        this.money.text = User.Instance.CurrentCharacter.Id.ToString();
    }
    public void OnReset()
    {
        BagManager.Instance.Reset();
        StartCoroutine(InitBags());
    }
}
