using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Services;

namespace Assets.Scripts.Managers
{
    class GulidManager : Singleton<GulidManager>
    {
        private NGulidInfo gulid_info;
        public NGulidInfo Gulid_Info
        {
            get
            {
                return this.gulid_info;
            }
            set
            {
                this.gulid_info = value;
                if (value == null)
                {
                    this.mumber_info = null;
                }
                else
                    foreach (var m in this.gulid_info.Members)
                    {
                        if(User.Instance.CurrentCharacter!=null)
                        {
                            if (m.characterId == User.Instance.CurrentCharacter.Id)
                                this.mumber_info = m;
                        }
                        
                    }
                if(OnNGulidInfoAction!=null)
                OnNGulidInfoAction(value);
            }
        }
        public UnityAction<NGulidInfo> OnNGulidInfoAction;
        public NGulidMemberInfo mumber_info;


        /// <summary>
        /// 所有公会信息
        /// </summary>
        private List<NGulidInfo> gulidlist;
        public List<NGulidInfo> GulidList
        {
            get { return this.gulidlist; }
            set
            {
                this.gulidlist = value;
                if (this.OnGulidListAction != null)
                    this.OnGulidListAction();
            }
        }
        public UnityAction OnGulidListAction;


        public void Init(NGulidInfo info)
        {
            SetGulidInfo(info);
            GulidService.Instance.SendGulidList();
        }
        public GulidManager()
        {

            GulidService.Instance.OnGulidInfoAction += this.SetGulidInfo;
            GulidService.Instance.OnGulidListAction += this.SetGulidInfos;
        }

        private void SetGulidInfos(List<NGulidInfo> arg0)
        {
            this.GulidList = arg0;
        }

        private void SetGulidInfo(NGulidInfo info)
        {
            this.Gulid_Info = info;
        }
        public bool has_Gulid()
        {
            if (this.gulid_info == null) return false;
            return true;
        }
        internal void ShowGulid()
        {
            if(this.gulid_info!=null)
            {
                UIManager.Instance.Show<UIGulid>();
            }
            else
            {
                var x= UIManager.Instance.Show<UINoGulid>();
                x.Onclose += this.OnNoGulidClose;
            }
        }

        private void OnNoGulidClose(UIWindow sender, UIWindow.WindowResult result)
        {
            if(result==UIWindow.WindowResult.Yes)
            {
                UIManager.Instance.Show<UICreatGulid>();
            }
            else if(result== UIWindow.WindowResult.No)
            {
                UIManager.Instance.Show<UIGulidList>();
            }
        }
    }
}
