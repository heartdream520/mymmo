﻿
using Assets.Scripts.UI.Set;
using Assets.Scripts.UI.UIQuest;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

	public class UIElement
    {
        //资源所在地
        public string Resources;
        //是否全局
        public bool Cache;
        //对象实例
        public GameObject Instance;
        public bool is_shop;
    }
    public int UIcnt;
    public Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();
    public UIManager()
    {
        this.UIResources.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest", Cache = true, is_shop=true});
        this.UIResources.Add(typeof(UIBag), new UIElement() { Resources = "UI/Bag/UIBag", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIShop), new UIElement() { Resources = "UI/Shop/UIShop", Cache = false, is_shop = true });

        this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resources = "UI/Equip/UIEquip", Cache = false, is_shop = true });

        this.UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resources = "UI/Quest/UIQuestDialog", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resources = "UI/Quest/UIQuestSystem", Cache = false, is_shop = true });

        this.UIResources.Add(typeof(UIFriend), new UIElement() { Resources = "UI/Friend/UIFriend", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UITeam), new UIElement() { Resources = "UI/Team/UITeam", Cache = false, is_shop = false });

        this.UIResources.Add(typeof(UIGulid), new UIElement() { Resources = "UI/Gulid/UIGulid", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UICreatGulid), new UIElement() { Resources = "UI/Gulid/UICreatGulid", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIGulidList), new UIElement() { Resources = "UI/Gulid/UIGulidList", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UINoGulid), new UIElement() { Resources = "UI/Gulid/UINoGulid", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIGulidApplies), new UIElement() { Resources = "UI/Gulid/UIGulidApplies", Cache = false, is_shop = true });

        this.UIResources.Add(typeof(UISet), new UIElement() { Resources = "UI/Set/UISet", Cache = false, is_shop = true });

        this.UIResources.Add(typeof(UIChat), new UIElement() { Resources = "UI/Chat/UIChat", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIPopCharMenu), new UIElement() { Resources = "UI/Menu/UIPopCharMenu", Cache = false, is_shop = true });
        this.UIResources.Add(typeof(UIMusicSet), new UIElement() { Resources = "UI/Set/UIMusicSet", Cache = false, is_shop = true });
        User.Instance.CurrentCharacter_Set_Action += () =>
          {
              this.UIcnt = 0;
          };
    }
    ~UIManager()
    {

    }
    public T Show<T>()
    {
        //SoundManager.Instance.PlaySound("ui_open");
        Type type = typeof(T);

        
        if(this.UIResources.ContainsKey(type))
        {
            if (UIResources[type].is_shop)
                UIcnt++;
            UIElement info = this.UIResources[type];
            if(info.Instance!=null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if(prefab==null)
                {
                    Debug.LogErrorFormat("UIManager->Show<T> Type:{0} Resources:{1} not exist", type.Name,info.Resources);
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            SoundManager.Instance.PlayerSound(SoundDefine.UI_Win_Open);
            return info.Instance.GetComponent<T>();
        }
        Debug.LogErrorFormat("UIManager->Show<T> Type:{0} not exist", type.Name);
        return default(T);
    }
    public void Close(Type type)
    {
        if (UIResources[type].is_shop)
            UIcnt--;
        //SoundManager.Instance.PlaySound("ui_close");
        if (this.UIResources.ContainsKey(type))
        {
            SoundManager.Instance.PlayerSound(SoundDefine.UI_Win_Close);
            UIElement info = this.UIResources[type];
            if(info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }

        }
    }
    
}
