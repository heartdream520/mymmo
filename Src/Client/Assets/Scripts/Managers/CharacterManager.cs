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

namespace Services
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

       
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
            int[] key = Characters.Keys.ToArray();
            foreach(var k in key)
            {
                this.RemoveCharacter(k);
            }
            this.Characters.Clear();
        }

        public void AddCharacter(SkillBridge.Message.NCharacterInfo cha)
        {
            Debug.LogFormat("CharacterManager->AddCharacter:CharacterId:{0} CharacterName:{1} MapId:{2} Entity:{3}",
                cha.Id, cha.Name, cha.mapId, cha.Entity.String());

            //.Info.Id == User.Instance.CurrentCharacter.Id
            //Debug.LogErrorFormat("character_info_id:{0}  User.Instance.CurrentCharacter.ID:{1}", cha.Id, User.Instance.CurrentCharacter.Id);
            Character character = new Character(cha);
            

            //实体管理器添加实体
            EntityManager.Instance.AddEntity(character);

            character.Info.Id = character.entityId;

            this.Characters[cha.Entity.Id] = character;
            if (OnCharacterEnter!=null)
            {
                OnCharacterEnter(character);
            }
        }


        public void RemoveCharacter(int characterId)
        {
            Character cha= this.Characters[characterId];
            Debug.LogFormat("CharacterManager->RemoveCharacter:CharacterId:{0} CharacterName:{1} MapId:{2} Entity:{3}",
                cha.Info.Id, cha.Name, cha.Info.mapId, cha.EntityData.String());
            Debug.LogFormat("RemoveCharacter->RemoveCharacter:{0}", characterId);
            
            if(this.Characters.ContainsKey(characterId))
            {
                //从实体管理器中删除
                EntityManager.Instance.RemoveEntity(cha.EntityData);
                if (OnCharacterLeave != null)
                    OnCharacterLeave(this.Characters[characterId]);
                this.Characters.Remove(characterId);
            }
        }
        
    }
}
