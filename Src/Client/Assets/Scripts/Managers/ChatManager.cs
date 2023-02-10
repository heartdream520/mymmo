using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
using Assets.Scripts.UI.Set;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


namespace Assets.Scripts.Managers
{
    class ChatManager : Singleton<ChatManager>
    {


        internal void Init()
        {
            foreach(var x in this.Messages)
            {
                x.Clear();
            }
        }
        public UnityAction OnChatAction;

        public int privateId = 0;
        public string privateName = "";


        public bool[] hasNewMessage = new bool[6]
        {
            false,false,false,
            false,false,false,
        };

        public enum LocalChannel
        {
            ALL = 0,
            Loacl = 1,
            World = 2,
            Team = 3,
            Gulid = 4,
            Private = 5,
        }

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Gulid|ChatChannel.Local|ChatChannel.Private|ChatChannel.System|ChatChannel.Team|ChatChannel.World,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Gulid,
            ChatChannel.Private
        };
        public void StartPrivateChat(int targetId,string targetName)
        {
            this.privateId = targetId;
            this.privateName = targetName;
            this.sendChannel = LocalChannel.Private;
            this.disPlayChannel= LocalChannel.Private;
            if (this.OnChatAction != null)
                this.OnChatAction();
        }
        public LocalChannel sendChannel;
        public ChatChannel SendNChannel
        {
            get
            {
                switch (sendChannel)
                {
                    case LocalChannel.ALL:
                        return ChatChannel.All;
                    case LocalChannel.Loacl:
                        return ChatChannel.Local;
                    case LocalChannel.World:
                        return ChatChannel.World;
                    case LocalChannel.Team:
                        return ChatChannel.Team;
                    case LocalChannel.Gulid:
                        return ChatChannel.Gulid;
                    case LocalChannel.Private:
                        return ChatChannel.Private;

                }
                return ChatChannel.Local;
            }
        }



        public List<ChatMessage>[] Messages = new List<ChatMessage>[6]
        {
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
        };
        internal LocalChannel disPlayChannel;





        internal bool SetSendChannel(int idx)
        {
            var channel = (LocalChannel)idx;

            //Debug.LogError(channel);

            if (channel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    SoundManager.Instance.PlayerSound(SoundDefine.UI_Message_Error);
                    return false;
                }
            }
            if (channel == LocalChannel.Gulid)
            {
                if (GulidManager.Instance.Gulid_Info==null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    SoundManager.Instance.PlayerSound(SoundDefine.UI_Message_Error);
                    return false;
                }
            }
            this.sendChannel = channel;
            this.disPlayChannel = channel;
            return true;
        }

        internal void SendChat(string text,int toId=0,string toName="")
        {
            if(this.sendChannel==LocalChannel.Private&&this.privateId==0)
            {
                MessageBox.Show("请选择私聊对象","聊天");
                return;
            }
            else if(this.sendChannel == LocalChannel.Private)
            {
                ChatService.Instance.SendChat(this.SendNChannel, text, this.privateId, this.privateName);
                return;
            }
            ChatService.Instance.SendChat(this.SendNChannel, text, toId, toName);
            this.disPlayChannel = this.sendChannel;
        }
        private void AddSystemMessage(string message)
        {
            this.Messages[(int)LocalChannel.ALL].Add(new ChatMessage()
            {
                Cannel = ChatChannel.System,
                Message = message,
                FromName = User.Instance.CurrentCharacter.Name,
                FromId = User.Instance.CurrentCharacter.Id,
            });
            if(this.disPlayChannel!= (int)LocalChannel.ALL)
            this.Messages[(int)this.disPlayChannel].Add(new ChatMessage()
            {
                Cannel = ChatChannel.System,
                Message = message,
                FromName = User.Instance.CurrentCharacter.Name,
                FromId = User.Instance.CurrentCharacter.Id,
            });
            if (this.OnChatAction != null)
                this.OnChatAction();
        }
        internal void AddMessage(ChatChannel channel, List<ChatMessage> messages)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((this.ChannelFilter[i] & channel) == channel)
                {
                    this.Messages[i].AddRange(messages);
                    if (messages.Count != 0 && i != 0 && !ChatService.Instance.first)
                        this.hasNewMessage[i] = true;

                }
            }
            if (this.OnChatAction != null)
                this.OnChatAction();
        }
        internal string GetCurrentMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var mess in this.Messages[(int)this.disPlayChannel])
            {
                sb.AppendLine(this.FormatMessage(mess));
            }
            if (this.disPlayChannel != LocalChannel.ALL)
            {
                var removeList = new List<ChatMessage>();
                foreach (var m in this.Messages[(int)this.disPlayChannel])
                    if (m.Cannel == ChatChannel.System)
                        removeList.Add(m);
                foreach (var m in removeList)
                    this.Messages[(int)this.disPlayChannel].Remove(m);
            }
            return sb.ToString();
        }

        private string FormatMessage(ChatMessage message)
        {
            switch (message.Cannel)
            {
                case ChatChannel.Local:
                    return string.Format("<color=red>[本地]{0} {1}</color>", this.FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color=cyan>[世界]{0} {1}</color>", this.FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return string.Format("<color=yellow>[系统]{0}</color>", message.Message);
                case ChatChannel.Private:
                    return string.Format("<color=yellowgreen>[私聊]{0} {1}</color>", this.FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return string.Format("<color=purple>[队伍]{0} {1}</color>", this.FormatFromPlayer(message), message.Message);
                case ChatChannel.Gulid:
                    return string.Format("<color=blue>[公会]{0} {1}</color>", this.FormatFromPlayer(message), message.Message);
                default:
                    break;
            }
            return "";
        }

        private object FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                return "<a name=\"\" class=\"player\">[我]</a>";
            }
            else return string.Format("<a name=\"c:{0}:{1}\" class=\"player\">[{1}]</a>",message.FromId,message.FromName);
        }


    }
}
