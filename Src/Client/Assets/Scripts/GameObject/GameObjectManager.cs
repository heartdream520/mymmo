using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;
using System;

public class GameObjectManager : MonoBehaviour
{
    /// <summary>
    /// 所有角色字典
    /// </summary>
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    void Start()
    {
        //初始化所有游戏对象
        StartCoroutine(InitGameObjects());


        //创建鼠标管理器
        //CreatMouseManager();


        //设置事件
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //玩家进入事件执行
    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    private void CreatMouseManager()
    {
        Debug.LogError("生成鼠标管理器");
        Instantiate( Resloader.Load<GameObject>("Prefabs/MouseManager"));
    }

    IEnumerator InitGameObjects()
    {
        //启动创建玩家
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }
    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <param name="character">要创建的玩家</param>
    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)
        {
            //根据角色资源位置获取游戏对象资源实体
            UnityEngine.Object obj = Resloader.Load<UnityEngine.Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            //创建游戏对象
            GameObject go = (GameObject)Instantiate(obj);
            //为新创建游戏对象命名
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            //设置初始位置和朝向
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            //将角色加入字典
            Characters[character.Info.Id] = go;
            //获取实体控制器
            EntityController ec = go.GetComponent<EntityController>();
            if (ec != null)
            {
                ec.entity = character;
                ec.isPlayer = character.IsPlayer;
            }
            //获取玩家控制器
            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null)
            {
                //当前角色为 本机进游戏的角色 将玩家控制器设为可用，否则设为不可用
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
                {
                    Debug.LogFormat("Player_character:{0}", Models.User.Instance.CurrentCharacter.Id);
                    MainPlayerCamera.Instance.player = go;
                    pc.enabled = true;
                    pc.character = character;

                    pc.entityController = ec;

                    //设置当前控制角色的游戏对象
                    User.Instance.CurrentCharacterObject = go;
                }
                else
                {
                    pc.enabled = false;
                }
            }
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
    }
}

