using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Models
{
    class User : Singleton<User>
    {
        /// <summary>
        /// user信息
        /// </summary>
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }

        /// <summary>
        /// 设置user信息
        /// </summary>
        /// <param name="info">user信息</param>
        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            if(info==null)
            {
                MessageBox.Show("info 为空");
            }
            //MessageBox.Show(info.ToString());
            this.userInfo = info;
        }

        /// <summary>
        /// 当前选择的角色信息
        /// </summary>
        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        

        /// <summary>
        /// 当前地图的数据
        /// </summary>
        public MapDefine CurrentMapData;
        private GameObject currentCharacterobject;
        public UnityAction<GameObject> CurrentCharacterObject_Set_Action;
        public GameObject CurrentCharacterObject
        {
            get { return currentCharacterobject; }
            set
            {
                currentCharacterobject = value;
                CurrentCharacterObject_Set_Action(value);
            }
        }
    }
}
