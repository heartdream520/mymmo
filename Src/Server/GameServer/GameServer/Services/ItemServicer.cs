using Common;
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
    class ItemServicer :Singleton<ItemServicer>
    {
        public ItemServicer()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);

        }



        public void Init()
        {

        }
        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("ItemServicer->OnItemBuy Character:{0} ShopID:{1} ShopItemID:{2}",
                character.ToString(),message.shopId,message.shopItemId);
            var result = ShopManager.Instance.BuyItem(sender, message.shopId, message.shopItemId);
            sender.Session.Respose.itemBuy = new ItemBuyResponse();
            sender.Session.Respose.itemBuy.Result = result;
            sender.SendResponse();

        }
        private void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("ItemServicer->OnItemEquip Character:{0} slot:{1} ItemId:{2} isEquip:{3}",
                character.ToString(), message.Slot, message.itemId,message.isEquip);
            var result = EquipManager.Instance.EquipItem(sender, message.Slot, message.itemId,message.isEquip);
            sender.Session.Respose.itemEquip = new ItemEquipRespose();
            sender.Session.Respose.itemEquip.Result = result;
            sender.SendResponse();
        }
    }
}
