using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using Models;
using SkillBridge.Message;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;

namespace Assets.Scripts.Services
{
    class QuestService : Singleton<QuestService>, IDisposable
    {
        public void Init() { }
        public QuestService()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptRespose>(this.OnQuestAccept);
            MessageDistributer.Instance.Subscribe<QuestSubmitRespose>(this.OnQuestSubmit);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptRespose>(this.OnQuestAccept);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitRespose>(this.OnQuestSubmit);
        }
        public void sendQuestAccept(Quest quest)
        {
            Debug.LogFormat("QuestService->sendQuestAccept QuestID:{0}",quest.Define.ID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
        }
        public void sendQuestSubmit(Quest quest)
        {
            Debug.LogFormat("QuestService->sendQuestSubmit QuestID:{0}", quest.Define.ID);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
        }

        private void OnQuestAccept(object sender, QuestAcceptRespose message)
        {
            Debug.LogFormat("QuestService->OnQuestAccept :{0}  ERR{1}",message.Result,message.Errormsg);

            if(message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败！", "错误", MessageBoxType.Error);
            }
        }

        private void OnQuestSubmit(object sender, QuestSubmitRespose message)
        {
            Debug.LogFormat("QuestService->OnQuestSubmit :{0}  ERR{1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestSubmited(message.Quest);
            }
            else
            {
                MessageBox.Show("任务提交失败！", "错误", MessageBoxType.Error);
            }
        }


    }
}
