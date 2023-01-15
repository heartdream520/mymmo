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
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        

        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender,UserLoginRequest request)
        {
            Log.InfoFormat("UserSerevice->OnLogin : UserName:{0}  PassWord:{1}", request.User, request.Passward);


            sender.Session.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if(user==null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";
            }
            else if(user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                //会话，代表当前连接的用户
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";

                //创建返回信息
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id =(int) user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                //更新玩家ID
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
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

                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(cha.Info);
                }
               
            }
            sender.SendResponse();
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserSerevice->OnRegister : UserName:{0}  PassWord:{1}", request.User, request.Passward);

            sender.Session.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "用户已存在.";
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
                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "None";
            }

            sender.SendResponse();
        }

        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {


            //将新建的角色加入数据库
            TCharacter Tcharacter = new TCharacter()
            {
                Name = request.Name,
                TID = (int)request.Class,
                Class = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
                Gold = 10000,
                Equips = new byte[28]

            };

            //为角色添加背包
            var bag = new TCharacterBag();
            bag.Owner = Tcharacter;
            bag.Items = new byte[0];
            bag.Unlocked = 50;
            Tcharacter.Bag = DBService.Instance.Entities.TCharacterBags.Add(bag);

            Tcharacter.Items.Add(new TCharacterItem()
            {
                Owner = Tcharacter,
                ItemID = 1,
                ItemCount=20
            });
            Tcharacter.Items.Add(new TCharacterItem()
            {
                Owner = Tcharacter,
                ItemID = 2,
                ItemCount = 20
            });


            DBService.Instance.Entities.Characters.Add(Tcharacter);
            sender.Session.User.Player.Characters.Add(Tcharacter);
            DBService.Instance.Entities.SaveChanges();
            
            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == sender.Session.User.Username).FirstOrDefault();
            //返回当前所有角色

            Log.InfoFormat("UserSerevice->OnCreateCharacter : UserName:{0} CharacterName:{1} CharacterDId:{2}"
                ,user.Username,request.Name,Tcharacter.ID);

            foreach (var c in user.Player.Characters)
            {
                Character cha = new Character(CharacterType.Player, c);

                sender.Session.Response.createChar.Characters.Add(cha.Info);
            }


            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";


            sender.SendResponse();
        }

        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            //通关下标确定进入游戏的玩家
            TCharacter db_character = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);

           
            //将此玩家加入当前在线玩家中
            Character cha = CharacterManager.Instance.AddCharacter(db_character);

            Log.InfoFormat("UserSerevice->OnGameEnter : userId：{0} character_DId:{1} EntityId:{2} character_Name：{3} MapId：{4}",
                sender.Session.User.ID, cha.Data.ID,cha.entityId, cha.Info.Name, cha.Info.mapId);

            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            
            //添加角色信息
            sender.Session.Response.gameEnter.Ncharacterinfo = cha.Info;

            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            sender.SendResponse();
            //设置当前的玩家
            sender.Session.Character = cha;
            //地图管理器，将进入游戏的角色加入相应地图中
            MapManager.Instance[db_character.MapID].CharacterEnter(sender, cha);
        }
        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("UserSerevice->OnGameLeave : userId：{0} character_DId:{1} EntityId:{2} character_Name：{3} MapId：{4}",
                 sender.Session.User.ID, cha.Data.ID, cha.entityId, cha.Info.Name, cha.Info.mapId);


            if (!CharacterManager.Instance.Characters.ContainsKey(cha.entityId))
            {
                Log.InfoFormat("UserSerevice->OnGameLeave CharacterManager.Characters not hava key : character_EntityId:{0}",
                 cha.Data.ID);
                return;
            }
            CharacterLeave(cha);

            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";

            sender.SendResponse();

        }

        public void CharacterLeave(Character cha)
        {
            if (cha == null) return;
            CharacterManager.Instance.RemoveCharacter(cha.entityId);

            MapManager.Instance[cha.Info.mapId].CharacterLevel(cha);
        }
    }
}
