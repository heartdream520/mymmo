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
    class BagService : Singleton<BagService>
    {
        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(this.OnBagSave);

        }

       

        public void Init()
        {

        }
        private void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("BagService->OnBagSave Character:{0},UnlLocked:{1}", character.ToString(), request.BagInfo.Unlocked);
            if (request.BagInfo != null)
            {
                character.Data.Bag.Items = request.BagInfo.Items;
                DBService.Instance.Save();
            }
            else
            {
                Log.ErrorFormat("BagService->OnBagSave  Request BagInfo is null");

            }
            
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.Bagsave = new BagSaveRespose();
            message.Response.Bagsave.Result = Result.Success;
            message.Response.Bagsave.Errormsg = "None";
            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }
    }
}