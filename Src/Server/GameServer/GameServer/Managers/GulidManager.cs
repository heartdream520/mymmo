using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Services;
using GameServer.Models;

namespace GameServer.Managers
{
    class GulidManager:Singleton<GulidManager>
    {
        public Dictionary<int, Gulid> Gulids = new Dictionary<int, Gulid>();
        private HashSet<string> GulidNames = new HashSet<string>();
        public void Init()
        {
            this.Gulids.Clear();
            this.GulidNames.Clear();
            foreach(var g in DBService.Instance.Entities.TGulids)
            {
                this.AddGulid(new Gulid(g));
            }
        }

        private void AddGulid(Gulid gulid)
        {
            this.Gulids.Add(gulid.GulidId, gulid);
            this.GulidNames.Add(gulid.Date.Name);
        }

        internal Gulid GetGulidById(int gulidId)
        {
            Gulid gulid;
            this.Gulids.TryGetValue(gulidId, out gulid);
            return gulid;
        }
        
        internal bool ExitGulidName(string gulidName)
        {
            return this.GulidNames.Contains(gulidName);
        }

        internal void CreateGulid(string gulidName, string gulidNotice, Character Leader)
        {
            DateTime now = DateTime.Now;
            var dGulid = DBService.Instance.Entities.TGulids.Create();
            dGulid.Name = gulidName;
            dGulid.Notice = gulidNotice;
            dGulid.LeaderID = Leader.Id;
            dGulid.LeaderName = Leader.Name;
            dGulid.CreateTime = now;
            DBService.Instance.Entities.TGulids.Add(dGulid);

            DBService.Instance.Save();

            Gulid gulid = new Gulid(dGulid);
            gulid.AddMember(Leader.Id,Leader.Name, Leader.Class, Leader.Data.Level, GulidTitle.President);
            Leader.Gulid = gulid;
            
            Leader.Data.GulidId = dGulid.Id;
            DBService.Instance.Save();
            this.AddGulid(gulid);

            

        }

        internal List<NGulidInfo> GetGulidList()
        {
            var list = new List<NGulidInfo>();

            foreach(var g in this.Gulids.Values)
            {
                list.Add(g.GetInListInfo());
            }

            return list;
        }
    }
}
