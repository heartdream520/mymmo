using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager :MonoSingleton<LoadManager>
{

    [Header("登录界面")]
    public GameObject Load_Panel;
    [Header("注册界面")]
    public GameObject Enroll_Panel;
    public enum state
    {
        ENROLL,LOAD
    }

    private state now_state;
    public state Now_State
    {
        get { return now_state; }
        set
        {
            now_state = value;
            switch (now_state)
            {
                case state.ENROLL:
                    Load_Panel.SetActive(false);
                    Enroll_Panel.SetActive(true);
                    break;
                case state.LOAD:
                    Load_Panel.SetActive(true);
                    Enroll_Panel.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
    private void Start()
    {
        Now_State = state.LOAD;
    }

    public void onchick_goto_enroll_button()
    {
        Now_State = state.ENROLL;
    }
    public void onchick_goto_load_button()
    {
        Now_State = state.LOAD;
    }

}
