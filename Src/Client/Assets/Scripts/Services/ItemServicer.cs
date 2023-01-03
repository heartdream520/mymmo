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
    }



    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnBuyItem);
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
}
