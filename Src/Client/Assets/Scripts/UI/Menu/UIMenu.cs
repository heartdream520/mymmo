using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenu : UIWindow, IDeselectHandler
{ 
    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData;
        if (ed.hovered.Contains(this.gameObject))
            return;
        this.OnClick_Close();
    }
    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.VEnable();
    }
    public virtual void VEnable()
    {

    }
}
