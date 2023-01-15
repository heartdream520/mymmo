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
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }

        private Map Map;
        private float spawnTime;
        private float unspawnTime;

        private SpawnPointDefine spawnPoint = null;
        private bool spawned;

        public Spawner(SpawnRuleDefine define, Map map)
        {
            Define = define;
            Map = map;
            if(DataManager.Instance.SpawnPoints.ContainsKey(this.Map.ID))
            {
                if(DataManager.Instance.SpawnPoints[this.Map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    this.spawnPoint = DataManager.Instance.SpawnPoints[this.Map.ID][this.Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("Spawner-> SpawnRule [{0}] SpawnPoint:{1} not existed",
                        this.Define.ID,this.Define.SpawnPoint);
                }
            }
        }

        public void Update()
        {
            if (this.CanSpawn())
                this.spawn();
        }

        private bool CanSpawn()
        {
            if (this.spawned)
                return false;
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time)
                return false;
            return true;

        }

        private void spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map:{0} Monuse:{1} Lv:{2} At Point:{3}",
                this.Define.MapID, this.Define.SpawnMonID, this.Define.SpawnLevel, this.Define.SpawnPoint);
            this.Map.MousterManager.Creat(this.Define.SpawnMonID,this.Define.SpawnLevel,this.spawnPoint.Position,this.spawnPoint.Direction);
        }
    }
}
