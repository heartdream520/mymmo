using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using System;

public class NpcManager :Singleton<NpcManager>{


    public delegate bool NpcActionHandler(NpcDefine npcDefine);
    /// <summary>
    /// npc类型事件字典
    /// </summary>
    Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>()
;
    /// <summary>
    /// 注册某种类型Npc的事件
    /// </summary>
    /// <param name="npcFunction">npc类型</param>
    /// <param name="npcActionHandler">事件</param>
    public void RegisterNpcEnvent(NpcFunction npcFunction, NpcActionHandler npcActionHandler)
    {
        if(!eventMap.ContainsKey(npcFunction))
        {
            eventMap[npcFunction] = npcActionHandler;
        }
        else eventMap[npcFunction] += npcActionHandler;
    }
    /// <summary>
    /// 获取NPC  Define
    /// </summary>
    /// <param name="npcid">NPCid</param>
    /// <returns>NpcDefine</returns>
    public NpcDefine GetNpcDefine(int npcid)
    {
        NpcDefine npc = null;
        DataManager.Instance.Npcs.TryGetValue(npcid, out npc);
        return npc;
    }

    /// <summary>
    /// NPC功能
    /// </summary>
    /// <param name="npcID"></param>
    /// <returns></returns>
    public bool Interactive(int npcID)
    {
        if(DataManager.Instance.Npcs.ContainsKey(npcID))
        {
            NpcDefine npc = DataManager.Instance.Npcs[npcID];
            return Interactive(npc);
        }
        return false;
    }
    public bool Interactive(NpcDefine npc)
    {
        if (npc.Type == NpcType.Task)
        {
            return DoTaskInteractive(npc);
        }
        else if(npc.Type == NpcType.Functional)
        {
            return DoFunctionalInteractive(npc);
        }
        return false;
    }

   
    /// <summary>
    /// 任务NPC
    /// </summary>
    private bool DoTaskInteractive(NpcDefine npc)
    {
        MessageBox.Show("点击了任务NPC"+npc.Name,"Npc对话");
        return true;
    }
    /// <summary>
    /// 功能NPC
    /// </summary>
    private bool DoFunctionalInteractive(NpcDefine npc)
    {
        if (npc.Type != NpcType.Functional)
            return false;
        if (!eventMap.ContainsKey(npc.Function))
            return false;
        return eventMap[npc.Function](npc);
    }
}
