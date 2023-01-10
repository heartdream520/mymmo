using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour {
    
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler Onclose;
    public  virtual System.Type Type { get { return this.GetType(); } }
    public enum WindowResult
    {
        None,Yes,No
    }
    public void Close (WindowResult result= WindowResult.None)
    {
        UIManager.Instance.Close(this.Type);
        if (this.Onclose != null)
            this.Onclose(this,result);
        this.Onclose = null;
    }
    public virtual void OnClick_Close()
    {
        this.Close();
    }
    public virtual void OnClick_Yes()
    {
        this.Close(WindowResult.Yes);
    }
    private void OnDisable()
    {
        InDisable();
    }
    public virtual void InDisable()
    {

    }


}
