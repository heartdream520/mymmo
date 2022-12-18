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
            Allentities.Remove(entity);
            MapEntities[mapid].Remove(entity);
        }
    }
}
