using UnityEngine;

class MessageBox
{
    static Object cacheObject = null;
    static GameObject go;
    /// <summary>
    /// 创建消息提示
    /// </summary>
    /// <param name="message">提示的消息</param>
    /// <param name="title">消息提示标题</param>
    /// <param name="type">消息提示类型</param>
    /// <param name="btnOK"></param>
    /// <param name="btnCancel"></param>
    /// <returns></returns>
    public static UIMessageBox Show(string message, string title="", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        Debug.Log(message);
        if(cacheObject==null)
        {
            //加载消息提示面板
            cacheObject = Resloader.Load<Object>("MyUI/UIMessageBox");
        }

        if(go!=null)
        {
            GameObject.Destroy(go);
        }
        //新建消息提示面板
        go = (GameObject)GameObject.Instantiate(cacheObject);
        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();
        //初始化消息提示面板
        msgbox.Init(title, message, type, btnOK, btnCancel);
        return msgbox;
    }
}

public enum MessageBoxType
{
    /// <summary>
    /// Information Dialog with OK button
    /// </summary>
    Information = 1,

    /// <summary>
    /// Confirm Dialog whit OK and Cancel buttons
    /// </summary>
    Confirm = 2,

    /// <summary>
    /// Error Dialog with OK buttons
    /// </summary>
    Error = 3
}