using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemServicer: Singleton<ItemServicer>,IDisposable
{
    

    // Use this for initialization
    public void Init () {
        MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnBuyItem);
        MessageDistributer.Instance.Subscribe<ItemEquipRespose>(this.OnItemEquip);
        
    }



    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnBuyItem);
        MessageDistributer.Instance.Unsubscribe<ItemEquipRespose>(this.OnItemEquip);
    }


    public void SendBuyItem(int shopId,int shopItemId)
    {
        Debug.Log("ItemServicer->SendBuyItem");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.itemBuy = new ItemBuyRequest();
        message.Request.itemBuy.shopId = shopId;
        message.Request.itemBuy.shopItemId = shopItemId;
        NetClient.Instance.SendMessage(message);
        

    }


    private void OnBuyItem(object sender, ItemBuyResponse message)
    {
        MessageBox.Show("购买结果:" + message.Result + "\n" + message.Errormsg, "购买完成");
    }
    private Item pendingEquip = null;
    private bool is_equip;
    public bool send_equip_Item(Item equip,bool is_equip)
    {
        if (pendingEquip != null)
            return false;
        Debug.LogFormat("ItemServicer->send_equip_Item Item:{0} is_equip:{1}",equip,is_equip);

        this.pendingEquip = equip;
        this.is_equip = is_equip;
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.itemEquip = new ItemEquipRequest();
        message.Request.itemEquip.isEquip = is_equip;
        message.Request.itemEquip.itemId = equip.id;
        message.Request.itemEquip.Slot = (int)equip.equipDefine.Slot;

        NetClient.Instance.SendMessage(message);


        return true;
    }
    private void OnItemEquip(object sender, ItemEquipRespose message)
    {
        if(message.Result==Result.Success)
        {
            if(pendingEquip!=null)
            {
                
                if(this.is_equip)
                {
                    EquipManager.Instance.OnEquipItem(pendingEquip);
                }
                else
                {
                    EquipManager.Instance.OnUnEquipItem(pendingEquip.equipDefine.Slot);
                }
                pendingEquip = null;
            }
        }
    }
}
