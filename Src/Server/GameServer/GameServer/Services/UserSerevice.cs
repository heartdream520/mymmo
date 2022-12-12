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

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
        }

        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender,UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if(user==null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在";
            }
            else if(user.Password != request.Passward)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                //会话，代表当前连接的用户
                sender.Session.User = user;
    
                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "None";

                //创建返回信息
                message.Response.userLogin.Userinfo = new NUserInfo();
                message.Response.userLogin.Userinfo.Id = 1;
                message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                //更新玩家ID
                message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                //更新玩家角色
                foreach(var c in user.Player.Characters)
                {
                    /*
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    */
                    Character cha = new Character(CharacterType.Player, c);

                    message.Response.userLogin.Userinfo.Player.Characters.Add(cha.Info);
                }
               
            }
            byte[]  data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add
                    (new TUser()
                        {
                            Username = request.User, Password = request.Passward, Player = player
                        }
                    );
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacter: charclass:{0}  name:{1}", request.Class, request.Name);
            

            //将新建的角色加入数据库
            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                TID = (int)request.Class,
                Class = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820
            };
            DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();


            /*
            //将新建的角色返回
            NCharacterInfo info = new NCharacterInfo();
            info.Name = request.Name;
            info.Tid = (int)request.Class;
            info.Class = request.Class;
            info.mapId = 1;
            NVector3 nVector3= new NVector3();
            nVector3.X = 5000;nVector3.Y = 4000;nVector3.Z = 820;
            info.Entity = new NEntity();
            info.Entity.Position = nVector3;

            Character cha = new Character(CharacterType.Player,);
           
            message.Response.createChar.Characters.Add(info);
            */
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == sender.Session.User.Username).FirstOrDefault();

            foreach (var c in user.Player.Characters)
            {
                Character cha = new Character(CharacterType.Player, c);

                message.Response.createChar.Characters.Add(cha.Info);
            }


            message.Response.createChar.Result = Result.Success;
            message.Response.createChar.Errormsg = "None";
         

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            //通关下标确定进入游戏的玩家
            TCharacter db_character = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserCharacterEnter: userId：{0} character_Id：{1} character_Name：{2} MapId：{3}",
                sender.Session.User.ID,db_character.MapID,db_character.Name,db_character.MapID );

            //将此玩家加入当前在线玩家中
            Character character = CharacterManager.Instance.AddCharacter(db_character);


            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg = "None"; 
            
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);

            
            

            //设置当前的玩家
            sender.Session.Character = character;
            //地图管理器，将进入游戏的角色加入相应地图中
            MapManager.Instance[db_character.MapID].CharacterEnter(sender, character);
        }

        /*
        /// <summary>
        /// 处理登录信息
        /// </summary>
        /// <param name="sender">发送回的信息</param>
        /// <param name="request">登录信息</param>
        void OnLoad(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoadRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户不存在.";
            }
            else if(user.Password!=request.Passward)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "密码错误.";
            }
            else
            {
                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }
        */
    }
}
