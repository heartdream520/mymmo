using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Managers
{
    class UserManager:Singleton<UserManager>
    {
        public Dictionary<string, TUser> Users = new Dictionary<string, TUser>();



        internal void AddUser(string id, TUser user)
        {
            this.Users[id] = user;
        }

        internal void RemoveUser(string id)
        {
            if (this.Users.ContainsKey(id))
                this.Users.Remove(id);
        }

        internal TUser TryGetUser(string id)
        {
            if (Users.ContainsKey(id))
                return Users[id];
            return null;
        }
    }
}
