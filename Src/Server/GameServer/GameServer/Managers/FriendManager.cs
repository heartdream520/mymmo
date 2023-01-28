using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FriendManager
    {
        private Character owner;
        public List<NFriendInfo> friends = new List<NFriendInfo>();
        /// <summary>
        /// 好友是否改变
        /// </summary>
        public bool friendChange;

        public FriendManager(Character character)
        {
            this.owner = character;
            this.InitFriends();
        }
        public void InitFriends()
        {
            this.friends.Clear();
            foreach(var f in this.owner.Data.Friends)
            {
                this.friends.Add(this.GetFriendInfo(f));
            }
        }
        internal void GetQuestInfos(List<NFriendInfo> friends)
        {
            foreach (var f in this.friends)
                friends.Add(f);
        }

        private NFriendInfo GetFriendInfo(TCharacterFriend info)
        {
            NFriendInfo friendInfo = new NFriendInfo();
            var character = CharacterManager.Instance.GetBaseCharacterInfo(info.FriendID);
            friendInfo.friendInfo = character;

            friendInfo.Id = info.Id;
            
            //在线状态为1，否则为0
            var friend_con = SessionManager.Instance.TryGetSession(info.FriendID);
            if(friend_con!=null)
            {
                friend_con.Session.Character.FriendManager.UpdateFriendInfo(this.owner.Info, 1);
            }
            friendInfo.Status =  friend_con== null ? 0 : 1;
            
            
            return friendInfo;

        }

        /// <summary>
        /// 更新某个好友上线信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="v"></param>
        private void UpdateFriendInfo(NCharacterInfo info, int status)
        {
            foreach(var f in this.friends)
            {
                if(f.friendInfo.Id==info.Id)
                {
                    f.Status = status;
                    break;
                }
            }
            this.friendChange = true;
        }
        /// <summary>
        /// 更新自己在好友中的上线状态
        /// </summary>
        /// <param name="info"></param>
        /// <param name="status"></param>
        internal void UpdateFriendSelfInfo(NCharacterInfo info, int status)
        {
            foreach(var f in this.friends)
            {
                var f_con= SessionManager.Instance.TryGetSession(f.friendInfo.Id);
                if(f_con!=null)
                {
                    f_con.Session.Character.FriendManager.UpdateFriendInfo(info, status);
                    FriendService.Instance.OnFriendList(f_con, new FriendListRequest());
                }

            }
        }
        internal NFriendInfo TryGetFriendInfoByFriendId(int Id)
        {
            foreach(var f in this.friends)
            {
                if (f.friendInfo.Id == Id)
                    return f;
            }
            return null;
        }

        internal void AddFriend(Character friend)
        {
            TCharacterFriend tf = new TCharacterFriend()
            {
                Class = friend.Info.ConfigId,
                FriendID = friend.Info.Id,
                FriendName = friend.Info.Name,
                level = friend.Info.Level,

            };
            this.owner.Data.Friends.Add(tf);
            this.friendChange = true;
        }



        internal bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem = owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if(removeItem!=null)
            {
                //removeItem.TCharacterID = -1;
                IList<TCharacterFriend> RemoveList = new List<TCharacterFriend>(){
                    removeItem
                };

                DBService.Instance.Entities.TCharacterFriends.RemoveRange(RemoveList);
                //owner.Data.Friends.Remove(removeItem);
                this.friendChange = true;
                return true;
            }
            return true; 

        }
        internal bool RemoveFriendById(int Id)
        {
            var removeItem = owner.Data.Friends.FirstOrDefault(v => v.Id == Id);
            if (removeItem != null)
            {
                //removeItem.TCharacterID = -1;
                IList<TCharacterFriend> RemoveList = new List<TCharacterFriend>(){
                    removeItem
                };

                DBService.Instance.Entities.TCharacterFriends.RemoveRange(RemoveList);
                //owner.Data.Friends.Remove(removeItem);
                this.friendChange = true;
                return true;
            }
            return true;

        }
        public void PostProcess(NetMessageResponse message)
        {
            if(this.friendChange)
            {
                this.InitFriends();
                if(message.friendList==null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(this.friends);
                }
                this.friendChange = false;
            }
        }
    }
}
