using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;
        
        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return minimapBoundingBox; }
        }
        public Transform PlayerTransform
        {
            get
            {
                if(User.Instance.CurrentCharacterObject==null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMinimap()
        {
            Debug.LogFormat("Load MiniMap Image :{0}", User.Instance.CurrentMapData.MiniMap);
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }
        public void UpdataMiniMap(Collider minimapBoundingBox)
        {
            Debug.LogFormat("MinimapManager->UpdataMiniMap");
            if(!minimapBoundingBox) Debug.LogError("传过来的地图盒子为空");
            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap != null)
                this.minimap.UpdataMap();
            else Debug.LogError("MinimapManager->UpdataMiniMap miniMap == null");
        }
    }
}
