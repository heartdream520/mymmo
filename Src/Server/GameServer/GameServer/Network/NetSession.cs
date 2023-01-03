using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer;
using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;

namespace Network
{
    class NetSession :INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }

        

        internal void Disconnected()
        {
            UserService.Instance.CharacterLeave(Character);
        }
        NetMessage response;
        public NetMessageResponse Respose
        {
            get
            {
                if(response==null)
                {
                    response = new NetMessage();
                }
                if (response.Response == null)
                    response.Response = new NetMessageResponse();
                return response.Response;
            }
        }
        public byte[] GetResponse()
        {
            Log.InfoFormat("NetSession->GetResponse");
            if(response!=null)
            {
                if(this.Character!=null&&this.Character.StatusManager.HasStatus)
                {
                    this.Character.StatusManager.PostProcess(Respose);
                }
                byte[] data = PackageHandler.PackMessage(response);
                response = null;
                return data;
            }
            else
            {
                Log.ErrorFormat("NetSession->GetResponse response IS NULL");
            }
            return null;
        }
    }
}
