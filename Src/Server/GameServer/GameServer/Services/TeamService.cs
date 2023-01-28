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
    class TeamService : Singleton<TeamService>
    {
        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<teamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponset);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInfoRequest>(this.OnTeamInfo);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);
        }
        public void Init()
        {
            TeamManager.Instance.Init();
        }

        private void OnTeamInviteRequest(NetConnection<NetSession> sender, teamInviteRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("TeamService->OnTeamInviteRequest FromId:{0} FromName:{1} ToId:{2} ToName:{3}",
                message.fromId, message.FromName, message.toId, message.ToName);

            var target = SessionManager.Instance.TryGetSession(message.toId);
            if(target == null)
            {
                sender.Session.Response.teamInviteResponse = new TeamInviteResponse();
                sender.Session.Response.teamInviteResponse.Result = Result.Failed;
                sender.Session.Response.teamInviteResponse.Errormsg = "好友不在线";
                sender.SendResponse();
                return;
            }
            if(target.Session.Character.team!=null)
            {
                sender.Session.Response.teamInviteResponse = new TeamInviteResponse();
                sender.Session.Response.teamInviteResponse.Result = Result.Failed;
                sender.Session.Response.teamInviteResponse.Errormsg = "对方已经有队伍";
                sender.SendResponse();
                return;
            }
            target.Session.Response.teamInviteRequest = message;
            target.SendResponse();
        }

        private void OnTeamInviteResponset(NetConnection<NetSession> sender, TeamInviteResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("TeamService->OnTeamInviteRequest ToCharacter:{0} Result:{1} FromId:{2} ToId:{3}",
                character,response.Result,response.Request.fromId, response.Request.toId);
            
            var requester = SessionManager.Instance.TryGetSession(response.Request.fromId);
            if (response.Result==Result.Success)
            {
                
                if(requester==null)
                {
                    sender.Session.Response.teamInviteResponse = new TeamInviteResponse();
                    sender.Session.Response.teamInviteResponse.Result = Result.Failed;
                    sender.Session.Response.teamInviteResponse.Errormsg = "请求者已下线";
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);

                    sender.Session.Response.teamInviteResponse = new TeamInviteResponse();
                    sender.Session.Response.teamInviteResponse.Result = Result.Success;
                    sender.Session.Response.teamInviteResponse.Errormsg = "加入队伍成功";
                    
                }
                sender.SendResponse();
            }
            if(requester!=null)
            {
                requester.Session.Response.teamInviteResponse = response;
                requester.SendResponse();
            }
        }

        private void OnTeamInfo(NetConnection<NetSession> sender, TeamInfoRequest request)
        {
            Log.InfoFormat("TeamService->OnTeamInfo Character:{0}",sender.Session.Character);
            sender.Session.Response.teamInfo = new TeamInfoResponse();
            sender.Session.Response.teamInfo.Result = Result.Success;
            sender.Session.Response.teamInfo.Team = new NteamInfo();
            Team team = sender.Session.Character.team;
            sender.Session.Response.teamInfo.Team.Id = team.id;
            sender.Session.Response.teamInfo.Team.leaderId = team.Leader.Id;
            foreach (var m in team.members)
            {
                sender.Session.Response.teamInfo.Team.Members.Add(m.GetBasicInfo());
            }
            sender.SendResponse();
        }

        private void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest request)
        {
            Log.InfoFormat("TeamService->OnTeamLeave Character:{0}", sender.Session.Character);
            var character = sender.Session.Character;
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.Session.Response.teamLeave.Errormsg = "离开队伍成功！";
            character.team.Leave(character);
            sender.SendResponse();
        }
    }
}
