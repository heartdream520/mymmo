using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager :Singleton<TestManager> {

    public void Init()
    {
        NpcManager.Instance.RegisterNpcEnvent(NpcFunction.InvokeShop, OnNpcInvokeShop);
        NpcManager.Instance.RegisterNpcEnvent(NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
    }
    private bool OnNpcInvokeShop(NpcDefine npc)
    {
        Debug.LogFormat("TestManager->OnNpcInvokeShop :NPC:[ID:{0}: Name:{1}] Type:{2} Function:{3} Param:{4}",
            npc.ID, npc.Name, npc.Type, npc.Function, npc.Param);
        UIMain.Instance.onchick_test_button("点击商店NPC"+npc.Name);
        return true;
    }
    private bool OnNpcInvokeInsrance(NpcDefine npc)
    {
        Debug.LogFormat("TestManager->OnNpcInvokeInsrance :NPC:[ID:{0}: Name:{1}] Type:{2} Function:{3} Param:{4}",
            npc.ID, npc.Name, npc.Type, npc.Function, npc.Param);
        MessageBox.Show("点击副本NPC"+npc.Name);
        return true;
    }

}
