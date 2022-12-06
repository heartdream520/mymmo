using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UIRegister : MonoBehaviour {

    public InputField username;
    public InputField password;
    public InputField passwordConfirm;


    // Use this for initialization
    void Start () {
        
        UserService.Instance.OnRegister += this.OnRegister;
       
    }
    private void OnEnable()
    {
        username.text = "";
        password.text = "";
        passwordConfirm.text = "";
    }

    void OnRegister(SkillBridge.Message.Result result, string msg)
    {
        if(msg=="用户已存在.")
        {
            MessageBox.Show("用户已存在！");
            return;
        }
        if (result==SkillBridge.Message.Result.Success)
        {
            MessageBox.Show("注册成功！");
            return;
        }
        MessageBox.Show(string.Format("结果：{0} msg:{1}",result,msg));
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        if (string.IsNullOrEmpty(this.passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        UserService.Instance.SendRegister(this.username.text, this.password.text);
    }
}
