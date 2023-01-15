using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;
        public Quest()
        {

        }
        public Quest(NQuestInfo info)
        {
            Debug.LogError(info.QuestId);
            this.Info = info;
            this.Define = DataManager.Instance.Quests[info.QuestId];
        }
        public Quest(QuestDefine define)
        {
            this.Define = define;
            this.Info = null;
        }
        public string GetTypeName()
        {
            if (Define.Type == QuestType.Main) return "主线";
            if (Define.Type == QuestType.Branch) return "支线";
            //return EnumUtil
            return "name";
        }
    }
}
