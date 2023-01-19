using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Entities;
using Services;
using SkillBridge.Message;
using Models;
using System;

public class GameObjectManager :MonoSingleton<GameObjectManager>
{
    /// <summary>
    /// 所有角色字典
    /// </summary>
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    public override void OnStart()
    {
        //初始化所有游戏对象
        
        StartCoroutine(InitGameObjects());


        //创建鼠标管理器
        //CreatMouseManager();


        //设置事件
        
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        

        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    // Update is called once per frame
    void Update()
    {
    }
    //玩家进入事件执行
    void OnCharacterEnter(Character cha)
    {
        StartCoroutine(CreateCharacterObject(cha));
    }
    void OnCharacterLeave(Character cha)
    {
        DestroyCharacterObject(cha);
    }

    private void DestroyCharacterObject(Character cha)
    {
        Debug.LogFormat("GameObjectManager->DestroyCharacterObject CharacterId:{0} CharacterName:{1} CharacterEntityId:{2}",
            cha.Info.EnityId,cha.Info.Name,cha.entityId);
        if(this.Characters.ContainsKey(cha.Info.EnityId))
        {
            GameObject cha_game = Characters[cha.Info.EnityId];
            /*
            UIWorldElementManager.Instance.RemoveCharacterNameBar(cha_game.transform);
            Destroy(cha_game);
            */
            this.Characters.Remove(cha.Info.EnityId);
        }
    }

    private void CreatMouseManager()
    {
        Instantiate( Resloader.Load<GameObject>("Prefabs/MouseManager"));
    }
    
    IEnumerator InitGameObjects()
    {
        //Debug.LogError("初始化游戏对象！");
        //启动创建玩家
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            StartCoroutine(CreateCharacterObject(cha));
            yield return null;
        }
    }
    /// <summary>
    /// 创建玩家
    /// </summary>
    /// <param name="character">要创建的玩家</param>
    IEnumerator CreateCharacterObject(Character character)
    {
        while (SceneManager.GetActiveScene().name == "Load_Scenes" || SceneManager.GetActiveScene().name == "CharSelect")
        {
            yield return null;
        }
        while (SceneManager.GetActiveScene().name != MapService.Instance.new_scene_name)
        {
            yield return null;
        }
        if (!Characters.ContainsKey(character.Info.EnityId) || Characters[character.Info.EnityId] == null)
        {
            Debug.LogFormat("GameObjectManager->CreateCharacterObject CharacterId:{0} CharacterName:{1} CharacterEntityId:{2}",
            character.Info.EnityId, character.Info.Name, character.entityId);
            //根据角色资源位置获取游戏对象资源实体
            UnityEngine.Object obj = Resloader.Load<UnityEngine.Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                yield break;
            }
            //创建游戏对象
            GameObject go = (GameObject)Instantiate(obj);
            
            //为新创建游戏对象命名
            go.name = "Character_" + character.Info.EnityId + "_" + character.Info.Name;
            go.transform.SetParent(transform, false);
            //将角色加入字典
            Characters[character.Info.EnityId] = go;
            this.InitGameObject(Characters[character.entityId],character);
            
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
    }

    private void InitGameObject(GameObject go, Character character)
    {
        //设置初始位置和朝向
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        //获取实体控制器
        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsCurrentPlayerPlayer;
        }
        //获取玩家控制器
        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            //当前角色为 本机进游戏的角色 将玩家控制器设为可用，否则设为不可用
            if (character.IsCurrentPlayerPlayer)
            {

                //设置当前控制角色的游戏对象
                User.Instance.CurrentCharacterObject = go;
                Debug.LogFormat("GameObjectManager->InitGameObject Player_character:{0}", Models.User.Instance.CurrentCharacter.EnityId);
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;

                pc.entityController = ec;

                
                
            }
            else
            {
                pc.enabled = false;
            }
        }
    }

}

