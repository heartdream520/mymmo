using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum ItemFunction
    {
        RecoverHP,
        RecoverMP,
        AddBuff,
        AddExp,
        AddMoney,
        AddItem,
        AddSkillPoint
    }

    public class ItemDefine
    {
       // ID Name    Description Type    Category CanUse  UseCD Price   SellPrice Function

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        public bool CanUse { get; set; }
        public int Price { get; set; }
        public float UseCD { get; set; }
        
        public int SellPrice { get; set; }
        /// <summary>
        /// 堆叠限制
        /// </summary>
        public int StackLimit { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 功能
        /// </summary>
        public ItemFunction Function { get; set; }
        public int Param { get; set; }
        public List<int> Params { get; set; }


    }
}
