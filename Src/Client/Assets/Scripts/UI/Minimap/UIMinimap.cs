using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour {

    [Header("地图的碰撞盒")]
    public Collider minimapBoundingBox;
    [Header("箭头")]
    public Image arrow;
    [Header("箭头名字text")]
    public Text mapName;
    [Header("小地图图片")]
    public Image minimap;

    private Transform playerTransform;
	// Use this for initialization
	void Start () {
        this.InitMap();
    }

    void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        //if (this.minimap.overrideSprite == null)
        //更新小地图图片
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

        //设置初始大小
        this.minimap.SetNativeSize();
        //设置本地位置
        this.minimap.transform.localPosition = Vector3.zero;
        //设置玩家位置
        this.playerTransform = User.Instance.CurrentCharacterObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
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
