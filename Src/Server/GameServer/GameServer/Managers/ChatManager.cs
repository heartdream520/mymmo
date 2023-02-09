using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Services;
using Common.Utils;

namespace GameServer.Managers
{
    class ChatManager : Singleton<ChatManager>
    {
        public List<ChatMessage> System = new List<ChatMessage>();  //系统
        public List<ChatMessage> World = new List<ChatMessage>();  //系统
        public Dictionary<int, List<ChatMessage>> Local = new Dictionary<int, List<ChatMessage>>();//本地
        public Dictionary<int, List<ChatMessage>> Team = new Dictionary<int, List<ChatMessage>>();//队伍
        public Dictionary<int, List<ChatMessage>> Gulid = new Dictionary<int, List<ChatMessage>>();//公会
        public void Init()
        {

        }
        public void AddMessage(Character from, ChatMessage message)
        {
            message.FromId = from.Id;
            message.FromName = from.Name;
            message.Time = TimeUtil.timestamp;
            switch (message.Cannel)
            {
                case ChatChannel.Local:
                    this.AddLoaclMessage(from.Info.mapId, message);
                    break;
                case ChatChannel.World:
                    this.AddWorldMessage(message);
                    break;
                case ChatChannel.System:
                    this.AddSystemMessage(message);
                    break;
                case ChatChannel.Team:
                    this.AddTeamMessage(from.team.id, message);
                    break;
                case ChatChannel.Gulid:
                    this.AddGulidMessage(from.Gulid.GulidId, message);
                    break;
                default:
                    break;
            }
        }

        private void AddLoaclMessage(int mapId, ChatMessage message)
        {
            List<ChatMessage> messages;
            if (!this.Local.TryGetValue(mapId, out messages))
            {
                messages = new List<ChatMessage>();
                this.Local[mapId] = messages;
            }
            messages.Add(message);
        }

        private void AddWorldMessage(ChatMessage message)
        {
            this.World.Add(message);
        }

        private void AddSystemMessage(ChatMessage message)
        {
            this.System.Add(message);
        }

        private void AddTeamMessage(int id, ChatMessage message)
        {
            List<ChatMessage> messages;
            if (!this.Team.TryGetValue(id, out messages))
            {
                messages = new List<ChatMessage>();
                this.Team[id] = messages;
            }
            messages.Add(message);
        }

        private void AddGulidMessage(int gulidId, ChatMessage message)
        {
            List<ChatMessage> messages;
            if (!this.Gulid.TryGetValue(gulidId, out messages))
            {
                messages = new List<ChatMessage>();
                this.Gulid[gulidId] = messages;
            }
            messages.Add(message);
        }


        public int GetLocalMessage(int mapId, int idx, List<ChatMessage> message)
        {
            List<ChatMessage> messages;
            if (!Local.TryGetValue(mapId, out messages))
            {
                return 0;
            }

            return this.GetNewMessages(idx, message, messages);
        }
        public int GetWorldMessage(int idx, List<ChatMessage> message)
        {
            return this.GetNewMessages(idx, message, this.World);
        }
        public int GetSystemMessage(int idx, List<ChatMessage> message)
        {
            return this.GetNewMessages(idx, message, this.System);
        }
        public int GetTeamMessage(int teamId, int idx, List<ChatMessage> message)
        {
            List<ChatMessage> messages;
            if (!Team.TryGetValue(teamId, out messages))
            {
                return 0;
            }
            return this.GetNewMessages(idx, message, messages);
        }
        public int GetGulidMessage(int gulidId, int idx, List<ChatMessage> message)
        {
            List<ChatMessage> messages;
            if (!Gulid.TryGetValue(gulidId, out messages))
            {
                return 0;
            }
            return this.GetNewMessages(idx, message, messages);
        }

        private int GetNewMessages(int idx, List<ChatMessage> result, List<ChatMessage> messages)
        {
            if (idx == 0)
            {
                if (messages.Count > GameDefine.MexChatRecoredNums)
                {
                    idx = messages.Count - GameDefine.MexChatRecoredNums;
                }
            }
            for (; idx < messages.Count; idx++)
                result.Add(messages[idx]);
            return idx;
        }
    }
}
