using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class ItemManager:Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public void Init(List<NItemInfo> list)
        {
            this.Items.Clear();
            foreach(var it in list)
            {
                Item item = new Item(it);
                this.Items.Add(it.Id, item);
                Debug.LogFormat("ItemManager->InitItem :{0}", item);
            }
            StatusServicer.Instance.RegisterStatusNofity(StatusType.Item,OnItemNotify);
        }



        public ItemDefine GetInfoDefine(int ItemId)
        {
            return DataManager.Instance.Items[ItemId];
        }
        public bool UseItem(int itemID)
        {
            return false;
        }
        public bool UseItem(Item item)
        {
            return false;
        }
        private bool OnItemNotify(NStatus status)
        {
            if(status.Action==StatusAction.Add)
            {
                this.AddItem(status.Id, status.Value);
            }
            else if(status.Action==StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }


        private void AddItem(int id, int value)
        {
            Item item = null;
            if(this.Items.TryGetValue(id,out item))
            {
                item.count += value;

            }
            else
            {
                item = new Item(id, value);
                this.Items[id] = item;
            }
            BagManager.Instance.AddItem(id, value);
        }

        private void RemoveItem(int id, int value)
        {
            if(!this.Items.ContainsKey(id))
            {
                Debug.LogError("ItemManager->RemoveItem not exist Item:ID:"+id);
                return;
            }
            Item item = this.Items[id];
            if(item.count<value)
            {
                Debug.LogErrorFormat("ItemManager->RemoveItem Count not enough  ID:"+id);
                return;
            }
            item.count -= value;
            BagManager.Instance.RemoveItem(id, value);

        }
    }
}
