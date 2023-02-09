using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Services
{
    class ChatService : Singleton<ChatService>, IDisposable
    {
        public void Init() { }
        public bool first=true;
        public ChatService()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(this.OnChatResponse);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ChatResponse>(this.OnChatResponse);

        }
        internal void SendChat(ChatChannel channel, string content, int toId, string toName)
        {
            Debug.LogFormat("ChatService->SendChat Channel:{0} Message:{1}", channel, content);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.chatMessage = new ChatMessage();
            message.Request.Chat.chatMessage.Cannel = channel;
            message.Request.Chat.chatMessage.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.Chat.chatMessage.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.Chat.chatMessage.ToId = toId;
            message.Request.Chat.chatMessage.ToName = toName;
            message.Request.Chat.chatMessage.Message = content;
            NetClient.Instance.SendMessage(message);

            
        }
        private void OnChatResponse(object sender, ChatResponse response)
        {
            if(!string.IsNullOrEmpty(response.Errormsg))
            {
                MessageBox.Show(response.Errormsg, "聊天");
            }
            if(response.Result==Result.Success)
            {
                ChatManager.Instance.AddMessage(ChatChannel.Local,response.localMessages);
                ChatManager.Instance.AddMessage(ChatChannel.Gulid,response.gulidMessages);
                ChatManager.Instance.AddMessage(ChatChannel.Private,response.privateMessages);
                ChatManager.Instance.AddMessage(ChatChannel.System,response.systemMessages);
                ChatManager.Instance.AddMessage(ChatChannel.Team,response.teamMessages);
                ChatManager.Instance.AddMessage(ChatChannel.World,response.worldMessages);

            }
            first = false;

        }


    }
}
