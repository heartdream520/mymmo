using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Team
    {
        public int id;
        public Character Leader;
        public List<Character> members = new List<Character>();
        public int timeTS;
        public Team(Character leader)
        {
            this.AddMember(leader);
        }

        public void AddMember(Character member)
        {
            
            if(this.members.Count==0)
            {
                this.Leader = member;
            }
            Log.InfoFormat("Team->AddMember Leader:{0} Member:{1}", this.Leader, member);
            this.members.Add(member);
            member.team = this;
            this.timeTS = Time.timestamp;
            this.SendMessage();
        }
        public void Leave(Character member)
        {
            Log.InfoFormat("Team->Leave Leader:{0} Member:{1}", this.Leader, member);

            this.members.Remove(member);
            if(this.Leader==member)
            {
                if (this.members.Count > 0)
                    this.Leader = this.members[0];
                else this.Leader = null;
            }
            member.team = null;
            this.timeTS = Time.timestamp;
            this.SendMessage();

        }
        public void SendMessage()
        {
            foreach (var c in this.members)
            {
                var con = SessionManager.Instance.TryGetSession(c.Id);
                if (con != null)
                    con.SendResponse();
            }
        }
        public void PostProcess(NetMessageResponse message)
        {
           // if(message.teamInfo==null)
            {
                message.teamInfo = new TeamInfoResponse();
                message.teamInfo.Result = Result.Success;
                message.teamInfo.Team = new NteamInfo();
                message.teamInfo.Team.Id = this.id;
                message.teamInfo.Team.leaderId = this.Leader.Id;
                foreach (var m in this.members)
                    message.teamInfo.Team.Members.Add(m.GetBasicInfo());

            }
        }
    }
}
