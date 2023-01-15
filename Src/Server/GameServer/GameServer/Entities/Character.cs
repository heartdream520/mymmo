using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Character : CharacterBase
    {
       
        public TCharacter Data;
        public ItemManager itemManager;
        public StatusManager StatusManager;

        public QuestManager QuestManager;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.Tid = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Entity = this.EntityData;
            //装备信息
            this.Info.Equips = this.Data.Equips;

            this.Info.Gold = cha.Gold;
            this.Define = DataManager.Instance.Characters[this.Info.Tid];


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
        }
        public override string ToString()
        {
            return string.Format("ID:{0}  DID:{1} EID:{2}",Id,this.Data.ID,this.entityId);
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
    }
}
