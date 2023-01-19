using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class FriendManager:Singleton<FriendManager>
    {
        public List<NFriendInfo> allFriends;
        public void Init(List<NFriendInfo> friends)
        {
            this.allFriends = friends;
        }
    }
}
