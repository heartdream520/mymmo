
using UnityEngine;

class InputBox
{
    static Object cacheObject = null;
    static GameObject go;
    public static UIInputBox Show(string message, string title = "", string btnOK = "", string btnCancel = "",string tips="")
    {
        Debug.Log(message);
        if (cacheObject == null)
        {
            cacheObject = Resloader.Load<Object>("MyUI/UIInputBox");
        }

        if (go != null)
        {
            GameObject.Destroy(go);
        }
        go = (GameObject)GameObject.Instantiate(cacheObject);
        UIInputBox msgbox = go.GetComponent<UIInputBox>();
        msgbox.Init(title, message, btnOK, btnCancel,tips);
        return msgbox;
    }
}