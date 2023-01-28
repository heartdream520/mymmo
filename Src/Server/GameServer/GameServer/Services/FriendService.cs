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
    class FriendService :Singleton<FriendService>
    {
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponset);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendListRequest>(this.OnFriendList);
        }



        public void Init()
        {

        }
        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {

            Character character = sender.Session.Character;
            Log.InfoFormat("FriendService->OnFriendAddRequest FromId:{0} FromName:{1} ToId:{2} ToName:{3}",
                request.fromId, request.FromName, request.toId, request.ToName);
            
            
            
            if (character.FriendManager.TryGetFriendInfoByFriendId(request.toId)!=null)
            {
                sender.Session.Response.friendAddResponset = new FriendAddResponse();
                sender.Session.Response.friendAddResponset.Result = Result.Failed;
                sender.Session.Response.friendAddResponset.Errormsg = "已经是好友了";
                sender.SendResponse();
                return;
            }
            NetConnection<NetSession> friend_con = null;
            friend_con = SessionManager.Instance.TryGetSession(request.toId);
            if (friend_con==null)
            {
                sender.Session.Response.friendAddResponset = new FriendAddResponse();
                sender.Session.Response.friendAddResponset.Result = Result.Failed;
                sender.Session.Response.friendAddResponset.Errormsg = "该角色不存在或不在线"; ;
                sender.SendResponse();
                return;
            }
            
            friend_con.Session.Response.friendAddRequest = request;
            friend_con.SendResponse();
        }



        private void OnFriendAddResponset(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("FriendService->OnFriendAddResponset Result:{0} Errormsg:{1}",
                response.Result,response.Errormsg);

            
            var request_con = SessionManager.Instance.TryGetSession(response.Request.fromId);
            request_con.Session.Response.friendAddResponset = response;


            if (response.Result == Result.Success)
            {
                if (sender.Session.Response.friendAddResponset == null)
                    sender.Session.Response.friendAddResponset = new FriendAddResponse();

                if (request_con == null)
                {

                    sender.Session.Response.friendAddResponset.Result = Result.Failed;
                    sender.Session.Response.friendAddResponset.Errormsg = "请求者已下线";
                    sender.SendResponse();
                }
                else
                {

                    sender.Session.Response.friendAddResponset.Result = Result.Success;
                    sender.Session.Response.friendAddResponset.Errormsg = "添加成功";
                    
                    //互相添加好友
                    character.FriendManager.AddFriend(request_con.Session.Character);
                    request_con.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    sender.SendResponse();
                    
                }

            }
            request_con.SendResponse();


        }

        private void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("FriendService->OnFriendRemove CharacterId:{0} RemoveCId:{1}",
               character.Id, request.friendId);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.friendId = request.friendId;

            if (character.FriendManager.TryGetFriendInfoByFriendId(request.friendId) != null)
            {
                if (character.FriendManager.RemoveFriendByFriendId(request.friendId))
                {
                    sender.Session.Response.friendRemove.Result = Result.Success;
                    sender.Session.Response.friendRemove.Errormsg = "删除成功";
                    var friend_con = SessionManager.Instance.TryGetSession(request.friendId);
                    //在线
                    if (friend_con != null)
                    {
                        friend_con.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                        friend_con.SendResponse();
                    }
                        
                    //不在线
                    else
                        this.RemoveFriend(request.friendId, character.Id);

                }
            }
            else
            {
                sender.Session.Response.friendRemove.Result = Result.Failed;
                sender.Session.Response.friendRemove.Errormsg = "要删除的好友不存在";
            }
            sender.SendResponse();
            DBService.Instance.Save();
            

        }

        private void RemoveFriend(int id, int friend_Id)
        {
            var remove = DBService.Instance.Entities.TCharacterFriends.FirstOrDefault(v => v.TCharacterID == id && v.FriendID == friend_Id);
            if(remove!=null)
            {
                //remove.TCharacterID = -1;
                IList<TCharacterFriend> RemoveList = new List<TCharacterFriend>(){
                    remove
                };

                DBService.Instance.Entities.TCharacterFriends.RemoveRange(RemoveList);
            
            }

        }
        public void OnFriendList(NetConnection<NetSession> sender, FriendListRequest message)
        {
            Log.InfoFormat("FriendService->OnFriendList");
            var cha= sender.Session.Character;
            cha.FriendManager.InitFriends();
            sender.Session.Response.friendList = new FriendListResponse();
            sender.Session.Response.friendList.Friends.AddRange(cha.FriendManager.friends);
            sender.SendResponse();
        }
    }
}
