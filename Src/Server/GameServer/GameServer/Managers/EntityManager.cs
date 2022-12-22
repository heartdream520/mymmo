using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;
namespace GameServer.Managers
{
    class EntityManager :Singleton<EntityManager>
    {
        //计数用
        private int idx;
        public List<Entity> Allentities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();
        public void AddEntity(int mapId,Entity entity)
        {

            
            Allentities.Add(entity);
            entity.EntityData.Id = ++idx;
            Log.InfoFormat("EntityManager->AddEntity: MapId:{0} EntityId:{1} ", mapId, entity.entityId);
            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapId,out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities; 
            }
            entities.Add(entity);
        }
        public void RemoveEntity(int mapid,Entity entity)
        {
            Log.InfoFormat("EntityManager->RemoveEntity: MapId:{0} EntityId:{1} ", mapid, entity.entityId);
            if (!Allentities.Exists(t => t.entityId == entity.entityId))
            {
                Log.WarningFormat("EntityManager->RemoveEntity: Allentities not exited EntityId:{0} ",entity.entityId);
            }
            else
                Allentities.Remove(entity);

            if (!MapEntities[mapid].Exists(t => t.entityId == entity.entityId))
            {
                Log.WarningFormat("EntityManager->RemoveEntity: MapEntities[{0}] not exited EntityId:{1} ", mapid, entity.entityId);
            }
            else
                MapEntities[mapid].Remove(entity);
        }
        public void ChangeEntity_Map(Entity entity,int from_Map,int to_Map)
        {
            Log.InfoFormat("EntityManager->ChangeEntity_Map: EntityId:{0},from_Map_ID:{1} to_Map_Id:{2} ",entity.entityId,from_Map,to_Map);

            if (!MapEntities[from_Map].Exists(t => t.entityId == entity.entityId))
            {
                Log.WarningFormat("EntityManager->ChangeEntity_Map: MapEntities[{0}] not exited EntityId:{1} ",from_Map, entity.entityId);
            }
            else
                MapEntities[from_Map].Remove(entity);

            List<Entity> entities = null;
            if (!MapEntities.TryGetValue(to_Map, out entities))
            {
                entities = new List<Entity>();
                MapEntities[to_Map] = entities;
            }
            entities.Add(entity);
        }
    }
}
