using Common;
using Common.Data;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public Result BuyItem(NetConnection<NetSession>sender,int shopId,int shopItemId)
        {
            if (!DataManager.Instance.Shops.ContainsKey(shopId))
            {
                Log.ErrorFormat("ShopManager->BuyItem:ShopId:{0} not exist", shopId);
                return Result.Failed;
            }
                
            ShopItemDefine shopItem;
            if(DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId,out shopItem))
            {

                Log.InfoFormat("ShopManager->BuyItem BuyCharacter:{0} ShopId:{1} ShopItemId:{2} ",
                    sender.Session.Character.ToString(), shopId, shopItemId);
                Log.InfoFormat("ShopManager->BuyItem CharacterGold:{0},Price:{1}", sender.Session.Character.Gold, shopItem.Price * shopItem.Count);
                if (sender.Session.Character.Gold>=shopItem.Price*shopItem.Count)
                {
                   
                    sender.Session.Character.itemManager.AddItem(shopItem.ItemID, shopItem.Count);
                    sender.Session.Character.Gold -= shopItem.Price * shopItem.Count;
                    DBService.Instance.Save();
                    return Result.Success;

                }
            }
            else
            {
                Log.ErrorFormat("ShopManager->BuyItem:ShopId:{0} not exist ShopItemId:{1}", shopId,shopItem);

            }
            return Result.Failed;
        }
    }
}
