﻿using Common;
using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Character : CharacterBase ,IPostResponser
    {
       
        public TCharacter Data;
        public ItemManager itemManager;
        public StatusManager StatusManager;

        public QuestManager QuestManager;
        public FriendManager FriendManager;

        public Team team;
        //队伍更新时间戳
        public int TeamUpdateTS;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            this.Info.Level = cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Entity = this.EntityData;
            //装备信息
            this.Info.Equips = this.Data.Equips;

            this.Info.Gold = cha.Gold;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];


            //玩家道具初始化
            this.itemManager = new ItemManager(this);
            this.itemManager.GetItemInfos(this.Info.Items);

            //玩家背包初始化
            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Items = this.Data.Bag.Items;
            this.Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            this.StatusManager = new StatusManager(this);

            //玩家任务初始化
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);

            //玩家好友初始化
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetQuestInfos(this.Info.Friends);
        }

        internal NCharacterInfo GetBasicInfo()
        {
            var info = new NCharacterInfo();
            info.Type = this.Info.Type;
            info.Id = this.Info.Id;
            info.Name = this.Info.Name;
            info.Level = this.Info.Level;
            info.ConfigId = this.Info.ConfigId;
            info.Class = this.Info.Class;
            return info;

        }

        public override string ToString()
        {
            return string.Format("ID:{0} EID:{1}",Id,this.entityId);
        }
        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value) return;

                this.StatusManager.AddGoldChange((int)(value - this.Data.Gold));
                this.Data.Gold = value;
            }
        }

        public void Onlive()
        {

            this.FriendManager.UpdateFriendSelfInfo(this.Info, 1);
        }
        internal void Clear()
        {
            this.FriendManager.UpdateFriendSelfInfo(this.Info, 0);
            if (this.team != null)
                this.team.Leave(this);
        }

        public void PostProcess(NetMessageResponse message)
        {
            this.FriendManager.PostProcess(message);
            if(this.team!=null)
            {
                if(this.TeamUpdateTS<this.team.timeTS)
                {
                    Log.InfoFormat("Character->PostProcess  character:{0} UpdateTeam", this.ToString());
                    this.TeamUpdateTS = this.team.timeTS;
                    this.team.PostProcess(message);
                }
                
            }

            if (this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }
        }
    }
}
