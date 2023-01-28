using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;
using System;

namespace Assets.Scripts.Services
{
    class TeamService : Singleton<TeamService>, IDisposable
    {

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<teamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<teamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }
        public void SendTeamInviteRequest(int friendId,string friendName)
        {
            Debug.LogFormat("TeamService->SendTeamInviteRequest Id:{0} Name:{1}",friendId,friendName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteRequest = new teamInviteRequest();
            message.Request.teamInviteRequest.fromId = User.Instance.CurrentCharacter.Id;
            message.Request.teamInviteRequest.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.teamInviteRequest.toId = friendId;
            message.Request.teamInviteRequest.ToName = friendName;
            NetClient.Instance.SendMessage(message);

        }
        public void SendTeamInviteResponse(bool accept, teamInviteRequest request)
        {
            Debug.LogFormat("TeamService->SendTeamInviteResponse FromId:{0} FromName:{1}", request.fromId, request.FromName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteResponse = new TeamInviteResponse();
            message.Request.teamInviteResponse.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteResponse.Errormsg = accept ?  "组队成功":"对方拒绝了组队请求";
            message.Request.teamInviteResponse .Request=request;
            NetClient.Instance.SendMessage(message);
        }
        public void SendTeamInfo()
        {
            Debug.LogFormat("TeamService->SendTeamInfo");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInfo = new TeamInfoRequest();
            NetClient.Instance.SendMessage(message);
        }
        public void SendTeamLeave()
        {
            Debug.LogFormat("TeamService->SendTeamLeave");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.characterId = User.Instance.CurrentCharacter.Id;
            message.Request.teamLeave.characterId = User.Instance.TeamInfo.Id;
            NetClient.Instance.SendMessage(message);
        }
        private void OnTeamInviteRequest(object sender, teamInviteRequest request)
        {
            Debug.LogFormat("TeamService->OnTeamInviteRequest");
            var box = MessageBox.Show(string.Format("{0} 邀请你加入队伍", request.FromName), "组队请求", MessageBoxType.Confirm, "接受", "拒绝");
            box.OnYes = () =>
              {
                  this.SendTeamInviteResponse(true, request);
              };
            box.OnNo = () =>
               {
                   this.SendTeamInviteResponse(false, request);
               };
        }

        private void OnTeamInviteResponse(object sender, TeamInviteResponse response)
        {
            Debug.LogFormat("TeamService->OnTeamInviteResponse");
            MessageBox.Show(response.Errormsg);
            if(response.Result==Result.Success)
            {
                this.SendTeamInfo();
            }
        }

        private void OnTeamInfo(object sender, TeamInfoResponse response)
        {
            Debug.LogFormat("TeamService->OnTeamInfo");

            TeamManager.Instance.UpdateTeamInfo(response.Team);
        }

        private void OnTeamLeave(object sender, TeamLeaveResponse response)
        {
            Debug.LogFormat("TeamService->OnTeamLeave");
            if (response.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("退出成功", "退出队伍");
            }
            else
                MessageBox.Show("退出失败", "退出队伍", MessageBoxType.Error);
        }



    }
}
