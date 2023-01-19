using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapCharacterEnterRequest>(this.OnMapCharacterLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapSyncRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);

        }

       
        public void Init()
        {
            MapManager.Instance.Init();
        }

        private void OnMapCharacterEnter(NetConnection<NetSession> sender, MapCharacterEnterRequest request)
        {
            throw new NotImplementedException();
        }
        private void OnMapCharacterLeave(NetConnection<NetSession> sender, MapCharacterEnterRequest request)
        {
            throw new NotImplementedException();
        }


        
        private void OnMapSyncRequest(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("MapService->OnMapSyncRequest SyncCharacterId:{0} Name:{1} EntityID:{2} Event:{3} ",
                character.Id,character.Info.Name,character.entityId,request.entitySync.Event);
            NEntitySync entity = request.entitySync;
            Log.InfoFormat("SyncEntityData Entitypos:{0} EntityDir:{1} Event:{2} EntitySpeed:{3} ",
                entity.Entity.Position,entity.Entity.Direction,entity.Event,entity.Entity.Speed);
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }

        internal void SendEntityUpdata(NetConnection<NetSession> connection, NEntitySync entitySync)
        {

            Log.InfoFormat("MapService->SendEntityUpdata SendCharacter: Infoid:{0} InfoName:{1} SyncCharacterID:{2}",
               connection.Session.Character.Info.EnityId, connection.Session.Character.Info.Name,entitySync.Id);
            connection.Session.Response.mapEntitySync = new MapEntitySyncResponse();
            connection.Session.Response.mapEntitySync.entitySyncs.Add(entitySync);
            connection.SendResponse();

        }
        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("MapService->OnMapTeleport :CharacterEntityID:{0} CharacterID:{1} TeleportID:{2}",
                character.entityId, character.Id, request.teleporterId);
            //不存在的传送点ID
            if(!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.ErrorFormat("MapService->OnMapTeleport TeleportID:{0} Not Exised", request.teleporterId);
                return;
            }
            TeleporterDefine teleporterDefine = DataManager.Instance.Teleporters[request.teleporterId];
            if(teleporterDefine.LinkTo==0||!DataManager.Instance.Teleporters.ContainsKey(teleporterDefine.LinkTo))
            {
                Log.ErrorFormat("MapService->OnMapTeleport TeleportID:{0} LinkTo:{1} Not Exised", 
                    request.teleporterId,teleporterDefine.LinkTo);
                return;
            }
            MapManager.Instance[character.Info.mapId].CharacterLevel(character);

            int from_Map_Id = character.Info.mapId;
            teleporterDefine = DataManager.Instance.Teleporters[teleporterDefine.LinkTo];
            character.Position = teleporterDefine.Position;
            character.Direction = teleporterDefine.Direction;
            character.Speed = 0;
            character.Info.mapId = teleporterDefine.MapID;

            EntityManager.Instance.ChangeEntity_Map(character,from_Map_Id, teleporterDefine.MapID);
            MapManager.Instance[teleporterDefine.MapID].CharacterEnter(sender, character);
        }

    }
}
