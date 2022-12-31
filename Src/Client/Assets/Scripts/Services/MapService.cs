using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Common.Data;
using Entities;
using Assets.Scripts.Managers;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {

        public bool switchover_scene = false;
        public string new_scene_name;
        /// <summary>
        /// 当前地图ID
        public int CurrentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

        }

      

        public void Dispose()
        /// </summary>
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("MapService->OnMapCharacterEnter :Map:{0} Count:{1}",
                response.mapId, response.Characters.Count);

            new_scene_name = DataManager.Instance.Maps[response.mapId].Resource;
            //遍历角色
            foreach (var cha in response.Characters)
            {
                //刷新本地数据确保安全
                if (User.Instance.CurrentCharacter == null || User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    //当前角色切换地图
                    User.Instance.CurrentCharacter = cha;
                }
                //将此地图中所有角色加入角色管理器中
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)
            {
                //切换地图
                this.EnterMap(response.mapId);
                //更新地图id
                this.CurrentMapId = response.mapId;
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Character cha = CharacterManager.Instance.Characters[response.characterId];
            Debug.LogFormat("MapService->OnMapCharacterLeave :Map:{0} CharacterId:{1} CharacterName:{2}",
                CurrentMapId, cha.Info.Id,cha.Info.Name);

            if (response.characterId == User.Instance.CurrentCharacter.Id)
            {
                CharacterManager.Instance.Clear();
            }
            else
                CharacterManager.Instance.RemoveCharacter(response.characterId);
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                MySceneManager.Instance.LoadScene(map.Resource);
                
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }
        public void SendMapEntitySync(EntityEvent entityEvent,NEntity entity)
        {
            Debug.LogFormat("MapService->SendMapEntitySync EntityId:{0} Pos:{1} Dir:{2},Spd:{3}",
                entity.Id, entity.Position.ToString(), entity.Direction.ToString(), entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }
        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("MapService->OnMapEntitySync :Entitys:{0}",
                response.entitySyncs.Count);
            sb.AppendLine();
            foreach(var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEnetitySync(entity);
                sb.AppendFormat("EntityID:{0}  Enent:{1} Entity:{2}",
                    entity.Id, entity.Event, entity.Event.ToString());
            }
            Debug.Log(sb);
        }
        public void SendMapTeleport(int id)
        {
            Debug.LogFormat("Mapservice->SendMapTeleport TeleportID:{0}", id);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = id;
            NetClient.Instance.SendMessage(message);

        }
    }
}
