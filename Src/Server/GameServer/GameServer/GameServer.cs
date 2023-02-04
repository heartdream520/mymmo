using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;
using GameServer.Managers;

namespace GameServer
{
    class GameServer
    {
        Thread thread;
        bool running = false;
        //网络服务
        NetService network;
        public bool Init()
        {
            network = new NetService();
            network.Init(8000);

            HelloWorldService.Instance.Init();
            DataManager.Instance.Load();
            DBService.Instance.Init();
            UserService.Instance.Init();

            MapService.Instance.Init();

            BagService.Instance.Init();

            ItemServicer.Instance.Init();
            QuestService.Instance.Init();
            FriendService.Instance.Init();

            TeamService.Instance.Init();
            GulidService.Instance.Init();

            thread = new Thread(new ThreadStart(this.Update));
            //StatusManager.thread = new Thread(new ThreadStart(this.Update));
            return true;
        }

        public void Start()
        {
            HelloWorldService.Instance.Start();

            network.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            running = false;
            thread.Join();

            network.Stop();
        }

        public void Update()
        {
            while (running)
            {
                Time.Tick();
                //100ms一帧
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
                MapManager.Instance.Update();
            }
        }
    }
}
