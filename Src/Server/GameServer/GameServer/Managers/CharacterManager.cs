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
    class CharacterManager : Singleton<CharacterManager>
    {
        /// <summary>
        /// ID 为数据库中的ID
        /// </summary>
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public CharacterManager()
        {
        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            Log.InfoFormat("CharacterManager->Clear()");
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter Tcharacter)
        {
            Character cha = new Character(CharacterType.Player, Tcharacter);
            EntityManager.Instance.AddEntity(cha.Info.mapId, cha);
            Log.InfoFormat("CharacterManager->AddCharacter(): MapId:{0} CharacterId:{1} EntityId:{2} InfoId:{3} ",
                cha.Data.MapID, cha.Id,cha.entityId, cha.Info.Id);
            //cha.Info.Id = cha.Id;
            this.Characters[cha.entityId] = cha;
            
            return cha;
        }


        public void RemoveCharacter(int characterId)
        {
            Character cha = this.Characters[characterId];
            Log.InfoFormat("CharacterManager->RemoveCharacter:  MapId:{0}  CharacterId:{1} EntityId:{2}", cha.Data.MapID,characterId,cha.entityId);
            if(!this.Characters.ContainsKey(characterId))
            {
                Log.WarningFormat("CharacterManager->RemoveCharacter:  MapId:{0} not Exist CharacterId:{1} EntityId:{2}", 
                    cha.Data.MapID, characterId, cha.entityId);
                return;

            }
            EntityManager.Instance.RemoveEntity(cha.Info.mapId,cha);
            this.Characters.Remove(characterId);

        }
        
    }
}
