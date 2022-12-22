using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoSingleton<UIMinimap> {

    [Header("地图的碰撞盒")]
    public Collider minimapBoundingBox;
    [Header("箭头")]
    public Image arrow;
    [Header("箭头名字text")]
    public Text mapName;
    [Header("小地图图片")]
    public Image minimap;

    private Transform playerTransform;
    public override void OnAwake()
    {
        MinimapManager.Instance.minimap = this;
        
         
    }
    // Use this for initialization
    public override void OnStart()
    {
        this.UpdataMap();
    }

    public void UpdataMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        //更新小地图图片
        this.minimap.sprite = MinimapManager.Instance.LoadCurrentMinimap();
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

        //设置初始大小
        this.minimap.SetNativeSize();
        //设置本地位置
        this.minimap.transform.localPosition = Vector3.zero;
        this.minimapBoundingBox =MinimapManager.Instance.MinimapBoundingBox;
        playerTransform = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerTransform == null)
            playerTransform = MinimapManager.Instance.PlayerTransform;
        if (playerTransform == null)
        {
            return;
        }
        if (!minimapBoundingBox)
        {
            minimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;
        }
        //地图实际大小
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        //玩家相对地图左下角的位置
        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        //计算新的地图的中心点
        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;


        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
	}
}
