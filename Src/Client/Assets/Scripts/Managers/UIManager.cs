using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>{

	class UIElement
    {
        //资源所在地
        public string Resources;
        //是否全局
        public bool Cache;
        //对象实例
        public GameObject Instance;
    }
    public int UIcnt;
    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();
    public UIManager()
    {
        this.UIResources.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest", Cache = true });
        this.UIResources.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = false });
        this.UIResources.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = false });

        this.UIResources.Add(typeof(UICharEquip), new UIElement() { Resources = "UI/UIEquip", Cache = false });
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
        UIcnt++;
        Type type = typeof(T);
        if(this.UIResources.ContainsKey(type))
        {
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
            return info.Instance.GetComponent<T>();
        }
        Debug.LogErrorFormat("UIManager->Show<T> Type:{0} not exist", type.Name);
        return default(T);
    }
    public void Close(Type type)
    {
        UIcnt--;
        //SoundManager.Instance.PlaySound("ui_close");
        if (this.UIResources.ContainsKey(type))
        {
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
