using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Services
{

    class GulidService : Singleton<GulidService>, IDisposable
    {
        public UnityAction<NGulidInfo> OnGulidInfoAction;
        public UnityAction<List<NGulidInfo>> OnGulidListAction;
        public UnityAction<Result> OnGulidCreatAction;
        public void Init()
        {


        }
        public GulidService()
        {
            MessageDistributer.Instance.Subscribe<GulidCreatResponse>(this.OnGulidCreat);
            MessageDistributer.Instance.Subscribe<GulidInfoResponse>(this.OnGulidInfo);
            MessageDistributer.Instance.Subscribe<GulidListResponse>(this.OnGulidList);
            MessageDistributer.Instance.Subscribe<GulidJoinRequest>(this.OnGulidJoinRequest);
            MessageDistributer.Instance.Subscribe<GulidJoinResponse>(this.OnGulidJoinResponse);
            MessageDistributer.Instance.Subscribe<GulidLeaveResponse>(this.OnGulidLeave);
            MessageDistributer.Instance.Subscribe<GulidAdminResponse>(this.OnGulidAdmin);
        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GulidCreatResponse>(this.OnGulidCreat);
            MessageDistributer.Instance.Unsubscribe<GulidInfoResponse>(this.OnGulidInfo);
            MessageDistributer.Instance.Unsubscribe<GulidListResponse>(this.OnGulidList);
            MessageDistributer.Instance.Unsubscribe<GulidJoinRequest>(this.OnGulidJoinRequest);
            MessageDistributer.Instance.Unsubscribe<GulidJoinResponse>(this.OnGulidJoinResponse);
            MessageDistributer.Instance.Unsubscribe<GulidLeaveResponse>(this.OnGulidLeave);
            MessageDistributer.Instance.Unsubscribe<GulidAdminResponse>(this.OnGulidAdmin);
        }




        internal void SendGulidList()
        {
            Debug.Log("GulidService->SendGulidList");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidList = new GulidListRequest();
            NetClient.Instance.SendMessage(message);
        }
        internal void SendGulidInfo()
        {
            Debug.Log("GulidService->SendGulidList");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidInfo = new GulidInfoRequest();

            NetClient.Instance.SendMessage(message);

        }

        internal void SendGulidCreat(string gulid_Name, string gulid_Notic)
        {
            Debug.LogFormat("GulidService->SendGulidCreat");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidCreat = new GulidCreatRequest();
            message.Request.gulidCreat.GulidName = gulid_Name;
            message.Request.gulidCreat.GulidNotice = gulid_Notic;


            NetClient.Instance.SendMessage(message);
        }
        internal void SendJoinGulidRequest(int CharacterId, int gulidId)
        {
            Debug.LogFormat("GulidService->SendJoinGulidRequest GulidId:{0}", gulidId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidJoinRequest = new GulidJoinRequest();
            message.Request.gulidJoinRequest.Apply = new NGulidApplyInfo();
            var cha = User.Instance.CurrentCharacter;
            message.Request.gulidJoinRequest.Apply.characterId = CharacterId;
            message.Request.gulidJoinRequest.Apply.Class = cha.ConfigId;
            message.Request.gulidJoinRequest.Apply.Level = cha.Level;
            message.Request.gulidJoinRequest.Apply.Name = cha.Name;
            message.Request.gulidJoinRequest.Apply.GulidId = gulidId;

            NetClient.Instance.SendMessage(message);

        }
        public void SendGulidJoinResponse(NGulidApplyInfo info, bool result)
        {
            Debug.LogFormat("GulidService->SendGulidJoinResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidJoinResponse = new GulidJoinResponse();
            message.Request.gulidJoinResponse.Apply = info;
            message.Request.gulidJoinResponse.Apply.ApplyResult = result ? ApplyResult.Accept : ApplyResult.Reject;
            message.Request.gulidJoinResponse.Result = result ? Result.Success : Result.Failed;
            message.Request.gulidJoinResponse.Errormsg = result ? "公会管理员同意您加入公会" : "公会管理员拒绝您加入公会";


            NetClient.Instance.SendMessage(message);
        }
        internal void SendGulidLeave()
        {
            Debug.LogFormat("GulidService->SendGulidLeave");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidLeave = new GulidLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGulidCreat(object sender, GulidCreatResponse response)
        {
            Debug.LogFormat("GulidService->OnGulidCreat Result:{0}", response.Result);
            MessageBox.Show(response.Errormsg, "公会");
            this.OnGulidCreatAction(response.Result);
            if (response.Result == Result.Success)
            {
                if (this.OnGulidInfoAction != null)
                    this.OnGulidInfoAction(response.gulidInfo);
            }
        }

        private void OnGulidInfo(object sender, GulidInfoResponse response)
        {
            Debug.LogFormat("GulidService->OnGulidInfo");
            if(!string.IsNullOrEmpty(response.Errormsg))
            {
                MessageBox.Show(response.Errormsg, "公会");
            }
            if (this.OnGulidInfoAction != null)
                this.OnGulidInfoAction(response.GulidInfo);
        }

        private void OnGulidList(object sender, GulidListResponse response)
        {
            Debug.LogFormat("GulidService->OnGulidList");
            if (this.OnGulidListAction != null)
                this.OnGulidListAction(response.GulidInfos);

        }

        private void OnGulidJoinRequest(object sender, GulidJoinRequest request)
        {
            Debug.LogFormat("GulidService->OnGulidJoinRequest");

        }

        private void OnGulidJoinResponse(object sender, GulidJoinResponse response)
        {
            Debug.LogFormat("GulidService->OnGulidJoinResponse Result:{0}", response.Result);
            MessageBox.Show(response.Errormsg, "公会");
            if (response.Result == Result.Success)
            {
                this.SendGulidInfo();
            }

        }

        private void OnGulidLeave(object sender, GulidLeaveResponse response)
        {
            Debug.LogFormat("GulidService->OnGulidLeave");
            MessageBox.Show(response.Errormsg, "公会");
            if (response.Result == Result.Success)
            {
                if (this.OnGulidInfoAction != null)
                    this.OnGulidInfoAction(null);
            }



        }
        public void SendGulidAdmin(GulidAdminCommand command,int target)
        {
            Debug.LogFormat("GulidService->SendGulidAdmin");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gulidAdmin = new GulidAdminRequest();
            message.Request.gulidAdmin.Command = command;
            message.Request.gulidAdmin.Target = target;
            NetClient.Instance.SendMessage(message);
        }
        private void OnGulidAdmin(object sender, GulidAdminResponse response)
        {
            var command = response.Command;
            MessageBox.Show(response.Errormsg, "公会");
            if(response.Result==Result.Success)
            {
                this.SendGulidInfo();
            }
        }
    }
}

