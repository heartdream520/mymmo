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

namespace Assets.Scripts.Services
{
    class StatusServicer : Singleton<StatusServicer> ,IDisposable
    {

        public void Init() { }
        public StatusServicer()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }
       

        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();

        public void RegisterStatusNofity(StatusType type, StatusNotifyHandler action)
        {
            if (!eventMap.ContainsKey(type))
                eventMap[type] = action;
            else eventMap[type] += action;
        }
        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            Debug.LogFormat("StatusServicer->OnStatusNotify");
            foreach (var status in notify.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusServicer->Notify Type:{0} Action:{1} Id:{2} Value:{3}",
                status.Type,status.Action,status.Id,status.Value);
            if(status.Type==StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    User.Instance.AddGold(status.Value);
                else User.Instance.AddGold(-status.Value);
                return;
            }
            StatusNotifyHandler handler;
            if(eventMap.TryGetValue(status.Type,out handler))
            {
                handler(status);
            }
        }
    }
}
