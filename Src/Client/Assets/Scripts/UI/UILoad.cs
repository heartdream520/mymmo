using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UILoad : MonoBehaviour
{

    public InputField username;
    public InputField password;

    public Toggle remember_user_Toggle;
    public Toggle use_Agreement_Toggle;


    // Use this for initialization
    void Start()
    {
        UserService.Instance.OnLoad += this.OnLoad;

    }
    private void OnEnable()
    {
        username.text = "";
        password.text = "";
    }
    void OnLoad(SkillBridge.Message.Result result, string msg)
    {

        if (msg == "用户不存在.")
        {
            MessageBox.Show("用户不存在！");
            return;
        }
        if (msg == "密码错误.")
        {
            MessageBox.Show("密码错误！");
            return;
        }
        if (result == SkillBridge.Message.Result.Success)
        {
            //MessageBox.Show("登录成功！");
            MySceneManager.Instance.LoadScene("CharSelect");
            return;
        }
        MessageBox.Show(string.Format("结果：{0} msg:{1}", result, msg));
    }
    void Update()
    {

    }

    public void OnClickLoad()
    {
        if(!use_Agreement_Toggle.isOn)
        {
            MessageBox.Show("请同意用户协议");
            return;
        }
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
        UserService.Instance.SendLoad(this.username.text, this.password.text);
    }

}
