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
    class TeamManager : Singleton<TeamManager>
    {
        public List<Team> Teams = new List<Team>();
        public Dictionary<int, Team> CharacterTeams = new Dictionary<int, Team>();

        internal void Init()
        {

        }

        internal void AddTeamMember(Character Leader, Character member)
        {
            if (Leader.team == null)
                Leader.team = this.CreateTeam(Leader);
            Leader.team.AddMember(member);
        }

        private Team CreateTeam(Character leader)
        {
            Team team = null;
            for(int i=0;i<this.Teams.Count;i++)
            {
                team = this.Teams[i];
                if(team.members.Count==0)
                {

                     ChatManager.Instance.Team.TryGetValue(team.id,out var x);
                    if (x != null)
                        x.Clear();
                    team.AddMember(leader);
                    
                    return team;
                }
            }
            team = new Team(leader);
            this.Teams.Add(team);
            team.id = this.Teams.Count;
            return team;
        }
    }
}
