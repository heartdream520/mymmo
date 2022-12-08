using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMessageBox :MonoBehaviour
{ 
    public Text title;
    public Text message;
    public Image[] icons;
    public Button buttonYes;
    public Button buttonNo;
    public Button buttonClose;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public UnityAction OnYes;
    public UnityAction OnNo;
    public GameObject Panel;

    public float v;

    // Use this for initialization
    void Start () {
        Panel.transform.localScale = Vector3.zero;
        StartCoroutine("appear");
	}
    IEnumerator appear()
    {
        float x = Panel.transform.localScale.x;
        while (x < 1.0)
        {
            x = Mathf.Lerp(x, 2f, v * Time.deltaTime);
            x = Mathf.Min(x, 1f);
            Panel.transform.localScale = new Vector3(x, x, x);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title">提示标题</param>
    /// <param name="message">提示信息</param>
    /// <param name="type">提示类型</param>
    /// <param name="btnOK"></param>
    /// <param name="btnCancel"></param>
    public void Init(string title, string message, MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.icons[0].enabled = type == MessageBoxType.Information;
        this.icons[1].enabled = type == MessageBoxType.Confirm;
        this.icons[2].enabled = type == MessageBoxType.Error;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = title;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = title;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);

        this.buttonNo.gameObject.SetActive(type == MessageBoxType.Confirm);
    }

    public void OnClickYes()
    {
        Destroy(this.gameObject);
        if (this.OnYes != null)
            this.OnYes();
    }

    public void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnNo != null)
            this.OnNo();
    }
}
