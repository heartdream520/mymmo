using Assets.Scripts.Managers;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Models
{
    class Item
    {
        public int id;
        public int count;
        public ItemDefine define;
        public Item(NItemInfo info)
        {
            this.id = (short)info.Id;
            this.count = (short)info.Count;
            this.define = DataManager.Instance.Items[info.Id];
        }
        public Item(int id,int count)
        {
            this.id = id;
            this.count = count;
            this.define = DataManager.Instance.Items[this.id];
        }
        public override string ToString()
        {
            return string.Format("ItemID:{0},Count:{1}",id,count);
        }
    }
}
