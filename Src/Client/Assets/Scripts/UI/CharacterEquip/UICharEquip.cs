using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharEquip : UIWindow {
    public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipPrefab;

    public Transform ItemListRoot;

    public Text CLvName;
    public List<Transform> Slots;

    private UIEquipItem selected_UIEquipItem;

    public GameObject Char_attribute_Panel;
    public GameObject Equip_attribute_Panel;
    public UIEquipItem Selected_UIEquipItem
    {
        set
        {
            if(selected_UIEquipItem!=null)
                selected_UIEquipItem.Selected = false;
            this.selected_UIEquipItem = value;
            if(value==null)
            {
                this.Char_attribute_Panel.SetActive(true);
                this.Equip_attribute_Panel.SetActive(false);
                
            }
            else
            {
                this.Char_attribute_Panel.SetActive(false);
                this.Equip_attribute_Panel.SetActive(true);
                this.Equip_attribute_Panel.GetComponent<Equip_attribute>().set_Equip_attribute(value.Item);
            }
        }
    }
    private void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += this.RefreshUI;

    }
    private void OnDisable()
    {
        EquipManager.Instance.OnEquipChanged -= this.RefreshUI;

    }

    private void RefreshUI()
    {
        this.Selected_UIEquipItem = null;

        ClearAllEquipList();
        InitAllEquipItem();
        ClearEquipList();
        InitEquipItem();
        if (this.money != null)
            this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
        if (CLvName != null)
            CLvName.text = "Lv." + User.Instance.CurrentCharacter.Level.ToString() + " " + User.Instance.CurrentCharacter.Name;
    }
    private void ClearAllEquipList()
    {
        for(int i=0;i<ItemListRoot.transform.childCount;i++)
        {
            Destroy(ItemListRoot.transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 初始化装备列表
    /// </summary>
    private void InitAllEquipItem()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if(kv.Value.itemDefine.Type== ItemType.Equip &&kv.Value.itemDefine.LimitClass==User.Instance.CurrentCharacter.Class)
            {
                Item equip = kv.Value;
                if (EquipManager.Instance.Contains(equip.id))
                    continue;

                GameObject go = Instantiate(this.itemPrefab, this.ItemListRoot);
                UIEquipItem uI = go.GetComponent<UIEquipItem>();
                uI.SetEquipItem(equip.id, equip, this, false);

            }
        }
    }

    private void ClearEquipList()
    {
        for(int i=0;i<Slots.Count;i++)
        {
            foreach (var it in Slots[i].transform.GetComponentsInChildren<UIEquipItem>())
            {
                Destroy(it.gameObject);
            }
        }
    }
    /// <summary>
    /// 初始化已经装备的列表
    /// </summary>
    private void InitEquipItem()
    { 
        for(int i=0;i<(int)EquipSlot.SlotMax;i++)
        {
            var item = EquipManager.Instance.Equips[i];
            if(item!=null)
            {
                GameObject go = Instantiate(this.itemEquipPrefab, this.Slots[i]);
                UIEquipItem uI = go.GetComponent<UIEquipItem>();
                uI.SetEquipItem(i, item, this, true);
            }
        }
    }
    public void DoEquip(Item equip)
    {
        EquipManager.Instance.EquipItem(equip);
    }
    public void UnEquip(Item equip)
    {
        EquipManager.Instance.unEquipItem(equip);

    }



}
