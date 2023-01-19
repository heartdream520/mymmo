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
    class FriendService:Singleton<FriendService>,IDisposable
    {
        public UnityAction OnFriendUpdate;

        
        public void Init()
        {

        }
        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }

        public void SendFriendAddRequest(int friendId,string friendName)
        {
            Debug.LogFormat("FriendService->SendFriendAddRequest FriendId:{0} FriendName:{1}", friendId, friendName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRequest = new FriendAddRequest();
            message.Request.friendAddRequest.fromId = User.Instance.CurrentCharacter.Id;
            message.Request.friendAddRequest.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.friendAddRequest.toId = friendId;
            message.Request.friendAddRequest.ToName = friendName;
            NetClient.Instance.SendMessage(message);





        }
        public void SendFriendAddResponse(bool accept,FriendAddRequest request)
        {
            Debug.LogFormat("FriendService->SendFriendAddResponse Accept:{0}", accept);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddResponset = new FriendAddResponse();
            message.Request.friendAddResponset.Request = request;
            message.Request.friendAddResponset.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddResponset.Errormsg = accept ? "对方同意了您的请求" : "对方拒绝了您的请求";
            NetClient.Instance.SendMessage(message);

        }
        public void SendFriendListRequest()
        {
            Debug.LogFormat("FriendService->SendFriendListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendList = new FriendListRequest();
            NetClient.Instance.SendMessage(message);
        }
        public void SendFriendRemoveRequest(int Id,int friendId)
        {
            Debug.LogFormat("FriendService->SendFriendRemoveRequest Id:{0} FriendId:{1}" ,Id,friendId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.Id = Id;
            message.Request.friendRemove.friendId = friendId;
            NetClient.Instance.SendMessage(message);
        }
        private void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            Debug.LogFormat("FriendService->OnFriendAddRequest FromId:{0} FromName:{1}", request.fromId, request.FromName);
            var box = MessageBox.Show(string.Format("{0} 请求添加好友", request.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            box.OnYes = () =>
              {
                  this.SendFriendAddResponse(true, request);
              };
            box.OnNo = () =>
              {
                  this.SendFriendAddResponse(false, request);
              };
        }

        private void OnFriendAddResponse(object sender, FriendAddResponse response)
        {
            Debug.Log("FriendService->OnFriendAddResponse");
            /*
            if(response.Result==Result.Success)
            {
                this.SendFriendListRequest();
            }
            */
            MessageBox.Show(response.Errormsg);
        }



        private void OnFriendList(object sender, FriendListResponse response)
        {
            Debug.LogFormat("FriendService->OnFriendList");
            FriendManager.Instance.allFriends = response.Friends;
            if (this.OnFriendUpdate != null)
                this.OnFriendUpdate();
        }

        public void OnFriendRemove(object sender, FriendRemoveResponse response)
        {
            Debug.LogFormat("FriendService->OnFriendRemove RemoveCharId:{0} Resule:{1} Errmsg:{2}",response.friendId,response.Result,response.Errormsg);
            /*
            if(response.Result==Result.Success)
                this.SendFriendListRequest();
            */
            MessageBox.Show(response.Errormsg);

        }


    }
}
