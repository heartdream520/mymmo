using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    class TeamManager : Singleton<TeamManager>
    {
        public void Init()
        {

        }
        internal void UpdateTeamInfo(NteamInfo team)
        {
            User.Instance.TeamInfo = team;
            this.ShowTeamUi(team != null);
        }

        private void ShowTeamUi(bool show)
        {
            if(UIMain.Instance!=null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }
    }
}
