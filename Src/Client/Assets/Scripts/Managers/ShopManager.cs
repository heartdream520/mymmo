using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager :Singleton<ShopManager>
{ 
    /// <summary>
    /// init 注册NPC事件
    /// </summary>
	public void init()
    {
        NpcManager.Instance.RegisterNpcEnvent(NpcFunction.InvokeShop, OnOpenShop);
    }

    private bool OnOpenShop(NpcDefine npcDefine)
    {
        this.ShowShop(npcDefine.Param);
        return true;
    }

    private void ShowShop(int param)
    {
        ShopDefine shopDefine = null;
        if(DataManager.Instance.Shops.TryGetValue(param,out shopDefine))
        {
            UIShop uIShop = UIManager.Instance.Show<UIShop>();
            if(uIShop!=null)
            {
                uIShop.SetShop(shopDefine);
            }
        }
        else
        {
            Debug.LogErrorFormat("ShopManager->ShowShop Shop:{0} not exised",param);
        }
    }
    public bool BuyItem(int shopId,int shopitemid)
    {
        ItemServicer.Instance.SendBuyItem(shopId, shopitemid);
        return true;
    }
}
