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

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;
        }

        internal void Update()
        {
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="cha"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character cha)
        {
            Log.InfoFormat("Map->CharacterEnter : MapID :{0} CharacterInfoId:{1} CharacterDataId:{2} EntityID:{3} ",
               cha.Info.mapId, cha.Info.Id, cha.Data.ID, cha.entityId);

            cha.Info.mapId = this.ID;

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();

            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(cha.Info);

            Log.ErrorFormat("CharacterEnter");
            //通知此地图其他角色新角色的进入
            foreach (var kv in this.MapCharacters)
            {
                Log.ErrorFormat("InMapCharacters: CharacterInfoId:{0} CharacterId:{1}  EntityID:{2}",
                    kv.Value.character.Info.Id, kv.Value.character.Id, kv.Value.character.EntityData.Id);
                message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                this.SendCharacterEnterMap(kv.Value.connection, cha.Info);
            }
            //Log.ErrorFormat("新加入角色ID{0}", character.Id);
            //将新角色加入到此地图角色的字典中
            this.MapCharacters[cha.entityId] = new MapCharacter(conn, cha);
            
            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        

        /// <summary>
        /// 通知地图中现有角色其他角色进入地图
        /// </summary>
        /// <param name="sendconn"></param>
        /// <param name="character"></param>
        void SendCharacterEnterMap(NetConnection<NetSession> sendconn, NCharacterInfo character)
        {
            Character sendCha = sendconn.Session.Character;
            Log.InfoFormat("Map->SendCharacterEnterMap sendCharacter : MapID:{0} CharacterDataId:{1} EntityID:{2} ",
                sendCha.Data.MapID, sendCha.Data.ID, sendCha.entityId);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(message);
            sendconn.SendData(data, 0, data.Length);
        }

        public void CharacterLevel(Character cha)
        {
            Log.InfoFormat("Map->CharacterLevel : MapID :{0} CharacterDataId:{1} EntityID:{2} ",
                cha.Info.mapId, cha.Data.ID,cha.entityId);
            if(!MapCharacters.ContainsKey(cha.entityId))
            {
                Log.WarningFormat("Map->CharacterLevel : MapCharacters not have key EntityId :{0}",
                    cha.entityId);
                return;
            }
            //发送离开信息
            foreach(var kv in this.MapCharacters)
            {
                this.SendCharacterLevelMap(kv.Value.connection, cha);
            }
            this.MapCharacters.Remove(cha.entityId);
        }

        private void SendCharacterLevelMap(NetConnection<NetSession> sendconn, Character cha)
        {
            Character sendCha = sendconn.Session.Character;
            Log.InfoFormat("Map SendCharacterLevelMap sendCharacter : MapID:{0} CharacterDataId:{1} EntityID:{2} ", 
                sendCha.Data.MapID,sendCha.Data.ID,sendCha.entityId);
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            message.Response.mapCharacterLeave.characterId = cha.entityId;

            byte[] data = PackageHandler.PackMessage(message);
            sendconn.SendData(data, 0, data.Length);
        }
        public void UpdateEntity(NEntitySync entitySync)
        {
            Log.InfoFormat("Map UpdateEntity SyncEntity : Id:{0}  ",
                entitySync.Id);
            foreach (var k in this.MapCharacters)
            {
                MapCharacter cha = this.MapCharacters[k.Key];
                Log.InfoFormat("Map UpdateEntity SendMapCharacter : Id:{0} InfoId:{1} Name:{2} ",
                cha.character.Id,cha.character.Info.Id,cha.character.Info.Name);
                if (k.Value.character.entityId==entitySync.Id)
                {
                    k.Value.character.Position = entitySync.Entity.Position;
                    k.Value.character.Direction = entitySync.Entity.Direction;
                    k.Value.character.Speed = entitySync.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdata(k.Value.connection, entitySync);
                }
            }
        }
    }
}
