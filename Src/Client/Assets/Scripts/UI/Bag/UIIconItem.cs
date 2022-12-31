using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour {

    public Image mainImage;
    public Image SecondImage;
    public Text mainText;

    internal void SetMainIcom(string icon, string v)
    {
        Sprite sprite= Resloader.Load<Sprite>(icon);
        this.mainImage.overrideSprite = sprite;
        this.mainText.text = v;
    }
}
