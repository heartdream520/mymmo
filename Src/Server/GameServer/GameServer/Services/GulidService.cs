using Common;
using Common.Data;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class GulidService :Singleton<GulidService>
    {
        public void Init() { GulidManager.Instance.Init(); }
        public GulidService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidCreatRequest>(this.OnGulidCreat);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidInfoRequest>(this.OnGulidInfo);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidJoinRequest>(this.OnGulidJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidJoinResponse>(this.OnGulidJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidLeaveRequest>(this.OnGulidLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidListRequest>(this.OnGulidList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GulidAdminRequest>(this.OnGulidAdmin);
        }



        private void OnGulidCreat(NetConnection<NetSession> sender, GulidCreatRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("GulidService->OnGulidCreat Character:{0},GulidName:{1},Notice:{2}",cha,request.GulidName,request.GulidNotice);
            sender.Session.Response.gulidCreat = new GulidCreatResponse();
            if(cha.Gulid!=null)
            {
                sender.Session.Response.gulidCreat.Result = Result.Failed;
                sender.Session.Response.gulidCreat.Errormsg = "已经有公会";
                sender.SendResponse();
                return;
            }
            if(GulidManager.Instance.ExitGulidName(request.GulidName))
            {
                sender.Session.Response.gulidCreat.Result = Result.Failed;
                sender.Session.Response.gulidCreat.Errormsg = "公会名称已存在";
                sender.SendResponse();
                return;
            }
            GulidManager.Instance.CreateGulid(request.GulidName, request.GulidNotice, cha);
            sender.Session.Response.gulidCreat.Result = Result.Success;
            sender.Session.Response.gulidCreat.Errormsg = "公会创建成功";
            sender.Session.Response.gulidCreat.gulidInfo = cha.Gulid.GetGulidInfo(cha);
            sender.SendResponse();
        }

        private void OnGulidInfo(NetConnection<NetSession> sender, GulidInfoRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("GulidService->OnGulidInfo Character:{0}",cha);
            sender.Session.Response.gulidInfo = new GulidInfoResponse();
            if (cha.Gulid == null)
            {
                sender.Session.Response.gulidInfo.Result = Result.Failed;
                sender.Session.Response.gulidInfo.GulidInfo = null;
            }
            else
            {
                sender.Session.Response.gulidInfo.Result = Result.Success;
                sender.Session.Response.gulidInfo.GulidInfo = cha.Gulid.GetGulidInfo(cha);
            }
            
            sender.SendResponse();

        }

        private void OnGulidList(NetConnection<NetSession> sender, GulidListRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("GulidService->OnGulidList");
            sender.Session.Response.gulidList = new GulidListResponse();
            sender.Session.Response.gulidList.Result = Result.Success;
            sender.Session.Response.gulidList.GulidInfos .AddRange( GulidManager.Instance.GetGulidList());
            sender.SendResponse();
        }
        private void OnGulidJoinRequest(NetConnection<NetSession> sender, GulidJoinRequest request)
        {
            Character cha = sender.Session.Character;
            var gulid = GulidManager.Instance.GetGulidById(request.Apply.GulidId);
            Log.InfoFormat("GulidService->OnGulidJoinRequest Character:{0} ,GulidName:{1}",cha,gulid.name);
            if(cha.Gulid!=null)
            {
                sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                sender.Session.Response.gulidJoinResponse.Result = Result.Failed;
                sender.Session.Response.gulidJoinResponse.Errormsg = "您已经加入公会";
                sender.SendResponse();
                return;
            }
            if(gulid==null)
            {
                sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                sender.Session.Response.gulidJoinResponse.Result = Result.Failed;
                sender.Session.Response.gulidJoinResponse.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
          
            if (gulid.JoinApply(request.Apply))
            {
                sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                sender.Session.Response.gulidJoinResponse.Result = Result.Success;
                sender.Session.Response.gulidJoinResponse.Errormsg = "公会申请发送成功";
                sender.SendResponse();
                return;
            }
            else
            {
                sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                sender.Session.Response.gulidJoinResponse.Result = Result.Failed;
                sender.Session.Response.gulidJoinResponse.Errormsg = "请勿重复申请";
                sender.SendResponse();
                return;
            }
        }

        private void OnGulidJoinResponse(NetConnection<NetSession> sender, GulidJoinResponse response)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("GulidService->OnGulidJoinResponse");
            var gulid = GulidManager.Instance.GetGulidById(response.Apply.GulidId);

            if(response.Result==Result.Success)
            {
                var dbSendCha = DBService.Instance.Entities.Characters.FirstOrDefault(v => v.ID == response.Apply.characterId);
                if(dbSendCha.GulidId!=0)
                {
                    sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                    sender.Session.Response.gulidJoinResponse.Result = Result.Failed;
                    sender.Session.Response.gulidJoinResponse.Errormsg = "对方已加入其他公会";
                    sender.SendResponse();
                }
                if (!gulid.JoinAccept(response.Apply))
                {
                    return;
                }
                sender.Session.Response.gulidJoinResponse = new GulidJoinResponse();
                sender.Session.Response.gulidJoinResponse.Result = Result.Success;
                sender.Session.Response.gulidJoinResponse.Errormsg = "对方已成功加入公会";
                sender.SendResponse();
            }
            var requester = SessionManager.Instance.TryGetSession(response.Apply.characterId);
            if(requester!=null)
            {
                requester.Session.Character.Gulid = gulid;
                requester.Session.Response.gulidJoinResponse = response;
                requester.SendResponse();
            }

        }

        private void OnGulidLeave(NetConnection<NetSession> sender, GulidLeaveRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("GulidService->OnGulidLeave");
            var gulid = cha.Gulid;
            if(gulid==null)
            {
                sender.Session.Response.gulidLeave = new GulidLeaveResponse();
                sender.Session.Response.gulidLeave.Result = Result.Failed;
                sender.Session.Response.gulidLeave.Errormsg = "您还没有公会";
                sender.SendResponse();
                return;
            }

            sender.Session.Response.gulidLeave = new GulidLeaveResponse();
            string msg;
            sender.Session.Response.gulidLeave.Result = gulid.MumberLeave(cha, out msg) ? Result.Success : Result.Failed;

            sender.Session.Response.gulidLeave.Errormsg = msg;
            sender.SendResponse();
        }

        private void OnGulidAdmin(NetConnection<NetSession> sender, GulidAdminRequest request)
        {
            var cha = sender.Session.Character;
            var gulid = cha.Gulid;
            string msg;
            var result=gulid.OnGulidAdmin(cha, request.Command, request.Target, out msg) ? Result.Success : Result.Failed;

            sender.Session.Response.gulidAdmin = new GulidAdminResponse();
            sender.Session.Response.gulidAdmin.Result = result;
            sender.Session.Response.gulidAdmin.Errormsg = msg;
            sender.Session.Response.gulidAdmin.Command = request.Command;
            
            sender.SendResponse();
            if (result==Result.Success)
            {
                gulid.timestamp = TimeUtil.timestamp;
                gulid.SendMessageToLoadMember();
            }
        }

    }
}
