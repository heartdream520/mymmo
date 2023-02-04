using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Gulid
    {
        public double timestamp;
        public TGulid Date;
        public int GulidId { get { return this.Date.Id; } }
        public string name { get { return this.Date.Name; } }
        public int LeaderId { get { return this.Date.LeaderID; } }
       

        public Gulid(TGulid Date)
        {

            this.timestamp = TimeUtil.timestamp;
            this.Date = Date;
        }


        internal NGulidInfo GetGulidInfo(Character cha)
        {
            var info = this.GetInListInfo();
            
            info.Members.AddRange( this.GetMemberInfo(this.Date.Members));
            if(this.Is_Manager(cha.Id))
            {
                info.Applies.AddRange(this.GetApplyList());
            }
            return info;
        }

        private List<NGulidApplyInfo> GetApplyList()
        {
            var list = new List<NGulidApplyInfo>();

            foreach(var a in this.Date.Applies)
            {
                var info = new NGulidApplyInfo();
                if (a.Result != (int)ApplyResult.None) continue;
                info.ApplyResult = ApplyResult.None;
                info.characterId = a.CharacterId;
                info.Class = a.Class;
                info.GulidId = a.TGulidId;
                info.Level = a.Level;
                info.Name = a.Name;
                list.Add(info);
            }
            return list;
        }

        public bool Is_Manager(int id)
        {
            if (this.LeaderId == id) return true;
            var member = this.Date.Members.FirstOrDefault(v => v.CharacterId == id);
            if(member!=null)
            {
                if (member.Title != (int)GulidTitle.None)
                    return true;
            }
            return false;
        }

        internal NGulidInfo GetInListInfo()
        {
            var info = new NGulidInfo();
            info.creatTime = (long)TimeUtil.GetTimeStamp(this.Date.CreateTime);
            info.Id = this.Date.Id;
            info.GulidName = this.Date.Name;

            info.LeaderId = this.Date.LeaderID;
            info.LeaderName = this.Date.LeaderName;
            info.memberCount = this.Date.Members.Count;
            info.Notice = this.Date.Notice;
            int num = 0;
            foreach(var m in this.Date.Members)
            {
                if (SessionManager.Instance.TryGetSession(m.CharacterId) != null)
                    num++;
            }
            info.loadmemberCount = num;
            return info;
        }
        private List<NGulidMemberInfo> GetMemberInfo(ICollection<TGulidMember> members)
        {
            var list = new  List<NGulidMemberInfo>();
            foreach(var m in this.Date.Members)
            {
                var info = new NGulidMemberInfo();
                info.characterId = m.CharacterId;
                info.Characterinfo = CharacterManager.Instance.GetBaseCharacterInfo(m.CharacterId);
                info.Id = m.Id;
                info.joinTime = (long)TimeUtil.GetTimeStamp(m.JoinTime);
                info.lastLoadTime = (long)TimeUtil.GetTimeStamp(m.LastLoadTime);
                info.Status = SessionManager.Instance.TryGetSession(info.characterId) == null ? 0 : 1;
                info.Title = (GulidTitle)m.Title;
                list.Add(info);
            }
            return list;
        }

        internal bool JoinApply(NGulidApplyInfo apply)
        {
            var oldApply = this.Date.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null) return false;
            var dbApply = DBService.Instance.Entities.TGulidApplies.Create();
            dbApply.ApplyTime = DateTime.Now;
            dbApply.CharacterId = apply.characterId;
            dbApply.Class = apply.Class;
            dbApply.Level= apply.Level;
            dbApply.Name= apply.Name;
            dbApply.Result =(int) ApplyResult.None;
            dbApply.TGulidId = this.Date.Id;
            DBService.Instance.Entities.TGulidApplies.Add(dbApply);
            this.Date.Applies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;

            this.SendMessageToLoadLeader();
            return true;


        }

        internal bool JoinAccept(NGulidApplyInfo apply)
        {
            var oldApply = this.Date.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply == null)
            {
                Log.ErrorFormat("Gulid->JoinAccept applyCId{0} not Exist",apply.characterId);
                return false;
            }

            oldApply.Result = (int)apply.ApplyResult;
            if(apply.ApplyResult==ApplyResult.Accept)
            {
                this.AddMember(apply.characterId,apply.Name, apply.Class, apply.Level, GulidTitle.None);
            }
            DBService.Instance.Save();

            this.timestamp = TimeUtil.timestamp;
            
            return true;


        }
        /// <summary>
        /// 在工会列表中的信息
        /// </summary>
        /// <returns></returns>
        internal void AddMember(int characterId,string name, int @class, int level, GulidTitle title)
        {
            DateTime now = DateTime.Now;
            var dbmember = DBService.Instance.Entities.TGulidMembers.Create();
            dbmember.CharacterId = characterId;
            dbmember.Name = name;
            dbmember.Class = @class;
            dbmember.Level = level;
            dbmember.Title = (int)title;
            dbmember.TGulidId = this.Date.Id;
            dbmember.JoinTime = now;
            dbmember.LastLoadTime = now;
            this.Date.Members.Add(dbmember);
            DBService.Instance.Save();

            var dbcha = DBService.Instance.Entities.Characters.FirstOrDefault(v => v.ID == characterId);
            dbcha.GulidId = this.Date.Id;
            DBService.Instance.Save();

            this.timestamp = Time.timestamp;
        }

        /// <summary>
        /// 处理是否离开成功并返回信息
        /// </summary>
        internal bool MumberLeave(Character cha, out string msg)
        {
            if(this.LeaderId==cha.Id)
            {
                msg = "请先转让会长在离开";
                return false;
            }
            this.DeleteMember(cha.Id,GulidLeaveWay.Self);
            msg = "退出公会成功！";
            SendMessageToLoadMember();
            return true;
        }

        private void DeleteMember(int characterId, GulidLeaveWay way)
        {
            var con = SessionManager.Instance.TryGetSession(characterId);
            if(con!=null)
            {
                var cha = con.Session.Character;
                cha.GulidLeaveWay = way;
                cha.Gulid = null;
            }
            
            var removeApply = DBService.Instance.Entities.TGulidApplies.FirstOrDefault(v => v.CharacterId == characterId && v.TGulidId == this.GulidId);
            if (removeApply != null)
            {
                IList<TGulidApply> RemoveList = new List<TGulidApply>(){
                    removeApply
                };
                DBService.Instance.Entities.TGulidApplies.RemoveRange(RemoveList);
            }
            
            
            var dbCharacter = DBService.Instance.Entities.Characters.FirstOrDefault(v => v.ID == characterId);
            if (dbCharacter!=null)
            {
                dbCharacter.GulidId = 0;
                
            }

            var remove = DBService.Instance.Entities.TGulidMembers.FirstOrDefault(v => v.CharacterId == characterId);
            if (remove != null)
            {
                IList<TGulidMember> RemoveList = new List<TGulidMember>(){
                    remove
                };

                DBService.Instance.Entities.TGulidMembers.RemoveRange(RemoveList);
            }
            DBService.Instance.Save();
            this.timestamp = Time.timestamp;
        }

        internal void PostProcess(Character character, NetMessageResponse message)
        {
            if(message.gulidInfo==null)
            {
                message.gulidInfo = new GulidInfoResponse();
                message.gulidInfo.Result = Result.Success;
                message.gulidInfo.GulidInfo = this.GetGulidInfo(character);
            }
        }
        public void SendMessageToLoadMember()
        {
            foreach(var m in this.Date.Members)
            {
                var con = SessionManager.Instance.TryGetSession(m.CharacterId);
                if (con != null)
                    con.SendResponse();
            }
        }
        public void SendMessageToLoadLeader()
        {
            foreach (var m in this.Date.Members)
            {
                if (m.Title == (int)GulidTitle.None) continue;
                var con = SessionManager.Instance.TryGetSession(m.CharacterId);
                if (con != null)
                    con.SendResponse();
            }
        }
        internal bool OnGulidAdmin(Character sendCha,GulidAdminCommand command, int target, out string msg)
        {
            var sendMember = DBService.Instance.Entities.TGulidMembers.FirstOrDefault(v => v.CharacterId == sendCha.Id && v.TGulidId == this.GulidId);
            if(sendMember==null)
            {
                Log.ErrorFormat("Gulid->OnGulidAdmin sendMember:{0} not Exist ", sendCha);
                msg = "您已经不在此公会中";
                return false;
            }
            if(command==GulidAdminCommand.Dispand)
            {
                if(sendMember.Title!=(int)GulidTitle.President)
                {
                    msg = "你不是会长没有解散公会的权力！";
                    return false;
                }
                this.OnDispand();
                msg = "公会解散成功！";
                return true;
            }
            if (sendCha.Id == target)
            {
                msg = "不能对自己进行此操作";
                return false;
            }
            var sendToMember = DBService.Instance.Entities.TGulidMembers.FirstOrDefault(v => v.CharacterId == target&& v.TGulidId == this.GulidId);
            if(sendToMember==null)
            {
                Log.ErrorFormat("Gulid->OnGulidAdmin sendMember:{0} not Exist ", sendCha);
                msg = "公会中没有此成员";
                return false;
            }
            var sendtoCon = SessionManager.Instance.TryGetSession(sendToMember.CharacterId);
            Character sendtoCha = null;
            if(sendtoCon!=null)
            {
                sendtoCha = sendtoCon.Session.Character;
            }
            switch (command)
            {
                case GulidAdminCommand.Kickout:
                    this.DeleteMember(target,GulidLeaveWay.Kickout);
                    msg = "已成功踢出成员";
                    if (sendtoCon != null)
                    {
                        sendtoCon.SendResponse();
                    }
                        
                    return true;


                case GulidAdminCommand.Promote:
                    this.OnMemberChangeTitle(sendToMember, GulidTitle.VicePresident);
                    msg = "晋升成功";
                    if (sendtoCon != null)
                    {
                        sendtoCon.Session.Response.gulidAdmin = new GulidAdminResponse();
                        sendtoCon.Session.Response.gulidAdmin.Result = Result.Success;
                        sendtoCon.Session.Response.gulidAdmin.Errormsg = "您已被晋升为副会长";
                        sendtoCon.SendResponse();
                    }
                    return true;


                case GulidAdminCommand.Transfer:
                    this.OnMemberChangeTitle(sendToMember, GulidTitle.President);
                    this.OnMemberChangeTitle(sendMember, GulidTitle.VicePresident);
                    if (sendtoCon != null)
                    {
                        sendtoCon.Session.Response.gulidAdmin = new GulidAdminResponse();
                        sendtoCon.Session.Response.gulidAdmin.Result = Result.Success;
                        sendtoCon.Session.Response.gulidAdmin.Errormsg = "您已被转让为会长";
                        sendtoCon.SendResponse();
                    }
                    msg = "转让会长成功";
                    return true;


                default:
                    break;
            }
            msg = "Gulid出现不科学错误！";
            return true;

        }

        private void OnMemberChangeTitle(TGulidMember member, GulidTitle title)
        {
            member.Title = (int)title;
            if(title==GulidTitle.President)
            {
                this.Date.LeaderID = member.CharacterId;
                this.Date.LeaderName = member.Name;
            }
            DBService.Instance.Save();
        }

        private void OnDispand()
        {
            IList<TGulidMember> RemoveMumbetList = new List<TGulidMember>();
            foreach (var m in this.Date.Members)
            {
                int cId = m.CharacterId;
                var con = SessionManager.Instance.TryGetSession(cId);
                if(con!=null)
                {
                    con.Session.Character.Gulid = null;
                    con.Session.Character.GulidLeaveWay = GulidLeaveWay.Dispand;
                }
                RemoveMumbetList.Add(m);
            }
            

            this.SendMessageToLoadMember();
            DBService.Instance.Entities.TGulidMembers.RemoveRange(RemoveMumbetList);

            IList<TGulidApply> RemoveApplyList = new List<TGulidApply>();
            foreach (var a in this.Date.Applies)
            {
                RemoveApplyList.Add(a);
            }
            DBService.Instance.Entities.TGulidApplies.RemoveRange(RemoveApplyList);

            IList<TGulid> RemoveGulidList = new List<TGulid>();
            var removeGulid = DBService.Instance.Entities.TGulids.FirstOrDefault(v => v.Id == this.GulidId);
            if(removeGulid!=null)
            {
                RemoveGulidList.Add(removeGulid);
                DBService.Instance.Entities.TGulids.RemoveRange(RemoveGulidList);
            }
            
            
            DBService.Instance.Save();

        }
    }
}
