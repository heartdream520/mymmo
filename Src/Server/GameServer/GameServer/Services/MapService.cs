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
               connection.Session.Character.Info.Id, connection.Session.Character.Info.Name,entitySync.Id);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entitySync);
            byte[] data = PackageHandler.PackMessage(message);
            connection.SendData(data, 0, data.Length);

        }
    }
}
