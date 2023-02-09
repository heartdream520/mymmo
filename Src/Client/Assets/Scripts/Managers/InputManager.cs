using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

class InputManager : MonoSingleton<InputManager>
{
    public bool IsInInputing = false;
    public List<InputField> inputFields;
    private void Update()
    {
        foreach(var i in this.inputFields)
        {
            if(i.isFocused)
            {
                this.IsInInputing = true;
                return;
            }
        }
        this.IsInInputing = false;
    }
}
