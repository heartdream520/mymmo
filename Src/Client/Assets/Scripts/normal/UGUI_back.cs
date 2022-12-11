using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

//句柄
public class UGUI_back : MonoBehaviour,
IPointerEnterHandler, IPointerExitHandler,//进入，离开
IPointerDownHandler, IPointerUpHandler,//按下抬起
IPointerClickHandler,//点击
IBeginDragHandler, IDragHandler, IEndDragHandler//拖拽
{
    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
    }
    /// <summary>
    /// 鼠标退出
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
    }
    /// <summary>
    /// 鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Down");
    }
    /// <summary>
    /// 鼠标抬起
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
       // Debug.Log("Up");
    }
    /// <summary>
    /// 鼠标点击
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");
    }
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag");
    }
    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
    }
}
