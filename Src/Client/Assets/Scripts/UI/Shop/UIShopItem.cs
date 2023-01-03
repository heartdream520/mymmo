using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems;

public class UIShopItem : MonoBehaviour,ISelectHandler
{

    public Image icon;
    public Text title;
    public Text price;
    public Text count;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.sprite = selected ? selectedBg : normalBg;
        }
    }
    public int ShopItemId { get; set; }
    private UIShop shop;
    private ItemDefine ItemDefine;
    private ShopItemDefine ShopItemDefine { get; set; }

    internal void SetShopItem(int id, ShopItemDefine shopItemDefine, UIShop owner)
    {
        this.ShopItemId = id;
        this.ShopItemDefine = shopItemDefine;
        this.shop = owner;
        this.ItemDefine = DataManager.Instance.Items[this.ShopItemDefine.ItemID];

        this.title.text = ItemDefine.Name;
        this.count.text = shopItemDefine.Count.ToString();
        this.price.text = shopItemDefine.Price.ToString();
        this.icon.overrideSprite = Resloader.Load<Sprite>(ItemDefine.Icon);

    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
