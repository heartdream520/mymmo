using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestStatus : MonoBehaviour {

    public Image[] statusImages;
    private NpcQuestStatus questStatus;

    internal void SetQuestStatus(NpcQuestStatus questStatus)
    {
        this.questStatus = questStatus;
        for(int i=0;i<4;i++)
        {
            if (statusImages[i] != null)
                this.statusImages[i].gameObject.SetActive(i == (int)questStatus);
        }
    }
}
