using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;
namespace Assets.Scripts.Service
{
    class BagService : Singleton<BagService>, IDisposable
    {
        public BagService()
        {
            MessageDistributer.Instance.Subscribe<BagSaveRespose>(this.OnBagSave);
        }

        
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<BagSaveRespose>(this.OnBagSave);
        }
        public void Init()
        {

        }

        public void SendBagSave(NBagInfo info)
        {
            Debug.LogFormat("BagService->SendBagSave");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Bagsave = new BagSaveRequest();
            message.Request.Bagsave.BagInfo = info;
            NetClient.Instance.SendMessage(message);
        }
        private void OnBagSave(object sender, BagSaveRespose message)
        {
            Debug.Log("BagService->OnBagSave");
        }

    }
}
