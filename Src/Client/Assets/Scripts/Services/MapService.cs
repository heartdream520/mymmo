using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Common.Data;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        /// <summary>
        /// 当前地图ID
        /// </summary>
        public int CurrentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);

        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
            //遍历角色
            foreach (var cha in response.Characters)
            {
                //刷新本地数据确保安全
                if (User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    //当前角色切换地图
                    User.Instance.CurrentCharacter = cha;
                }
                //将此地图中所有角色加入角色管理器中
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)
            {
                //切换地图
                this.EnterMap(response.mapId);
                //更新地图id
                this.CurrentMapId = response.mapId;
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                MySceneManager.Instance.LoadScene(map.Resource);
                
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }
    }
}
