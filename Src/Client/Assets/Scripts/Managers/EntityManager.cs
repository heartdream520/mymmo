using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using Entities;
using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanaged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }
    class EntityManager : Singleton<EntityManager>
    {

        Dictionary<int, Entity> entitys = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();
        public void RegisterEntityChangeNotify(int entityId,IEntityNotify notify)
        {
            this.notifiers[entityId] = notify;
        }
        public void AddEntity(Entity entity)
        {
            Debug.LogFormat("EntityManager AddEntity EntityId :{0} ", entity.entityId);
            entitys[entity.entityId] = entity;
        }
        

        public void RemoveEntity(NEntity entity)
        {
            Debug.LogFormat("EntityManager RemoveEntity EntityId :{0} ", entity.Id);
            this.entitys.Remove(entity.Id);
            if(notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        
        }
        
        internal void OnEnetitySync(NEntitySync data)
        {
            Entity entity = null;
            this.entitys.TryGetValue(data.Id, out entity);
            if (entity != null)
            {
                if(data.Entity!=null)
                {
                    entity.EntityData = data.Entity;
                }
                if(notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanaged(entity);
                    notifiers[entity.entityId].OnEntityEvent(data.Event);

                }
            }
        }
    }
}
