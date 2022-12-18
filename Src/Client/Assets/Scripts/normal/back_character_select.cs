using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using Assets.Scripts.Managers;
using Services;

public class back_character_select : MonoBehaviour {
    
    public void onchick_back_character_select()
    {
        MapService.Instance.CurrentMapId = 0;
        MouseManager.Instance.ToShowCursor();

        UserService.Instance.SendCharacterLeave();

        MySceneManager.Instance.LoadScene("CharSelect");

        
    }
   
}
