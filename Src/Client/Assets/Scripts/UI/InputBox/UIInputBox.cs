using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInputBox : MonoBehaviour
{
    public Text title;
    public Text message;
    public Button buttonYes;
    public Button buttonNo;
    public Button buttonClose;

    public Text buttonYesTitle;
    public Text buttonNoTitle;
    public Text tips;

    public UnityAction OnYes;
    public UnityAction OnNo;
    public GameObject Panel;
    public delegate bool SumbitHandler(string inputText, out string tips);
    public event SumbitHandler Onsumbit;
    private string emptyTips;
    public InputField InputField;
    public float v;

    // Use this for initialization
    void Start()
    {
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
    public void Init(string title, string message, string btnOK = "", string btnCancel = "",string tips="")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = btnOK;

        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = btnCancel;
        else this.buttonNoTitle.transform.parent.gameObject.SetActive(false);

        this.tips.text = tips;
        this.emptyTips = tips;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);
        
    }

    public void OnClickYes()
    {

        this.tips.text = "";
        if (string.IsNullOrEmpty(this.InputField.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }
        if(this.Onsumbit!=null)
        {
            string tips;
            if(! this.Onsumbit(this.InputField.text,out tips))
            {
                this.tips.text = tips;
                return;
            }
        }

        if (this.OnYes != null)
            this.OnYes();
        Destroy(this.gameObject);
    }

    public void OnClickNo()
    {

        if (this.OnNo != null)
            this.OnNo();
        Destroy(this.gameObject);
    }
}
