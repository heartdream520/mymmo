using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : Singleton<BagManager>
{
    public int Unlocked;
    public BagItem[] items;
    NBagInfo info;
    unsafe public void Init(NBagInfo info)
    {
        this.info = info;
        this.Unlocked = info.Unlocked;
        items = new BagItem[this.Unlocked];
        if (info.Items != null && info.Items.Length >= this.Unlocked)
        {
            Analyze(info.Items);
        }
        else
        {
            info.Items = new byte[sizeof(BagItem) * this.Unlocked];
            Reset();
        }
    }

    public void Reset()
    {
        int i = 0;
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.count<kv.Value.define.StackLimit)
            {
                this.items[i].ItemId = (ushort)kv.Key;
                this.items[i].Count = (ushort)kv.Value.count;
            }
            else
            {
                int count = kv.Value.count;
                while(count>kv.Value.define.StackLimit)
                {
                    this.items[i].ItemId = (ushort)kv.Key;
                    this.items[i].Count = (ushort)kv.Value.define.StackLimit;
                    i++;
                    count -= kv.Value.define.StackLimit;
                }
                this.items[i].ItemId = (ushort)kv.Key;
                this.items[i].Count = (ushort)count;
            }
            i++;
        }
        BagService.Instance.SendBagSave(this.GetBagInfo());
    }

    /// <summary>
    /// 分析获取背包
    /// </summary>
    /// <param name="data"></param>
    unsafe public void Analyze(byte[] data)
    {
        fixed (byte* pt = data)
        {
            for(int i=0;i<this.Unlocked;i++)
            {
                BagItem* item = (BagItem*)(pt + (i * sizeof(BagItem)));
                this.items[i] = *item;
            }
        }
    }



    unsafe public NBagInfo GetBagInfo()
    {
        //Debug.LogError("BagManager->NBagInfo");
        fixed (byte* pt = info.Items)
        {
            for (int i = 0; i < this.Unlocked; i++)
            {
                BagItem* item = (BagItem*)(pt + (i * sizeof(BagItem)));
                *item = items[i];
            }
        }
        return this.info;
    }



    internal void AddItem(int id, int count)
    {
        int limit = DataManager.Instance.Items[id].StackLimit;
        for (int i = 0; i < Unlocked; i++)
        {
            BagItem bagItem = items[i];
            if (bagItem.ItemId == id && bagItem.Count < limit)
            {
                int max = limit - bagItem.Count;
                if (count <= max)
                {
                    items[i].Count += (ushort)count;
                    return;
                }
                else
                {
                    items[i].Count = (ushort)limit;
                    count -= max;
                }
            }
            if (bagItem.ItemId == 0)
            {
                if (count <= limit)
                {
                    items[i].ItemId = (ushort)id;
                    items[i].Count = (ushort)count;
                    return;
                }
                else
                {
                    items[i].ItemId = (ushort)id;
                    items[i].Count = (ushort)count;
                    count -= limit;
                }
            }
        }
        MessageBox.Show("背包已满！");

    }

    internal void RemoveItem(int id, int value)
    {
        throw new NotImplementedException();
    }

}

