using Assets.Scripts.Managers;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
public class UIWorldElementManager : MonoSingleton<UIWorldElementManager> {

    public GameObject nameBarPrefab;
    public GameObject NpcStatusPrefab;
    public GameObject NpcNameBarPrefab;

    private Dictionary<Transform, GameObject> charNameBarElements = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> npcNameBarElements = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> npcStatusElements= new Dictionary<Transform, GameObject>();

    // Use this for initialization
    public override void OnStart () {
		
	}
	


    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
        if (!character.IsPlayer) return;
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.charNameBarElements[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.charNameBarElements.ContainsKey(owner))
        {
            Destroy(this.charNameBarElements[owner]);
            this.charNameBarElements.Remove(owner);
        }
    }
    public void AddNpcNameBar(Transform owner, NpcDefine define)
    {
        GameObject goNameBar = Instantiate(NpcNameBarPrefab, this.transform);
        goNameBar.name = "NpcNameBar " + define.Name;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINpcNameBar>().NpcDefine = define;
        goNameBar.SetActive(true);
        this.npcNameBarElements[owner] = goNameBar;
    }

    public void RemoveNpcNameBar(Transform owner)
    {
        if (this.npcNameBarElements.ContainsKey(owner))
        {
            Destroy(this.npcNameBarElements[owner]);
            this.npcNameBarElements.Remove(owner);
        }
    }

    internal void AddNpcQuestStatus(Transform owner, NpcQuestStatus questStatus)
    {
        if(this.npcStatusElements.ContainsKey(owner))
        {
            npcStatusElements[owner].GetComponent<UIQuestStatus>().SetQuestStatus(questStatus);
        }
        else
        {
            GameObject go = GameObject.Instantiate(NpcStatusPrefab, owner, false);
            go.name = owner.name + "NpcStatus";
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(questStatus);
            go.SetActive(true);
            this.npcStatusElements[owner] = go;
        }
    }

    internal void RemoveNpcQuestStatus(Transform owner)
    {
        if (this.npcStatusElements.ContainsKey(owner))
        {
            Destroy(this.npcStatusElements[owner]);
            this.npcStatusElements.Remove(owner);
        }
    }
}
