using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager :MonoSingleton<LoadManager>
{

    [Header("登录界面")]
    public GameObject Load_Panel;
    [Header("注册界面")]
    public GameObject Enroll_Panel;
    [Header("登录移动脚本")]
    public UI_appear Load_appear;
    [Header("注册移动脚本")]
    public UI_appear Enroll_appear;
    [Header("遮罩panel")]
    public GameObject Mask_Panel;
    public enum state
    {
        ENROLL,LOAD
    }

    [Header("登录界面图片")]
    public Image load_panel_image;
    [Header("注册界面图片")]
    public Image enroll_panel_image;

    [Header("虚实化速度")]
    public float v;


    public override void OnAwake()
    {

        Load_appear.start_move_r_action += Load_appear.appear_start;
        Load_appear.start_move_l_action += Load_appear.dis_appear_start;

        Enroll_appear.start_move_r_action += Enroll_appear.appear_start;
        Enroll_appear.start_move_l_action += Enroll_appear.dis_appear_start;



        Enroll_appear.after_move_l_action += Load_appear.move_to_r;
        Enroll_appear.after_move_l_action += delegate ()
        {
            Enroll_Panel.SetActive(false);
            Load_Panel.SetActive(true);
            Load_appear.move_to_r();
        };

        Load_appear.after_move_l_action += Enroll_appear.move_to_r;
        Load_appear.after_move_l_action += delegate ()
        {
            Load_Panel.SetActive(false);
            Enroll_Panel.SetActive(true);
            Enroll_appear.move_to_r();
        };
        
        Load_appear.after_move_r_action += delegate ()
        {
            StopCoroutine("Login_panel");
            Mask_Panel.SetActive(false);
            Enroll_appear.stop_all_Coroutine();
            Load_appear.stop_all_Coroutine();
        };
        Enroll_appear.after_move_r_action += delegate ()
        {
            StopCoroutine("Login_panel");
            Mask_Panel.SetActive(false);
            Enroll_appear.stop_all_Coroutine();
            Load_appear.stop_all_Coroutine();
        };
    }
    public override void OnStart()
    {
        //Now_State = state.LOAD;
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
                    Mask_Panel.SetActive(true);
                    Load_appear.move_to_l();
                    StartCoroutine("Login_panel");
                    break;
                case state.LOAD:
                    Mask_Panel.SetActive(true);
                    Enroll_appear.move_to_l();
                    StartCoroutine("Login_panel");

                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Login_panel()
    {
        while(true)
        {
            float ea = enroll_panel_image.color.a;
            if (now_state == state.LOAD)
            {

                //Debug.Log("load");
                ea = Mathf.Lerp(ea, 0f, v * Time.deltaTime);

            }
            else
            {

                //Debug.Log("enroll");
                ea = Mathf.Lerp(ea, 1.0f, v * Time.deltaTime);
            }
            enroll_panel_image.color = new Color(enroll_panel_image.color.r, enroll_panel_image.color.b, enroll_panel_image.color.g, ea);
            yield return new WaitForEndOfFrame();
        }
        

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
