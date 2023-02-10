using Assets.Scripts.UI.Set;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public Text Title;
    public Text money;
    public GameObject shopItem;
    ShopDefine shopDefine;

    public Transform itemRoot;
    public void Awake()
    {
        User.Instance.Gold_Change_Action += this.Chanage_gold_Text;
    }



    private void OnDisable()
    {
        User.Instance.Gold_Change_Action -= this.Chanage_gold_Text;
    }
    IEnumerator InitShop()
    {
        foreach(var kv in DataManager.Instance.ShopItems[shopDefine.ID])
        {
            if(kv.Value.Status>0)
            {

                //让商店不出售非此角色能够使用的物品
                ShopItemDefine define = kv.Value;
                ItemDefine item = DataManager.Instance.Items[define.ItemID];
                if ((int)item.LimitClass != 0 && (int)item.LimitClass != User.Instance.CurrentCharacter.ConfigId)
                    continue;


                GameObject go = Instantiate(shopItem, itemRoot);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
            }
        }
        yield return null;
    }
    public void SetShop(ShopDefine shopDefine)
    {
        this.shopDefine = shopDefine;
        this.Title.text = shopDefine.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();

        StartCoroutine(InitShop());
    }
    private UIShopItem selectedItem;
    public void SelectShopItem(UIShopItem uIShopItem)
    {
        if (selectedItem != null)
            selectedItem.Selected = false;
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        selectedItem = uIShopItem;
    }
    public void OnChickBuy()
    {
        if(this.selectedItem==null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }
        if(!ShopManager.Instance.BuyItem(shopDefine.ID, selectedItem.ShopItemId))
        {
            MessageBox.Show("购买失败！");
        }

    }
    private void Chanage_gold_Text(long count)
    {
        if (!this.gameObject) return;
        this.money.text = count.ToString();
    }
}
