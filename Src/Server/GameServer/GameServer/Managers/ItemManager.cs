using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        Character owner;
        public Dictionary<int, Item> items = new Dictionary<int, Item>();
        public ItemManager(Character owner)
        {
            this.owner = owner;
            foreach(var item in owner.Data.Items)
            {
                this.items.Add(item.Id, new Item(item));
            }
        }
        public bool UseItem(int itemId,int count=1)
        {
            Log.InfoFormat("ItemManager->UseItem CharacterDID:{0} EID:{1} ItemID:{2} Count:{3}",
                owner.Data.ID,owner.entityId,itemId,count);
            Item item = null;
            if(this.items.TryGetValue(itemId,out item))
            {
                if (item.Count < count)
                    return false;

                //添加使用逻辑

                item.Remove(count);
                return true;

            }
            return false;
        }
        public bool HasItem(int itemID)
        {
            Item item = null;
            if (this.items.TryGetValue(itemID, out item))
                return item.Count > 0;
            return false;
        }
        public Item GetItem(int itemID)
        {
            Item item = null;
            this.items.TryGetValue(itemID, out item);
            return item;
        }
        public bool AddItem(int itemID,int count)
        {
            Log.InfoFormat("ItemManager->AddItem CharacterDID:{0} EID:{1} ItemID:{2} Count:{3}",
                owner.Data.ID, owner.entityId, itemID, count);
            Item item = null;
            if(this.items.TryGetValue(itemID,out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbitem = new TCharacterItem();
                dbitem.ItemCount = count;
                dbitem.ItemID = itemID;
                dbitem.TCharacterID = owner.Data.ID;
                dbitem.Owner = owner.Data;

                owner.Data.Items.Add(dbitem);
                Item ite = new Item(dbitem);
                this.items.Add(itemID,ite);

            }
            //DBService.Instance.Save();
            return true;
        }
        public bool RemoveItem(int itemID,int count)
        {
            Log.InfoFormat("ItemManager->RemoveItem CharacterDID:{0} EID:{1} ItemID:{2} Count:{3}",
               owner.Data.ID, owner.entityId, itemID, count);
            if (!this.items.ContainsKey(itemID)) return false;
            Item item = this.items[itemID];
            if (item.Count < count) return false;
            item.Remove(count);
            //DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo>list)
        {
            foreach(var item in this.items)
            {
                list.Add(new NItemInfo()
                {
                    Id = item.Value.ItemID, Count = item.Value.Count
                });
            }
        }
    }
}
