using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class HelloWorldService:Singleton<HelloWorldService>
    {
        public void Init()
        {

        }
        public void Start()
        {

            //使用方法  OnFirstTestRequest  处理 消息 FirstTestRequest
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }
        //处理消息FirstTestRequest的方法
        public void OnFirstTestRequest(NetConnection<NetSession>sender,FirstTestRequest request)
        {
            Log.InfoFormat("OnFirstTestRequest: HelloWorld:{0}", request.Helloword);
        }

    }
}
