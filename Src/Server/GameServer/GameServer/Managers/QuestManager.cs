using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class QuestManager
    {
        public Character Owner;

        public QuestManager(Character character)
        {
            this.Owner = character;
        }

        internal void GetQuestInfos(List<NQuestInfo> quests)
        {
            foreach(var quest in this.Owner.Data.Quests)
            {
                quests.Add(this.GetQuestInfo(quest));
            }
        }

        private NQuestInfo GetQuestInfo(TCharacterQueat quest)
        {
            return new NQuestInfo()
            {
                QuestId = quest.QuestID,
                QuestGuid = quest.Id,
                Status = (QuestStatus)quest.Status,
                Targets = new int[3]
                {
                    quest.Target1,
                    quest.Target2,
                    quest.Target3
                }
            };
        }

        internal Result AcceptQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;
            if(DataManager.Instance.Quests.TryGetValue(questId,out quest))
            {
                var Dquset = DBService.Instance.Entities.TCharacterQueats.Create();

                Dquset.QuestID = questId;
                if(quest.Target1 == QuestTarget.None)
                {
                    //没有目标，直接完成
                    Dquset.Status = (int)QuestStatus.Complated;
                }
                else
                {
                    //有目标
                    Dquset.Status = (int)QuestStatus.InProgress;
                }
                sender.Session.Response.questAccept.Quest = this.GetQuestInfo(Dquset);
                character.Data.Quests.Add(Dquset);
                DBService.Instance.Save();
                return Result.Success;
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "接受任务不存在！";
                Log.ErrorFormat("接受任务不存在！");
                return Result.Failed;
            }
            
        }

        internal Result SubmitQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;
            if (DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var Dquset = character.Data.Quests.Where(q => q.QuestID == questId).FirstOrDefault();
                if(Dquset!=null)
                {
                    if(Dquset.Status!=(int)QuestStatus.Complated)
                    {
                        sender.Session.Response.questAccept.Errormsg = "任务未完成！";
                        return Result.Failed;
                    }
                    Dquset.Status = (int)QuestStatus.Finished;
                    sender.Session.Response.questSubmit.Quest = this.GetQuestInfo(Dquset);
                    DBService.Instance.Save();

                    //处理奖励
                    if(quest.RewardGold>0)
                    {
                        character.Gold += quest.RewardGold;
                    }
                    if(quest.RewardExp>0)
                    {
                        //character.Exp += quest.RewardExp;
                    }
                    if(quest.RewardItem1>0)
                    {
                        character.itemManager.AddItem(quest.RewardItem1, quest.RewardItem1Count);
                    }
                    if(quest.RewardItem2>0)
                    {
                        character.itemManager.AddItem(quest.RewardItem2, quest.RewardItem2Count);
                    }
                    if(quest.RewardItem3>0)
                    {
                        character.itemManager.AddItem(quest.RewardItem3, quest.RewardItem3Count);
                    }
                    DBService.Instance.Save();
                    return Result.Success;
                }
                else
                {
                    sender.Session.Response.questAccept.Errormsg = "提交任务不存在[0]！";
                    Log.ErrorFormat("提交任务不存在[0]！");
                    return Result.Failed;
                }
               
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "提交任务不存在[1]！";
                Log.ErrorFormat("提交任务不存在[1]！");
                return Result.Failed;
            }
        }
    }
}
