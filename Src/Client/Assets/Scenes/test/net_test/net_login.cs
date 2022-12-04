using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class net_login : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        Network.NetClient.Instance.Init("127.0.0.1", 8000);
        Network.NetClient.Instance.Connect();


        //封装的消息类
        SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();
        msg.Request = new SkillBridge.Message.NetMessageRequest();
        msg.Request.firstRequest = new SkillBridge.Message.FirstTestRequest();

        SkillBridge.Message.FirstTestRequest firstTestRequest = new SkillBridge.Message.FirstTestRequest();
        firstTestRequest.Helloword = "hello word";
        
        msg.Request.firstRequest = firstTestRequest;
        //发送消息
        Network.NetClient.Instance.SendMessage(msg);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
