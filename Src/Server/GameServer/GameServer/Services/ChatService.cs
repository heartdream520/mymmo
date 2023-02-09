using Common;
using Common.Data;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ChatService : Singleton<ChatService>
    {
        public void Init() { CharacterManager.Instance.Init(); }
        public ChatService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ChatRequest>(this.OnChat);
        }

        private void OnChat(NetConnection<NetSession> sender, ChatRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("ChatService->OnChat Character:{0} Channel:{1} message:{2}", cha, request.chatMessage.Cannel, request.chatMessage.Message);
            if (request.chatMessage.Cannel == ChatChannel.Private)
            {
                var SendToCon = SessionManager.Instance.TryGetSession(request.chatMessage.ToId);
                if (SendToCon == null)
                {
                    if (sender.Session.Response.Chat == null)
                        sender.Session.Response.Chat = new ChatResponse();
                    sender.Session.Response.Chat = new ChatResponse();
                    sender.Session.Response.Chat.Result = Result.Failed;
                    sender.Session.Response.Chat.Errormsg = "对方不在线";
                    sender.Session.Response.Chat.privateMessages.Add(request.chatMessage);
                    sender.SendResponse();
                }
                else
                {
                    if (SendToCon.Session.Response.Chat == null)
                    {
                        SendToCon.Session.Response.Chat = new ChatResponse();
                    }
                    request.chatMessage.FromId = cha.Id;
                    request.chatMessage.FromName = cha.Name;


                    SendToCon.Session.Response.Chat.Result = Result.Success;
                    SendToCon.Session.Response.Chat.privateMessages.Add(request.chatMessage);
                    SendToCon.SendResponse();
                    if (sender.Session.Response.Chat == null)
                    {
                        sender.Session.Response.Chat = new ChatResponse();
                    }
                    sender.Session.Response.Chat.Result = Result.Success;
                    sender.Session.Response.Chat.privateMessages.Add(request.chatMessage);
                    sender.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.Chat = new ChatResponse();
                sender.Session.Response.Chat.Result = Result.Success;
                ChatManager.Instance.AddMessage(cha, request.chatMessage);
                sender.SendResponse();
            }
        }
    }
}