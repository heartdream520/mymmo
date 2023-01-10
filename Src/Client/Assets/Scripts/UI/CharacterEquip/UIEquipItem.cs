using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text name;
    public Text level;
    public Text limitClass;
    public Text limitCategory;
    public Image backGround;
    public Sprite normalBg;
    public Sprite selectedBg;


    [HideInInspector]
    public Item Item;
    public int index { get; set; }
    private bool isEquiped = false;
    private UICharEquip owner;

    public Text equip_Slot;
    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (value)
                owner.Selected_UIEquipItem = this;
            this.backGround.overrideSprite = value ? selectedBg : normalBg;
        }
    }


    public void SetEquipItem(int id, Item equip, UICharEquip owner, bool is_equip)
    {
        this.owner = owner;
        this.index = id;
        this.Item = equip;
        this.isEquiped = is_equip;

        if (name != null) this.name.text = this.Item.itemDefine.Name;
        if (level != null) this.level.text = this.Item.itemDefine.Level.ToString();
        if (limitClass != null) this.limitClass.text = this.Item.itemDefine.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = this.Item.equipDefine.Category.ToString();
        EquipSlot slot= this.Item.equipDefine.Slot;
        if (this.equip_Slot != null) this.equip_Slot.text = EquipDefine.EquipSlot_Dic[slot];

        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.Item.itemDefine.Icon);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            if (this.Selected)
            {
                UnEquip();
                this.Selected = false;
            }
            else Selected = true;
        }
        else
        {
            if (this.Selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else this.Selected = true;
        }
    }

    private void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("要取下装备[{0}]吗？", this.Item.itemDefine.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
          {
              this.owner.UnEquip(this.Item);
          };

    }

    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备[{0}]吗？", this.Item.itemDefine.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
          {
              var oldEquip = EquipManager.Instance.GetEquip(this.Item.equipDefine.Slot);
              if(oldEquip != null )
              {
                  var newmsg = MessageBox.Show(string.Format("要替换装备[{0}]吗？", oldEquip.itemDefine.Name), "确认", MessageBoxType.Confirm);
                  newmsg.OnYes = () =>
                    {
                        this.owner.DoEquip(this.Item);
                    };
              }
              else
              {
                  this.owner.DoEquip(this.Item);
              }
          };
    }
}