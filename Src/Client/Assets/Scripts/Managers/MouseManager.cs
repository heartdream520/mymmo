using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Managers
{

    public class MouseManager : MonoSingleton<MouseManager>
    {

        public UnityAction<bool> Mouse_DisPlay_Action;
        private bool mouse_is_diplay;
        public bool Mouse_Is_Display
        {
            get { return mouse_is_diplay; }
            set
            {
                mouse_is_diplay = value;
                if (Mouse_DisPlay_Action != null)
                    Mouse_DisPlay_Action(value);
                if(value)
                {
                    ToShowCursor();
                }
                else
                {
                    ToHideCursor();
                }
            }
        }
        private void Update()
        {
            if (MapService.Instance.CurrentMapId == 0) return;
            if(UIManager.Instance.UIcnt>0)
            {
                if (mouse_is_diplay) return;
                ToShowCursor();
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                ToShowCursor();
            }
            else ToHideCursor();

        }
        public override void OnStart()
        {
            ToHideCursor();
        }
        /// <summary>
        /// 隐藏鼠标
        /// </summary>
        public void ToHideCursor()
        {
            
            Cursor.visible = false;
            mouse_is_diplay = false;
        }
        /// <summary>
        /// 显示鼠标
        /// </summary>
        public void ToShowCursor()
        {
            Cursor.visible = true;
            mouse_is_diplay = true;
        }
    }

}
