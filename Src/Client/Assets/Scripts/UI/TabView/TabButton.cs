﻿using Assets.Scripts.UI.Set;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour {

    public Sprite activeImage;
    private Sprite normalImage;

    [HideInInspector]
    public TabView tabView;

    [HideInInspector]
    public int tabIndex = 0;
    [HideInInspector]
    public bool selected = false;

    private Image tabImage;

	// Use this for initialization
	void Start () {
        tabImage = this.GetComponent<Image>();
        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
	}

    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }

    void OnClick()
    {
        SoundManager.Instance.PlayerSound(SoundDefine.UI_Click);
        this.tabView.SelectTab(this.tabIndex);
    }

}
