using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class EquipDefine
    {
        public int ID { get; set; }
        public EquipSlot Slot { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        public float HP { get; set; }
        public float MP { get; set; }
        /// <summary>
        /// 力量
        /// </summary>
        public float STR { get; set; }  
        /// <summary>
        /// 智力
        /// </summary>
        public float INT { get; set; }  
        /// <summary>
        /// 敏捷
        /// </summary>
        public float DEX { get; set; }  
        /// <summary>
        /// 物理攻击
        /// </summary>
        public float AD { get; set; }  
        /// <summary>
        /// 法术攻击
        /// </summary>
        public float AP { get; set; }  
        /// <summary>
        /// 物理防御
        /// </summary>
        public float DEF { get; set; }  
        /// <summary>
        /// 法术防御
        /// </summary>
        public float MDEF { get; set; }  
        /// <summary>
        /// 攻击速度
        /// </summary>
        public float SPD { get; set; }  
        /// <summary>
        /// 暴击率
        /// </summary>
        public float CRI { get; set; }

        private static Dictionary<EquipSlot, string> equipSlot_dic;
        public static Dictionary<EquipSlot, string> EquipSlot_Dic
        {
            get
            {
                if(equipSlot_dic==null)
                {
                    equipSlot_dic = new Dictionary<EquipSlot, string>();
                    equipSlot_dic[(EquipSlot)0] = "武器";
                    equipSlot_dic[(EquipSlot)1] = "副手";
                    equipSlot_dic[(EquipSlot)2] = "头部";
                    equipSlot_dic[(EquipSlot)3] = "胸甲";
                    equipSlot_dic[(EquipSlot)4] = "护肩";
                    equipSlot_dic[(EquipSlot)5] = "裤子";
                    equipSlot_dic[(EquipSlot)6] = "靴子";
                }
                return equipSlot_dic;
            }
        }

    }
}
