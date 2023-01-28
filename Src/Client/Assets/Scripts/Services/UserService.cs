using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result, string> OnLoad;
        public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;
        public UnityEngine.Events.UnityAction<Result, string> OnGameEnter;
        public UnityEngine.Events.UnityAction<Result, string> OnGameLeave;

        //消息队列
        NetMessage pendingMessage = null;
        bool connected = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            //绑定返回信息处理函数
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLoad);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCharacterCreate);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnUserGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnUserGameLeave);
            
        }

       

        public void Dispose()
        {
            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
            //解绑返回信息处理函数
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLoad);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCharacterCreate);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnUserGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnUserGameLeave);

        }

        public void Init()
        {

        }

        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            //出现错误
            Log.Init("Unity");
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if(this.pendingMessage!=null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }

        

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result,string reason)
        {
            //发送服务器断开信息
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userLogin!=null)
                {
                    if (this.OnLoad != null)
                    {
                        this.OnLoad(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else if(this.pendingMessage.Request.userRegister!=null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送注册消息
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="psw">密码</param>
        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserService->SendRegister: User:{0} Password:{1}",
                user, psw);
            //发送消息到服务器
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// 发送用户登录信息
        /// </summary>
        /// <param name="user">登录用户名</param>
        /// <param name="psw">登录用户密码</param>
        public void SendLoad(string user, string psw)
        {
            Debug.LogFormat("UserService->SendLoad: User:{0} Password:{1}",
                user, psw);
            //发送消息到服务器
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// 发送角色创建信息
        /// </summary>
        /// <param name="char_class">角色类型</param>
        /// <param name="text">角色名</param>
        public void SendCharacterCreate(CharacterClass char_class, string text)
        {
            
            Debug.LogFormat("UserService->SendCharacterCreate: CharacterName :{0} Class:{1}",
                text ,char_class);
            //发送消息到服务器
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Class =char_class;
            message.Request.createChar.Name = text;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        public void SendCharacterEnter(int idx)
        {

            var cha = User.Instance.Info.Player.Characters[idx];
            Debug.LogFormat("UserService->SendCharacterEnter: EnterCharacterID :{0} EnterCharacterName:{1} EnterCharacter_Idx",
                cha.EnityId, cha.Name,idx);
            
            //发送消息到服务器
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = idx;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        public void SendCharacterLeave()
        {
            NCharacterInfo cha = User.Instance.CurrentCharacter;
            Debug.LogFormat("UserService->SendCharacterLeave: LeaveCharacterID :{0} LeaveCharacterName:{1}", 
                cha.EnityId,cha.Name);

            
            //发送消息到服务器
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            message.Request.gameLeave.Character = User.Instance.CurrentCharacter;
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// 用户注册返回信息
        /// </summary>
        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("UserService->OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }
        /// <summary>
        /// 用户登录返回信息
        /// </summary>
        void OnUserLoad(object sender, UserLoginResponse response)
        {
            Debug.LogFormat("UserService->OnUserLoad:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnLoad != null)
            {
                this.OnLoad(response.Result, response.Errormsg);
            }
            if(response.Result==Result.Success)
            {
                //设置当前角色
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            }
        }
        /// <summary>
        /// 角色创建返回信息
        /// </summary>
        private void OnUserCharacterCreate(object sender, UserCreateCharacterResponse response)
        {
            Debug.LogFormat("UserService->OnUserCharacterCreate:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result==Result.Success)
            {
                //接受返回的新建角色
                User.Instance.Info.Player.Characters.Clear();
                foreach (var x in response.Characters)
                {
                    User.Instance.Info.Player.Characters.Add(x);
                    
                }
                    
               
            }
            if (this.OnCharacterCreate != null)
            {
                this.OnCharacterCreate(response.Result, response.Errormsg);
            }
        }
        /// <summary>
        /// 角色进入信息返回信息
        /// </summary>
        private void OnUserGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("UserService->OnUserGameEnter:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                User.Instance.CurrentCharacter = response.Ncharacterinfo;

                ItemManager.Instance.Init(response.Ncharacterinfo.Items);
                BagManager.Instance.Init(response.Ncharacterinfo.Bag);
                EquipManager.Instance.Init(response.Ncharacterinfo.Equips);

                QuestManager.Instance.Init(response.Ncharacterinfo.Quests);
                FriendManager.Instance.Init(response.Ncharacterinfo.Friends);
                TeamManager.Instance.Init();
            }
            if (this.OnGameEnter != null)
            {
                this.OnGameEnter(response.Result, response.Errormsg);
            }
        }
        /// <summary>
        /// 玩家离开信息返回信息
        /// </summary>
        private void OnUserGameLeave(object sender, UserGameLeaveResponse response)
        {
            Debug.LogFormat("UserService->OnUserGameLeave:{0} [{1}]", response.Result, response.Errormsg);
            MapService.Instance.CurrentMapId = 0;
            User.Instance.CurrentCharacter = null;
        }
    }
}
