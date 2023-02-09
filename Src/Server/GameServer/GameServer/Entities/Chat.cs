using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Managers;
using SkillBridge.Message;

namespace GameServer.Entities
{
    class Chat
    {
        private Character owner;

        public Chat(Character character)
        {
            this.owner = character;
        }
        public int localIdx;
        public int worldIdx;
        public int systemIdx;
        public int teamIdx;
        public int gulidIdx;

        internal void PostProcess(NetMessageResponse message)
        {
            if(message.Chat==null)
            {
                message.Chat = new ChatResponse();
                message.Chat.Result = Result.Success;
            }
            this.localIdx = ChatManager.Instance.GetLocalMessage(this.owner.Info.mapId, localIdx, message.Chat.localMessages);
            this.worldIdx = ChatManager.Instance.GetWorldMessage( worldIdx, message.Chat.worldMessages);
            this.systemIdx = ChatManager.Instance.GetSystemMessage(systemIdx, message.Chat.systemMessages);
            if(this.owner.team!=null)
            this.teamIdx = ChatManager.Instance.GetTeamMessage(this.owner.team.id, teamIdx, message.Chat.teamMessages);
            if(this.owner.Gulid!=null)
            this.gulidIdx = ChatManager.Instance.GetGulidMessage(this.owner.Gulid.GulidId, gulidIdx, message.Chat.gulidMessages);
            this.printf(message.Chat.localMessages);
            this.printf(message.Chat.gulidMessages);
            this.printf(message.Chat.privateMessages);
            this.printf(message.Chat.systemMessages);
            this.printf(message.Chat.teamMessages);
            this.printf(message.Chat.worldMessages);
            
        }

        private void printf(List<ChatMessage> messages)
        {
            foreach(var m in messages)
            {
                Log.InfoFormat(m.Cannel+" "+m.Message);
            }
        }
    }
}
