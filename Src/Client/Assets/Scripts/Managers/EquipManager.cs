using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using SkillBridge.Message;

namespace Assets.Scripts.Managers
{
    class EquipManager : Singleton<EquipManager>
    {

        public delegate void OnEquipChangeHandler();
        public event OnEquipChangeHandler OnEquipChanged;
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        byte[] Data;

        public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(this.Data);
        }

        /// <summary>
        /// 根据装备数据更新装备
        /// </summary>
        /// <param name="data">装备数据</param>
        unsafe private void ParseEquipData(byte[] data)
        {
            fixed(byte* pt =this.Data)
            {
                for(int i=0;i<this.Equips.Length;i++)
                {
                    int itemId = *(int*)(pt + (i * sizeof(int)));
                    if (itemId > 0)
                        this.Equips[i] = ItemManager.Instance.Items[itemId];
                    else this.Equips[i] = null;
                }
            }
        }
        /// <summary>
        /// 判断这件装备是否被装备
        /// </summary>
        /// <param name="equipId">Item ID</param>
        /// <returns></returns>
        public bool Contains(int equipId)
        {
            for(int i=0;i<this.Equips.Length;i++)
            {
                if (this.Equips[i] != null && this.Equips[i].id == equipId)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取装备数据
        /// </summary>
        /// <returns></returns>
        unsafe public byte[] GetEquipData()
        {
            fixed(byte* pt = Data)
            {
                for(int i=0;i<(int)EquipSlot.SlotMax;i++)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if (this.Equips[i] == null)
                        *itemId = 0;
                    else *itemId = this.Equips[i].id;
                }
            }
            return this.Data;
        }
        public void EquipItem(Item equip)
        {
            ItemServicer.Instance.send_equip_Item(equip, true);
        }
        public void unEquipItem(Item equip)
        {
            ItemServicer.Instance.send_equip_Item(equip, false);
        }
        public void OnEquipItem(Item equip)
        {
            if(this.Equips[(int)equip.equipDefine.Slot]!=null && this.Equips[(int)equip.equipDefine.Slot].id==equip.id)
            {
                return;
            }
            this.Equips[(int)equip.equipDefine.Slot] = ItemManager.Instance.Items[equip.id];
            if (OnEquipChanged != null)
                OnEquipChanged();



        }

        internal Item GetEquip(EquipSlot slot)
        {
            return this.Equips[(int)slot];
        }

        public void OnUnEquipItem(EquipSlot slot)
        {
            if(this.Equips[(int)slot]!=null)
            {
                this.Equips[(int)slot] = null;
                if (OnEquipChanged != null)
                    OnEquipChanged();
            }
        }
    }
}
