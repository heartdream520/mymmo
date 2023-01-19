using UnityEngine;

class MessageBox
{
    static Object cacheObject = null;
    static GameObject go;
    public static UIMessageBox Show(string message, string title="", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        Debug.Log(message);
        if(cacheObject==null)
        {
            cacheObject = Resloader.Load<Object>("MyUI/UIMessageBox");
        }

        if(go!=null)
        {
            GameObject.Destroy(go);
        }
        go = (GameObject)GameObject.Instantiate(cacheObject);
        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();
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