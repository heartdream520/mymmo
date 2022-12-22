using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Assets.Scripts.Managers;

public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    public Camera camera;
    public Transform viewPoint;

    public GameObject player;
    public Vector3 mouse_pos;
    [Header("旋转速度")]
    public float rotate_v;
    [Header("摄像机最小旋转x")]
    public float min_rotate_x;
    [Header("摄像机最大旋转x")]
    public float max_rotate_x;

    public AnimationCurve camera_curve;

    // Use this for initialization
    public override void OnStart () {
        mouse_pos = Input.mousePosition;
	}

    /// <summary>
    /// 左右移动角度
    /// </summary>
    public float x;
    /// <summary>
    /// 上下移动角度
    /// </summary>
    public float y;

    

    // Update is called once per frame
    void Update () {

        updata_Rotation();
        updata_camera_curve();
    }
    private void updata_Rotation()
    {
        if (MouseManager.Instance.Mouse_Is_Display) return;
        x += Input.GetAxis("Mouse X")*rotate_v;
        //鼠标向上应该减少角度
        y -= Input.GetAxis("Mouse Y")*rotate_v;
        y = Mathf.Clamp(y, min_rotate_x, max_rotate_x);

        transform.rotation = Quaternion.Euler(y, x, 0);
    }
    private void updata_camera_curve()
    {
        camera.fieldOfView = camera_curve.Evaluate(y);
    }



    private void LateUpdate()
    {
        if (player == null)
        {
            if (User.Instance.CurrentCharacterObject)
            {
                player = User.Instance.CurrentCharacterObject;
            }
            else
                return;
        }
            

        this.transform.position = player.transform.position;
        //this.transform.rotation = player.transform.rotation;
        
    }
}
