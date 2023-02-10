﻿using Assets.Scripts.UI.Set;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabView : MonoBehaviour
{

    public TabButton[] tabButtons;
    public GameObject[] tabPages;

    public UnityAction<int> OnTabSelect;

    public int index = -1;
    // Use this for initialization
    IEnumerator Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(0);
    }

    private int selected_Idx = -1;
    public void SelectTab(int index, bool action = true)
    {
        if (this.index != index)
        {
            
            selected_Idx = index;
            for (int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].Select(i == index);

                if (this.tabPages.Length == 1)
                    this.tabPages[0].SetActive(true);
                else
                    if (i < tabPages.Length)
                    tabPages[i].SetActive(i == index);
            }

            if (action)
                if (OnTabSelect != null)
                    OnTabSelect(index);
        }
    }
}
