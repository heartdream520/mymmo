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
        public Item(NItemInfo info)
        {
            this.id = (short)info.Id;
            this.count = (short)info.Count;
        }
        public override string ToString()
        {
            return string.Format("ItemID:{0},Count:{1}",id,count);
        }
    }
}
