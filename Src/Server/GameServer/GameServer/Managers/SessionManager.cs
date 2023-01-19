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
namespace GameServer.Managers
{
    class SessionManager:Singleton<SessionManager>
    {
        public Dictionary<int, NetConnection<NetSession>> Sessions = new Dictionary<int, NetConnection<NetSession>>();



        internal void AddSession(int id, NetConnection<NetSession> sender)
        {
            this.Sessions[id] = sender;
        }

        internal void RemoveSession(int id)
        {
            if (this.Sessions.ContainsKey(id))
                this.Sessions.Remove(id);
        }

        internal NetConnection<NetSession> TryGetSession(int id)
        {
            if (Sessions.ContainsKey(id))
                return Sessions[id];
            return null;
        }
    }
}
