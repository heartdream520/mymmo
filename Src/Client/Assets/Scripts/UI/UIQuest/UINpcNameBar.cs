using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINpcNameBar : MonoBehaviour {

    public Text NpcName;
    private NpcDefine define;
    public NpcDefine NpcDefine
    {
        get { return define; }
        set
        {
            this.define = value;
            InitItself();
        }
    }

    private void InitItself()
    {
        this.NpcName.text = define.Name;
    }
}
