using Assets.Scripts.Managers;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    public class Item
    {
        public int id;
        public int count;
        public ItemDefine itemDefine;
        public EquipDefine equipDefine;
        public Item(NItemInfo info)
        {
            this.id = (short)info.Id;
            this.count = (short)info.Count;
            DataManager.Instance.Items.TryGetValue(info.Id, out this.itemDefine);
            DataManager.Instance.Equips.TryGetValue(this.id, out this.equipDefine);
        }
        public Item(int id, int count)
        {
            this.id = id;
            this.count = count;
            DataManager.Instance.Items.TryGetValue(this.id, out this.itemDefine);
            DataManager.Instance.Equips.TryGetValue(this.id, out this.equipDefine);
        }
        public override string ToString()
        {
            return string.Format("ItemID:{0},Count:{1}",id,count);
        }
    }
}
