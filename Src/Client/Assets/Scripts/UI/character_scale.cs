using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class character_scale : UGUI_back
{
    [Header("角色")]
    public GameObject[] characters;

    [Header("角色选择脚本")]
    public MyUICharacterSelect MyUICharacterSelect;
    public int id;
    private float x;
    [Header("角色旋转速度")]
    public float V;
    private void Awake()
    {
        MyUICharacterSelect.now_selected_Char_action += selected_character;
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        x = Input.mousePosition.x;
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        float y= Input.mousePosition.x;
        float z = y - x;
        characters[id].transform.eulerAngles += Vector3.up * z * V;
        x = y;
    }
    public void selected_character(int x)
    {
        id = x-1;
        for(int i=0;i<3;i++)
        {
            characters[i].SetActive(i == id);
        }
    }
}
