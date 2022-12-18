using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{

    public class MouseManager : MonoSingleton<MouseManager>
    {

        private bool mouse_is_diplay;
        public bool Mouse_Is_Display
        {
            get { return mouse_is_diplay; }
        }
        private void Update()
        {
            if (MapService.Instance.CurrentMapId == 0) return;
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
