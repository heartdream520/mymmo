using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Models;
using Assets.Scripts.Managers;

namespace Entities
{
    public class Character : Entity 
    {
        /// <summary>
        /// 用于通信的角色定义
        /// </summary>
        public NCharacterInfo Info;

        /// <summary>
        /// 从配置表中读入的角色配置
        /// </summary>
        public Common.Data.CharacterDefine Define;

        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }

        public bool IsPlayer
        {
            get { return this.Info.Id == User.Instance.CurrentCharacter.Id; }
        }

        public Character(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.Tid];
            
        }

        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed;
        }

        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPosition:{0}", position);
            this.position = position;
        }

        public void OnEntityRemoved()
        {
            Debug.LogFormat("Character OnEntityRemoved()  EntityId :{0}",this.entityId);
        }
    }
}
