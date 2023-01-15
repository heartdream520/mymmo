using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services
{
    class QuestService : Singleton<QuestService>
    {
        public QuestService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestListRequest>(this.OnQuestList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestAcceptRequest>(this.OnQuestAccept);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestSubmitRequest>(this.OnQuestSubmit);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestAbandonRequest>(this.OnQuestAbandon);

        }
        public void Init()
        {

        }
        private void OnQuestList(NetConnection<NetSession> sender, QuestListRequest request)
        {
            throw new NotImplementedException();
        }

        private void OnQuestAccept(NetConnection<NetSession> sender, QuestAcceptRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("QuestService->OnQuestAccept Character:{0} QuestID:{1}", character, request.QuestId);

            sender.Session.Response.questAccept = new QuestAcceptRespose();

            Result result = character.QuestManager.AcceptQuest(sender, request.QuestId);
            sender.Session.Response.questAccept.Result = result;
            sender.SendResponse();

        }

        private void OnQuestSubmit(NetConnection<NetSession> sender, QuestSubmitRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("QuestService->OnQuestSubmit Character:{0} QuestID:{1}", character, request.QuestId);

            sender.Session.Response.questSubmit = new QuestSubmitRespose();

            Result result = character.QuestManager.SubmitQuest(sender, request.QuestId);
            sender.Session.Response.questSubmit.Result = result;
            sender.SendResponse();
        }

        private void OnQuestAbandon(NetConnection<NetSession> sender, QuestAbandonRequest request)
        {
            throw new NotImplementedException();
        }
    }
}