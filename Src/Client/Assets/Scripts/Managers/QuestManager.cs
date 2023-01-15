using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
using Assets.Scripts.UI.UIQuest;
using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Managers
{
    public enum NpcQuestStatus
    {
        None = 0,   //无任务
        Complete,   //有已完成可提交的任务
        Available,  //拥有可接受任务
        Incomplete  //拥有未完成任务
    }
    class QuestManager : Singleton<QuestManager>
    {
        /// <summary>
        /// 所有有效任务
        /// </summary>
        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        /// <summary>
        /// 某个NPC具有某种类型的任务
        /// </summary>
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests 
            = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public UnityAction<Quest> OnQuestStatusChangeAction;

        
        public void Init(List<NQuestInfo> quests)
        {
            this.OnQuestStatusChangeAction += this.setPostQuest;
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            this.InitQuests();
        }



        private void InitQuests()
        {
            //初始化所有任务
            foreach(var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuests[quest.Info.QuestId] = quest;
            }
            checkAvailableQuests();
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            List<Quest> completes;   //已完成任务
            List<Quest> availables;  //可接受任务
            List<Quest> incompletes; //进行中任务

            if(!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete,out completes))
            {
                completes = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Complete] = completes;
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }

            //info为空说明任务还未接取
            if (quest.Info == null)
            {
                if(npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (quest.Info.Status == QuestStatus.Complated && npcId == quest.Define.SubmitNPC 
                    && !this.npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                }
                if (quest.Info.Status == QuestStatus.InProgress && npcId == quest.Define.SubmitNPC
                    && !this.npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                }
            }
        }
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status;
            if(this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return NpcQuestStatus.Complete;
                if (status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }
        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();

            if(this.npcQuests.TryGetValue(npcId,out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return this.ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return this.ShowQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return this.ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }

        private bool ShowQuestDialog(Quest quest)
        {
            if(quest.Info==null||quest.Info.Status==QuestStatus.Complated)
            {
                UIQuestDialog uI = UIManager.Instance.Show<UIQuestDialog>();
                uI.SetQuest(quest);
                uI.Onclose += this.OnQuestDialogClose;
                return true;
            }
            if(quest.Info!=null|| quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }

            return true;
        }

        public void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog uIQuest = (UIQuestDialog)sender;
            if(result==UIWindow.WindowResult.Yes)
            {
                if (uIQuest.quest.Info == null)
                    QuestService.Instance.sendQuestAccept(uIQuest.quest);
                else if (uIQuest.quest.Info.Status == QuestStatus.Complated)
                    QuestService.Instance.sendQuestSubmit(uIQuest.quest);

                //MessageBox.Show(uIQuest.quest.Define.DialogAccept);
            }
            else if(result==UIWindow.WindowResult.No)
            {
                MessageBox.Show(uIQuest.quest.Define.DialogDeny);
            }

        }
        /// <summary>
        /// 刷新所有任务状态
        /// </summary>
        /// <param name="info">新加的任务</param>
        /// <returns>新加的任务</returns>
        private Quest RefreshQuestStatus(NQuestInfo info)
        {
            this.npcQuests.Clear();
            Quest result;
            if(this.allQuests.ContainsKey(info.QuestId))
            {
                this.allQuests[info.QuestId].Info = info;
                result = this.allQuests[info.QuestId];
            }
            else
            {
                result = new Quest(info);
                this.allQuests[info.QuestId] = result;
            }
            checkAvailableQuests();
            foreach(var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
            if (this.OnQuestStatusChangeAction != null)
                OnQuestStatusChangeAction(result);
            return result;
        }

        private void checkAvailableQuests()
        {
            //初始化所有可用任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                QuestDefine questDefine = kv.Value;
                checkAvailableQuest(questDefine,false);
            }
        }
        private void setPostQuest(Quest arg0)
        {
            Quest thisQuest = arg0;
            if (thisQuest.Define.PostQuest == 0) return;
            if (thisQuest.Info != null && thisQuest.Info.Status == QuestStatus.Finished
                        && !this.allQuests.ContainsKey(thisQuest.Define.PostQuest))
            {
                this.checkAvailableQuest(DataManager.Instance.Quests[thisQuest.Define.PostQuest], true);
            }
        }

        private void checkAvailableQuest(QuestDefine questDefine,bool is_post)
        {
            if (questDefine.LimitClass != CharacterClass.None && questDefine.LimitClass != User.Instance.CurrentCharacter.Class)
                return;
            if (questDefine.LimitLevel > User.Instance.CurrentCharacter.Level)
                return;
            if (allQuests.ContainsKey(questDefine.ID))
                return;
            if (!is_post && questDefine.PreQuest > 0)
            {
                Quest preQuest;
                if (allQuests.TryGetValue(questDefine.PreQuest, out preQuest))
                {
                    if (preQuest.Info == null)
                        return;  //前置任务未接取
                    if (preQuest.Info.Status != QuestStatus.Finished)
                        return;  //前置任务未完成
                }
                else
                    return; //前置任务未接
            }
            Quest quest = new Quest(questDefine);
            this.AddNpcQuest(quest.Define.AcceptNPC, quest);
            this.AddNpcQuest(quest.Define.SubmitNPC, quest);
            this.allQuests[quest.Define.ID] = quest;
        }

        //service接受任务成功进行引用
        public void OnQuestAccepted(NQuestInfo info)
        {
            Quest quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }
        public void OnQuestSubmited(NQuestInfo info)
        {
            Quest quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }
    }
}
