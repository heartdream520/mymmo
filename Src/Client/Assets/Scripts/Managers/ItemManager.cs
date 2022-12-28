using Assets.Scripts.Models;
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
        }
        public ItemDefine GetInfoDefine(int ItemId)
        {
            return null;
        }
        public bool UseItem(int itemID)
        {
            return false;
        }
        public bool UseItem(Item item)
        {
            return false;
        }
    }
}
