using Assets.Scripts.Models;
using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI.UIQuest
{
    class UIQuestDialog : UIWindow
    {
        public Quest quest;

        public Text questName;
        public UIQuestAttribute uIQuestAttribute;
        public GameObject acceptButton;
        public GameObject submitButton;
        internal void SetQuest(Models.Quest quest)
        {
            this.quest = quest;
            this.questName.text = string.Format("[{0}]{1}", this.quest.GetTypeName(),this.quest.Define.Name);
            if(this.quest.Info==null)
            {
                this.acceptButton.SetActive(true);
                this.submitButton.SetActive(false);
            }
            else
            {
                this.acceptButton.SetActive(false);
                this.submitButton.SetActive(true);
            }
            this.uIQuestAttribute.SetQuest(quest);
        }
    }
}
