using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using SkillBridge.Message;
using Models;
using Assets.Scripts.Managers;
using Assets.Scripts.Services;


public class UICreatGulid : UIWindow {

    public InputField name;
    public InputField notic;
    private void Start()
    {
        GulidService.Instance.OnGulidCreatAction += this.OnCreat;
    }
    private void OnDestroy()
    {
        GulidService.Instance.OnGulidCreatAction -= this.OnCreat;
    }
    public void OnchickCreatGulidButton()
    {
        if(string.IsNullOrEmpty(name.text))
        {
            MessageBox.Show("请输入公会名称","公会");
            return;
        }
        if(string.IsNullOrEmpty(notic.text))
        {
            MessageBox.Show("请输入公会公告","公会");
            return;
        }
        if(name.text.Length<2|| name.text.Length > 5)
        {
            MessageBox.Show("公会名应为2-5个汉字","公会");
            return;
        }
        if(notic.text.Length<3|| notic.text.Length > 50)
        {
            MessageBox.Show("公会宗旨应为3-50个汉字","公会");
            return;
        }
        GulidService.Instance.SendGulidCreat(this.name.text, this.notic.text);
    }
    private void OnCreat(Result result)
    {
        if(result==Result.Success)
        {
            this.OnClick_Close();
        }
    }

}
