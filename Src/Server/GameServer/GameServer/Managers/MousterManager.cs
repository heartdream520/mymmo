using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;
using GameServer.Models;

namespace GameServer.Managers
{
    class MousterManager
    {
        private Map map;
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();
        public void  Init(Map map)
        {
            this.map = map;
        }
        public Monster Creat(int Tid,int level,NVector3 pos,NVector3 dir)
        {
            Monster monster = new Monster(Tid, level, pos, dir);
            EntityManager.Instance.AddEntity(this.map.ID, monster);
            monster.Info.Id = monster.entityId;
            monster.Info.EnityId = monster.entityId;
            monster.Info.mapId = this.map.ID;
            this.map.MonsterEnter(monster);
            this.Monsters[monster.entityId] = monster;
            return monster;
        }
    }
}
