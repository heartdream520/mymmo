using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {


    [Header("地图包围盒")]
    public Collider MinimapBoundingBox;

    void Start () {
        MinimapManager.Instance.UpdataMiniMap(this.MinimapBoundingBox);

    }
	
}
